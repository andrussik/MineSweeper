using System;
using System.Collections.Generic;
using GameEngine;

namespace ConsoleUi
{
    public class BoardUI
    {
        private const string VerticalSeparator = "|";
        private const string HorizontalSeparator = "-";
        private const string CenterSeparator = "+";
        private static readonly ConsoleColor CurrentForeGround = Console.ForegroundColor;
        private static readonly ConsoleColor CurrentBackGround = Console.BackgroundColor;
        private static readonly Dictionary<int, ConsoleColor> NumberColors = new Dictionary<int, ConsoleColor>()
        {
            {1, ConsoleColor.Blue},
            {2, ConsoleColor.DarkGreen},
            {3, ConsoleColor.Red},
            {4, ConsoleColor.Magenta},
            {5, ConsoleColor.Black},
            {6, ConsoleColor.DarkRed},
            {7, ConsoleColor.Gray},
            {8, ConsoleColor.Cyan}
        };

        public static void PrintBoard(GameBoard game)
        {
            Console.Clear();
            
            // Print first row for column indices.
            PrintColumnIndices(game);
            
            // Print game board rows.
            PrintRows(game);
        }

        private static void PrintColumnIndices(GameBoard game)
        {
            Console.Write("  ");
            for (var col = 0; col < game.Cols; col++)
            {
                if (col <= 9)
                {
                    Console.Write("   ");
                    ColoredConsoleWrite((col+1).ToString(), ConsoleColor.Green, CurrentBackGround);
                }
                else
                {
                    Console.Write("  ");
                    ColoredConsoleWrite((col+1).ToString(), ConsoleColor.Green, CurrentBackGround);
                }
            }

            // Add separator
            Console.Write("\n   ");
            for (var col = 0; col < game.Cols; col++)
            {
                Console.Write(CenterSeparator + HorizontalSeparator + HorizontalSeparator + HorizontalSeparator);
                if (col == game.Cols - 1)
                {
                    Console.Write(CenterSeparator);
                }
            }
        }

        private static void PrintRows(GameBoard game)
        {
            Console.Write("\n");
            
            for (var row = 0; row < game.Rows; row++)
            {
                if (row < 9)
                {
                    Console.Write(" ");
                    ColoredConsoleWrite(foreGround: ConsoleColor.Green, text: (row+1).ToString(), backGround: CurrentBackGround);
                    Console.Write(" ");
                    Console.Write(VerticalSeparator);
                }
                else
                {
                    ColoredConsoleWrite(foreGround: ConsoleColor.Green, text: (row+1).ToString(), backGround: CurrentBackGround);
                    Console.Write(" ");
                    Console.Write(VerticalSeparator);
                }

                for (var col = 0; col < game.Cols; col++)
                {
                    PrintCell(game.Board[row, col]);
                    Console.Write(VerticalSeparator);
                }
                
                // Add separator
                Console.Write("\n   ");
                if (row < game.Rows)
                {
                    for (var col = 0; col < game.Cols; col++)
                    {
                        Console.Write(CenterSeparator + HorizontalSeparator + HorizontalSeparator + HorizontalSeparator);
                        if (col == game.Cols - 1)
                        {
                            Console.Write(CenterSeparator);
                        }
                    }

                    Console.Write("\n");
                }
            }
        }

        private static void PrintCell(Cell cell)
        {
            if (cell.IsSelected)
            {
                PrintSelectedCell(cell);
                return;
            }
            ColoredConsoleWrite(" ", CurrentForeGround, CurrentBackGround);
            if (!cell.IsRevealed && cell.IsFlagged)
            {
                ColoredConsoleWrite("F", ConsoleColor.Yellow, CurrentBackGround);
            }
            else if (cell.IsRevealed)
            {
                if (!cell.IsMine)
                {
                    if (cell.AdjacentMines == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        var numberColor = NumberColors[cell.AdjacentMines];
                        var adjacentMines = cell.AdjacentMines.ToString();
                        ColoredConsoleWrite(adjacentMines, numberColor, CurrentBackGround);
                    }
                }
                else
                {
                    Console.Write("*");
                }
            }
            else
            {
                ColoredConsoleWrite("O", CurrentForeGround, CurrentBackGround);
            }

            ColoredConsoleWrite(" ", CurrentForeGround, CurrentBackGround);
        }

        private static void PrintSelectedCell(Cell cell)
        {
            ColoredConsoleWrite(" ", ConsoleColor.White, ConsoleColor.White);
            if (!cell.IsRevealed && cell.IsFlagged)
            {
                ColoredConsoleWrite("F", ConsoleColor.Yellow, ConsoleColor.White);
            }
            else if (cell.IsRevealed)
            {
                if (!cell.IsMine)
                {
                    if (cell.AdjacentMines == 0)
                    {
                        ColoredConsoleWrite(" ", ConsoleColor.White, ConsoleColor.White);
                    }
                    else
                    {
                        var numberColor = NumberColors[cell.AdjacentMines];
                        var adjacentMines = cell.AdjacentMines.ToString();
                        ColoredConsoleWrite(adjacentMines, numberColor, ConsoleColor.White);
                    }
                }
            }
            else
            {
                ColoredConsoleWrite("O", CurrentForeGround, ConsoleColor.White);
            }

            ColoredConsoleWrite(" ", ConsoleColor.White, ConsoleColor.White);
        }

        private static void ColoredConsoleWrite(string text,
            ConsoleColor foreGround, 
            ConsoleColor backGround)
        {
            Console.ForegroundColor = foreGround;
            Console.BackgroundColor = backGround;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}