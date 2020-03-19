using System;

namespace GameEngine
{
    public class GameBoard
    {
        public Cell[,] Board { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int TotalMines { get; set; }
        public int TotalRevealed { get; set; }
        public bool GameLost { get; set; }
        public bool GameWon { get; set; }
        public bool FirstMoveDone { get; set; }
        
        public GameBoard(int rows=9, int cols=9, int totalMines=10)
        {
            Board = new Cell[rows,cols];
            Rows = rows;
            Cols = cols;
            TotalMines = totalMines;
            TotalRevealed = 0;
            GameLost = false;
            GameWon = false;
            FirstMoveDone = false;

            // Generate cells for the game board
            var id = 0;
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    Board[row, col] = new Cell(id, row, col);
                    id++;
                }
            }

            // Selected cell for player
            Board[rows/2, cols/2].IsSelected = true;
        }

        private void FirstMove(int row, int col)
        {
            if (IsValid(row, col)) Board[row, col].IsRevealed = true;
            if (IsValid(row, col - 1)) Board[row, col - 1].IsRevealed = true;
            if (IsValid(row, col + 1)) Board[row, col + 1].IsRevealed = true;
            if (IsValid(row + 1, col)) Board[row + 1, col].IsRevealed = true;
            if (IsValid(row + 1, col - 1)) Board[row + 1, col - 1].IsRevealed = true;
            if (IsValid(row + 1, col + 1)) Board[row + 1, col + 1].IsRevealed = true;
            if (IsValid(row - 1, col - 1)) Board[row - 1, col - 1].IsRevealed = true;
            if (IsValid(row - 1, col + 1)) Board[row - 1, col + 1].IsRevealed = true;
            if (IsValid(row - 1, col)) Board[row - 1, col].IsRevealed = true;

            // Generate mines for the game board
            GenerateMines();
            
            // Count adjacent mines
            CountAdjacentMines();
            
            FirstMoveDone = true;
        }
        
        private void GenerateMines()
        {
            for (var i = 0; i < TotalMines; i++)
            {
                var id = new Random().Next(0, Rows * Cols);
                var row = id / Cols;
                var col = id % Cols;

                if (!Board[row, col].IsMine && !Board[row, col].IsRevealed)
                {
                    Board[row, col].IsMine = true;
                }
                else
                {
                    i--;
                }
            }
        }
        
        private void CountAdjacentMines()
        {
            foreach (var cell in Board)
            {
                var count = 0;
                var row = cell.Row;
                var col = cell.Col;
                if (IsValid(row - 1, col) && Board[row - 1, col].IsMine) count++;
                if (IsValid(row + 1, col) && Board[row + 1, col].IsMine) count++;
                if (IsValid(row, col - 1) && Board[row, col - 1].IsMine) count++;
                if (IsValid(row, col + 1) && Board[row, col + 1].IsMine) count++;
                if (IsValid(row - 1, col + 1) && Board[row - 1, col + 1].IsMine) count++;
                if (IsValid(row + 1, col - 1) && Board[row + 1, col - 1].IsMine) count++;
                if (IsValid(row + 1, col + 1) && Board[row + 1, col + 1].IsMine) count++;
                if (IsValid(row - 1, col - 1) && Board[row - 1, col - 1].IsMine) count++;
                cell.AdjacentMines = count;
            }
        }

        private void RevealMines()
        {
            foreach (var cell in Board)
            {
                if (cell.IsMine) cell.IsRevealed = true;
            }
        }

        public void Uncover(int row, int col) {
            if (!FirstMoveDone && IsValid(row, col))
            {
                FirstMove(row, col);
            }
            
            else if (IsValid(row, col)) {
                var cell = Board[row, col];

                if (cell.IsRevealed) return;
                
                cell.IsRevealed = true;

                if (cell.IsRevealed && cell.IsMine)
                {
                    cell.IsSelected = false;
                    GameLost = true;
                    RevealMines();
                }

                TotalRevealed++;

                if (TotalRevealed == Rows * Cols - TotalMines)
                {
                    cell.IsSelected = false;
                    GameWon = true;
                    RevealMines();
                }
                
                if (cell.AdjacentMines > 0) return;

                Uncover(row-1, col);
                Uncover(row+1, col);
                Uncover(row, col-1);
                Uncover(row, col+1);
            }
        }

        private bool IsValid(int row, int col) 
        { 
            // Returns true if row number and column number is in range 
            return (row >= 0) && (row < Rows) && (col >= 0) && (col < Cols); 
        }

        public Cell GetPlayerSelectedCell()
        {
            var selectedCell = Board[0, 0];
            
            foreach (var cell in Board)
            {
                if (cell.IsSelected)
                {
                    selectedCell = cell;
                }
            }

            return selectedCell;
        }
    }
}
