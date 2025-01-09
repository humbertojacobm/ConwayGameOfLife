namespace ConwayGameOfLife.DatabaseModels
{
    public class Board
    {
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool[,] Cells { get; set; }
        public int Step { get; set; }
    }
}
