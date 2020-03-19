namespace GameEngine
{
    public class Cell
    {
        public int CellNumber { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsMine { get; set; }
        public int AdjacentMines { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsSelected { get; set; }

        public Cell(int cellNumber, int row, int col)
        {
            CellNumber = cellNumber;
            Row = row;
            Col = col;
        }
    }
}