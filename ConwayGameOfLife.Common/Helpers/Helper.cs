using ConwayGameOfLife.DatabaseModels;

namespace ConwayGameOfLife.Common
{
    public static class Helper
    {
        public static bool AreBoardsEqual(Board a, Board b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
                return false;

            for (int row = 0; row < a.Height; row++)
            {
                for (int col = 0; col < a.Width; col++)
                {
                    if (a.Cells[row, col] != b.Cells[row, col])
                        return false;
                }
            }

            return true;
        }
    }
}
