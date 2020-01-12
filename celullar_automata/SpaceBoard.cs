using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public class SpaceBoard
    {
        public enum CellState
        {
            Alive,
            Dead,
            Empty
        }
        public static DirectBitmap boardImage;
        private static Point focusPoint;
        private static double zoom;
        private int widthImg, heightImg;
        public Bitmap RetrieveBitmap(int frameWidth,int frameHeight)
        {
            Bitmap newBitmap = boardImage.Bitmap.Clone(new Rectangle(focusPoint.X,focusPoint.Y, (int)(zoom* widthImg),(int)(zoom* heightImg)),boardImage.Bitmap.PixelFormat);
            Bitmap resBitmap = new Bitmap(frameWidth, frameHeight);
            Graphics g = Graphics.FromImage(resBitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(newBitmap, 0, 0, frameWidth, frameHeight);
            return resBitmap;
        }
        public SpaceBoard(int width, int height)
        {
            this.widthImg = width;
            this.heightImg = height;
            boardImage = new DirectBitmap(widthImg, heightImg);
            Graphics g = Graphics.FromImage(boardImage.Bitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            Brush brsh = new SolidBrush(Color.White);
            g.FillRectangle(brsh, 0, 0, widthImg, heightImg);
            zoom = 1;
            focusPoint = new Point(0, 0);
        }
        public int CountVecini(int x, int y)
        {
            int counter = 0;
            if (x > 0)
                counter += boardImage.GetCellStatus(x - 1, y) == CellState.Alive ? 1 : 0;
            if (x < boardImage.Width - 1)
                counter += boardImage.GetCellStatus(x + 1, y) == CellState.Alive ? 1 : 0;

            if (y < boardImage.Height - 1)
                counter += boardImage.GetCellStatus(x, y + 1) == CellState.Alive ? 1 : 0;
            if (y > 0)
                counter += boardImage.GetCellStatus(x, y - 1) == CellState.Alive ? 1 : 0;

            if (x > 0 && y > 0)
                counter += boardImage.GetCellStatus(x - 1, y - 1) == CellState.Alive ? 1 : 0;
            if (x < boardImage.Width-1 && y< boardImage.Height-1)
                counter += boardImage.GetCellStatus(x + 1, y + 1) == CellState.Alive ? 1 : 0;

            if (x < boardImage.Width-1 && y > 0)
                counter += boardImage.GetCellStatus(x + 1, y - 1) == CellState.Alive ? 1 : 0;
            if (x > 0 && y < boardImage.Height-1)
                counter += boardImage.GetCellStatus(x - 1, y + 1) == CellState.Alive ? 1 : 0;
            return counter;
        }
        public void RandomizeBoard()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks*DateTime.Now.Second*DateTime.Now.Second);

            for (int i = 0; i < boardImage.Bitmap.Height; i++)
            {
                for (int j = 0; j < boardImage.Bitmap.Width; j++)
                {
                    if(rnd.Next(0, 3)==1)
                    {
                        boardImage.SetCellStatus(j, i, CellState.Alive);
                    }
                    else
                    {
                        boardImage.SetCellStatus(j, i, CellState.Empty);
                    }

                }
            }
        }
        public void IterateOnce()
        {
            List<Point> newAlivePoints = new List<Point>();
            List<Point> newDeadPoints = new List<Point>();
            for(int i = 0;i<boardImage.Bitmap.Height;i++)
            {
                for(int j = 0;j<boardImage.Bitmap.Width;j++)
                {
                    int countVec = CountVecini(j, i);
                    if (boardImage.GetCellStatus(j, i) == CellState.Alive)
                    {
                        if (!(countVec == 2 || countVec == 3))
                            newDeadPoints.Add(new Point(j, i));
                    }
                    else
                    {
                        if (countVec == 3)
                            newAlivePoints.Add(new Point(j, i));
                    }
                }
            }
            newAlivePoints.ForEach((p) =>boardImage.SetCellStatus(p.X,p.Y,CellState.Alive));
            newDeadPoints.ForEach((p) => boardImage.SetCellStatus(p.X, p.Y, CellState.Empty));

        }
        public void AlterFocusPoint(int dX,int dY)
        {
            dX /= 5;
            dY /= 5;
            if (focusPoint.X+dX < 0 ||
                focusPoint.X+dX + zoom* widthImg >= widthImg ||
                focusPoint.Y+dY  < 0 ||
                focusPoint.Y+dY + zoom * heightImg >= heightImg)
                return;
            focusPoint.X += dX;
            focusPoint.Y += dY;
        }
        public void AlterZoom(double newZoom)
        {
            if (focusPoint.X < 0 ||
                focusPoint.X + newZoom * widthImg >= widthImg ||
                focusPoint.Y < 0 ||
                focusPoint.Y + newZoom * heightImg >= heightImg)
                return;
            zoom = newZoom;
        }
        public void PlaceCellAt(Point point, int pictureFrameWidth, int pictureFrameHeight)
        {
            double width = zoom * (double)widthImg;
            double height = zoom * (double)heightImg;
            double placeX = point.X/ (double)pictureFrameWidth;
            double placeY = point.Y/ (double)pictureFrameHeight;
            Point resultingPoint = new Point((int)Math.Round((width * placeX)+ (focusPoint.X)), (int)Math.Round((height * placeY)+(focusPoint.Y)));

            boardImage.SetCellStatus(resultingPoint.X, resultingPoint.Y, CellState.Alive);
        }
    }
}
