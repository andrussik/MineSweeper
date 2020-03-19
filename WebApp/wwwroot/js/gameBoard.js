let GameBoard = {};

function make2DArray(rows, cols) {
    const arr = new Array(rows);
    for (let i = 0; i < arr.length; i++) {
        arr[i] = new Array(cols);
    }
    return arr;
}

function gameBoard(rows, cols, totalMines){
    const Board = make2DArray(rows, cols);

    let id = 0;
    for (let row = 0; row < rows; row++){
        for (let col = 0; col < cols; col++){
            Board[row][col] = cell(id, row, col);
            id++;
        }
    }
    
    GameBoard.Board = Board;
    GameBoard.Rows = rows;
    GameBoard.Cols = cols;
    GameBoard.TotalMines = totalMines;
    GameBoard.TotalRevealed = 0;
    GameBoard.GameLost = false;
    GameBoard.GameWon = false;
    GameBoard.FirstMoveDone = false;

    Board[Math.floor(rows/2)][Math.floor(cols/2)].IsSelected = true;
}

function firstMove(row, col){
    if (isValid(row, col)) { 
        GameBoard.Board[row][col].IsRevealed = true;
    }
    if (isValid(row, col - 1)) {
        GameBoard.Board[row][col - 1].IsRevealed = true;
    }
    if (isValid(row, col + 1)) {
        GameBoard.Board[row][col + 1].IsRevealed = true;
    }
    if (isValid(row + 1, col)) {
        GameBoard.Board[row + 1][col].IsRevealed = true;
    }
    if (isValid(row + 1, col - 1)) {
        GameBoard.Board[row + 1][col - 1].IsRevealed = true;
    }
    if (isValid(row + 1, col + 1)) {
        GameBoard.Board[row + 1][col + 1].IsRevealed = true;
    }
    if (isValid(row - 1, col - 1)) {
        GameBoard.Board[row - 1][col - 1].IsRevealed = true;
    }
    if (isValid(row - 1, col + 1)) {
        GameBoard.Board[row - 1][col + 1].IsRevealed = true;
    }
    if (isValid(row - 1, col)) {
        GameBoard.Board[row - 1][col].IsRevealed = true;
    }
    
    generateMines();
    countAdjacentMines();

    if (isValid(row, col)) {
        openCell(GameBoard.Board[row][col]);
    }
    if (isValid(row, col - 1)) {
        openCell(GameBoard.Board[row][col - 1]);
    }
    if (isValid(row, col + 1)) {
        openCell(GameBoard.Board[row][col + 1]);
    }
    if (isValid(row + 1, col)) {
        openCell(GameBoard.Board[row + 1][col]);
    }
    if (isValid(row + 1, col - 1)) {
        openCell(GameBoard.Board[row + 1][col - 1]);
    }
    if (isValid(row + 1, col + 1)) {
        openCell(GameBoard.Board[row + 1][col + 1]);
    }
    if (isValid(row - 1, col - 1)) {
        openCell(GameBoard.Board[row - 1][col - 1]);
    }
    if (isValid(row - 1, col + 1)) {
        openCell(GameBoard.Board[row - 1][col + 1]);
    }
    if (isValid(row - 1, col)) {
        openCell(GameBoard.Board[row - 1][col]);
    }

    GameBoard.FirstMoveDone = true;
}

function generateMines(){
    const min = 0;
    const max = GameBoard.Rows * GameBoard.Cols - 1;

    for (let i = 0; i < GameBoard.TotalMines; i++) {
        const id = Math.floor(Math.random() * (max - min + 1)) + min;
        const row = Math.floor(id / GameBoard.Cols);
        const col = id % GameBoard.Cols;

        if (!GameBoard.Board[row][col].IsMine && !GameBoard.Board[row][col].IsRevealed) {
            GameBoard.Board[row][col].IsMine = true;
        }
        else {
            i--;
        }
    }
}

function countAdjacentMines() {
    for (let i = 0; i < GameBoard.Rows; i++) {
        for (let j = 0; j < GameBoard.Cols; j++) {
            let count = 0;
            const row = i;
            const col = j;
            if (isValid(row - 1, col) && GameBoard.Board[row - 1][col].IsMine) count++;
            if (isValid(row + 1, col) && GameBoard.Board[row + 1][col].IsMine) count++;
            if (isValid(row, col - 1) && GameBoard.Board[row][col - 1].IsMine) count++;
            if (isValid(row, col + 1) && GameBoard.Board[row][col + 1].IsMine) count++;
            if (isValid(row - 1, col + 1) && GameBoard.Board[row - 1][col + 1].IsMine) count++;
            if (isValid(row + 1, col - 1) && GameBoard.Board[row + 1][col - 1].IsMine) count++;
            if (isValid(row + 1, col + 1) && GameBoard.Board[row + 1][col + 1].IsMine) count++;
            if (isValid(row - 1, col - 1) && GameBoard.Board[row - 1][col - 1].IsMine) count++;
            GameBoard.Board[i][j].AdjacentMines = count;
        }
    }
}

function uncover(row, col) {
    if (!GameBoard.FirstMoveDone && isValid(row, col))
    {
        firstMove(row, col);
    }
    
    else if (isValid(row, col)) {
        const cell = GameBoard.Board[row][col];
        if (cell.IsRevealed) {
            return;
        }

        GameBoard.Board[row][col].IsRevealed = true;
        openCell(cell);

        if (cell.IsRevealed && cell.IsMine)
        {
            GameBoard.Board[row][col].IsSelected = false;
            GameBoard.GameLost = true;
            document.getElementById("restartButton").style.backgroundImage = "url('css/icons/dizzy-face_1f635.png')";
            revealMines();
        }
        
        GameBoard.TotalRevealed++;

        if (GameBoard.TotalRevealed === GameBoard.Rows * GameBoard.Cols - GameBoard.TotalMines)
        {
            GameBoard.Board[row][col].IsSelected = false;
            GameBoard.GameWon = true;
            document.getElementById("restartButton").style.backgroundImage = "url('css/icons/grinning-face-with-star-eyes_1f929.png')";
            revealMines();
        }

        if (cell.AdjacentMines > 0) return;

        uncover(row-1, col);
        uncover(row+1, col);
        uncover(row, col-1);
        uncover(row, col+1);
    }
}

function isValid(row, col) {
    return (row >= 0) && (row < GameBoard.Rows) && (col >= 0) && (col < GameBoard.Cols)
}
