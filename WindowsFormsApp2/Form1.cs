using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        SpaceBoard board = new SpaceBoard(100,100);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = board.RetrieveBitmap(pictureBox1.Width,pictureBox1.Height);

        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {


        }
        Point startOfDrag;
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            startOfDrag = pictureBox1.PointToClient(MousePosition);
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var point = pictureBox1.PointToClient(MousePosition);
            board.AlterFocusPoint(startOfDrag.X - point.X, startOfDrag.Y - point.Y);
            startOfDrag = point;
            pictureBox1.Image = board.RetrieveBitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Stop();
        }

        private void PictureBox1_DoubleClick(object sender, EventArgs e)
        {
            board.PlaceCellAt(pictureBox1.PointToClient(MousePosition),pictureBox1.Width,pictureBox1.Height);
            pictureBox1.Image = board.RetrieveBitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            board.AlterZoom((double)trackBar1.Value / (double)100);
            pictureBox1.Image = board.RetrieveBitmap(pictureBox1.Width, pictureBox1.Height);

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (timer2.Enabled)
                timer2.Stop();
            else
                timer2.Start();

        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            board.IterateOnce();
            pictureBox1.Image = board.RetrieveBitmap(pictureBox1.Width, pictureBox1.Height);
            GC.Collect();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            board.RandomizeBoard();
            pictureBox1.Image = board.RetrieveBitmap(pictureBox1.Width, pictureBox1.Height);

        }
    }
}
