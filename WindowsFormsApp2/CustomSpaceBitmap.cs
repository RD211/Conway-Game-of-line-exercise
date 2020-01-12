using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public static class BitmapExtensions
    {
        static readonly Dictionary<int, SpaceBoard.CellState> ColorToEnum = new Dictionary<int, SpaceBoard.CellState>()
        {
            {Color.Red.ToArgb(),SpaceBoard.CellState.Dead },
            {Color.Blue.ToArgb(), SpaceBoard.CellState.Alive },
            {Color.White.ToArgb(),SpaceBoard.CellState.Empty }
        };
        static Dictionary<SpaceBoard.CellState, int> EnumToColor = ColorToEnum.ToDictionary((i) => i.Value, (i) => i.Key);
        public static SpaceBoard.CellState GetCellStatus(this DirectBitmap bitmap, int x, int y)
        {
            return ColorToEnum[bitmap.GetPixel(x, y).ToArgb()];
        }
        public static void SetCellStatus(this DirectBitmap bitmap, int x, int y, SpaceBoard.CellState state) => bitmap.SetPixel(x, y,Color.FromArgb( EnumToColor[state]));
    }
}
