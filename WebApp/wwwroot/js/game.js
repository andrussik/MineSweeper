let board = document.getElementById("board");

function loadBoard() {
    // Options menu
    let optionsDiv = document.createElement("div");
    optionsDiv.setAttribute("id", "optionsDiv");
    optionsDiv.style.width = GameBoard.Cols * 20 + "px";
    
    let settings = document.createElement("span");
    settings.setAttribute("id", "settings");
    settings.textContent = "settings";
    optionsDiv.appendChild(settings);
    
    optionsDiv.append(" | ");

    let saveGame = document.createElement("span");
    saveGame.setAttribute("id", "saveGame");
    saveGame.innerText = "save";
    optionsDiv.appendChild(saveGame);
    
    optionsDiv.append(" | ");
    
    let loadGame = document.createElement("span");
    loadGame.setAttribute("id", "loadGame");
    loadGame.innerText = "load";
    optionsDiv.appendChild(loadGame);
    
    // Board head
    let BoardHead = document.createElement("div");
    BoardHead.setAttribute("id", "boardHead");
    BoardHead.style.width = GameBoard.Cols * 20 + "px";
    
    let restartButton = document.createElement("div");
    restartButton.setAttribute("id", "restartButton");
    BoardHead.appendChild(restartButton);
    
    board.appendChild(optionsDiv);
    board.appendChild(BoardHead);

    // Game Board
    let id = 0;
    for (let row = 0; row < GameBoard.Rows; row++) {
        let rowDiv = document.createElement("div");
        rowDiv.setAttribute("id", "boardRow");

        for (let col = 0; col < GameBoard.Cols; col++) {
            let button = document.createElement("div");
            button.setAttribute("id", id.toString());

            rowDiv.appendChild(button);
            id++;
        }

        board.appendChild(rowDiv);
    }
}

function setCellAttributes() {
    for (let i = 0; i < GameBoard.Rows; i++) {
        for (let j = 0; j < GameBoard.Cols; j++) {
            const cell = GameBoard.Board[i][j];
            let button = document.getElementById(cell.CellNumber);

            if (cell.IsFlagged) {
                button.setAttribute("class", "flagged");
            } else if (!cell.IsRevealed) {
                button.setAttribute("class", "unRevealed");
            } else if (cell.IsRevealed) {
                openCell(cell);
            }
        }
    }
}

// left click on cell to reveal
// left click on dice to restart game
board.onclick = (e) => {
    const target = e.target;
    
    if (!GameBoard.GameLost && !GameBoard.GameWon) {
        const row = Math.floor(target.id / GameBoard.Cols);
        const col = target.id % GameBoard.Cols;
        uncover(row, col)
    } 
    if (target === document.getElementById("restartButton")) {
        document.getElementById("restartButton").style.backgroundImage = "url('css/icons/game-die.png')";
        gameBoard(GameBoard.Rows, GameBoard.Cols, GameBoard.TotalMines);
        setCellAttributes();
    }
};

// Right click to add flag
board.oncontextmenu = (e) => {
    e.preventDefault();
    if (!GameBoard.GameLost && !GameBoard.GameWon) {
        const target = e.target;
        let button = document.getElementById(target.id);
        const row = Math.floor(target.id / GameBoard.Cols);
        const col = target.id % GameBoard.Cols;
        const cell = GameBoard.Board[row][col];
        
        if (!cell.IsRevealed) {
            if (!cell.IsFlagged) {
                button.setAttribute("class", "flagged");
            } else {
                button.setAttribute("class", "unRevealed");
            }
            GameBoard.Board[row][col].IsFlagged = !cell.IsFlagged;
        }
    }
};

function openCell(cell) {
    let button = document.getElementById(cell.CellNumber);
    
    if (cell.IsMine) {
        button.setAttribute("class", "openedMine");
    }
    if (!cell.IsMine) {
        if (cell.AdjacentMines === 0) {
            button.setAttribute("class", "open0");
        } else if (cell.AdjacentMines === 1) {
            button.setAttribute("class", "open1");
        } else if (cell.AdjacentMines === 2) {
            button.setAttribute("class", "open2");
        } else if (cell.AdjacentMines === 3) {
            button.setAttribute("class", "open3");
        } else if (cell.AdjacentMines === 4) {
            button.setAttribute("class", "open4");
        } else if (cell.AdjacentMines === 5) {
            button.setAttribute("class", "open5");
        } else if (cell.AdjacentMines === 6) {
            button.setAttribute("class", "open6");
        } else if (cell.AdjacentMines === 7) {
            button.setAttribute("class", "open7");
        } else if (cell.AdjacentMines === 8) {
            button.setAttribute("class", "open8");
        }
    }
}

function revealMines() {
    for (let i = 0; i < GameBoard.Rows; i++) {
        for (let j = 0; j < GameBoard.Cols; j++) {
            GameBoard.Board[i][j].IsRevealed = true;
            const cell = GameBoard.Board[i][j];
            let button = document.getElementById(cell.CellNumber);
            
            if (cell.IsMine) {
                button.setAttribute("class", "openedMine")
            }
        }
    }
}
