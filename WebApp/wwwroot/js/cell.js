function cell(cellNumber, row, col) {
    const cell = {};
    cell.CellNumber = cellNumber;
    cell.Row = row;
    cell.Col = col;
    cell.IsMine = false;
    cell.AdjacentMines = 0;
    cell.IsRevealed = false;
    cell.IsFlagged = false;
    cell.IsSelected = false;
    return cell;
}
