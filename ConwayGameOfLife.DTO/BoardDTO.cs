namespace ConwayGameOfLife.DTO
{
    public class BoardDTO
    {
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool[,] Cells { get; set; }
    }
}
