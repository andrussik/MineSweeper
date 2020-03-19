using System;
using System.Collections.Generic;
using ConsoleUi;
using GameEngine;
using MenuSystem;
using SaveGame;

namespace ConsoleUI
{
    public class Game
    {
        private static readonly Menu MainMenu = new Menu();
        private static readonly Menu StartGameMenu = new Menu();
        private static Menu _saveMenu = default!;
        private static Menu _gameMenu = default!;

        private static void PrintGame(GameBoard board)
        {
            Console.Clear();
            BoardUI.PrintBoard(board);
            Console.WriteLine("SPACEBAR Open Cell");
            Console.WriteLine("F Flag Cell");
            Console.WriteLine("M Game Menu");

            if (board.GameLost)
            {
                CreateGameOverMenu(board, false);
                return;
            }

            if (board.GameWon)
            {
                CreateGameOverMenu(board, true);
                return;
            }

            var command = Console.ReadKey().Key;

            PlayerKeyboardInput(board, command, false);
        }

        private static void PlayerKeyboardInput(GameBoard board, ConsoleKey input, bool done)
        {
            var playerSelectedCell = board.GetPlayerSelectedCell();
            var playerRow = playerSelectedCell.Row;
            var playerCol = playerSelectedCell.Col;
            switch (input)
            {
                case ConsoleKey.UpArrow:
                    if (playerRow - 1 > -1)
                    {
                        playerSelectedCell.IsSelected = false;
                        board.Board[playerRow - 1, playerCol].IsSelected = true;
                    }

                    break;
                case ConsoleKey.DownArrow:
                    if (playerRow + 1 < board.Rows)
                    {
                        playerSelectedCell.IsSelected = false;
                        board.Board[playerRow + 1, playerCol].IsSelected = true;
                    }

                    break;
                case ConsoleKey.RightArrow:
                    if (playerCol + 1 < board.Cols)
                    {
                        playerSelectedCell.IsSelected = false;
                        board.Board[playerRow, playerCol + 1].IsSelected = true;
                    }

                    break;
                case ConsoleKey.LeftArrow:
                    if (playerCol - 1 > -1)
                    {
                        playerSelectedCell.IsSelected = false;
                        board.Board[playerRow, playerCol - 1].IsSelected = true;
                    }

                    break;
                case ConsoleKey.Spacebar:
                    board.Uncover(playerSelectedCell.Row, playerSelectedCell.Col);
                    break;
                case ConsoleKey.F:
                    playerSelectedCell.IsFlagged = !playerSelectedCell.IsFlagged;
                    break;
                case ConsoleKey.M:
                    CreateGameMenu(board);
                    break;
            }

            PrintGame(board);
        }

        public static void StartGame()
        {
            Console.Clear();
            
            MainMenu.Title = "MineSweeper Main Menu";
            MainMenu.MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Title = "Start New Game",
                    Execute = StartGameMenu.Run
                },
                new MenuItem()
                {
                    Title = "Exit",
                    Execute = QuitGame
                }
            };

            StartGameMenu.Title = "Start a new game of MineSweeper";
            StartGameMenu.MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Title = "Beginner",
                    Execute = () => PrintGame(GameMode.Beginner())
                },
                new MenuItem()
                {
                    Title = "Intermediate",
                    Execute = () => PrintGame(GameMode.Intermediate())
                },
                new MenuItem()
                {
                    Title = "Advanced",
                    Execute = () => PrintGame(GameMode.Advanced())
                },
                new MenuItem()
                {
                    Title = "Custom",
                    Execute = () => PrintGame(GameMode.Custom())
                },
                new MenuItem()
                {
                    Title = "Saved Games",
                    Execute = () =>
                    {
                        CreateSaveMenu();
                        _saveMenu.Run();
                    }
                },
                new MenuItem()
                {
                    Title = "Return to Main Menu",
                    Execute = MainMenu.Run
                },
                new MenuItem()
                {
                    Title = "Exit",
                    Execute = QuitGame
                },
            };
            
            MainMenu.Run();
        }

        private static void CreateGameMenu(GameBoard board)
        {
            _gameMenu = new Menu()
            {
                Title = "Game Menu",
                MenuItems = new List<MenuItem>()
                {
                    new MenuItem()
                    {
                        Title = "Continue Game",
                        Execute = () => PrintGame(board)
                    },
                    new MenuItem()
                    {
                        Title = "Save Game",
                        Execute = () => SaveGame(board)
                    },
                    new MenuItem()
                    {
                        Title = "Quit Game",
                        Execute = MainMenu.Run
                    }
                }
            };
            _gameMenu.Run();
        }
        
        private static void CreateSaveMenu()
        {
            _saveMenu = new Menu();
            _saveMenu.Title = "Saved Games";
            _saveMenu.MenuItems = new List<MenuItem>();
            // JSON
//            var savedGameFiles = SaveGameJson.GetSavedFileNames();
            var savedGameFiles = GameDb.GetSavedGameNames();
            foreach (var (key, value) in savedGameFiles)
            {
                Action loadOrDelete = () => CreateLoadMenu(key);
                _saveMenu.MenuItems.Add(new MenuItem()
                {
                    Title = value,
                    Execute = loadOrDelete
                });
            }

            _saveMenu.MenuItems.Add(
                new MenuItem()
                {
                    Title = "Return to Game Menu",
                    Execute = StartGameMenu.Run
                });
            _saveMenu.MenuItems.Add(
                new MenuItem()
                {
                    Title = "Return to Main Menu",
                    Execute = MainMenu.Run
                });
            _saveMenu.MenuItems.Add(
                new MenuItem()
                {
                    Title = "Exit",
                    Execute = QuitGame
                });
        }

        private static void CreateLoadMenu(int boardId)
        {
            var loadMenu = new Menu
            {
                Title = "Loading Game",
                MenuItems = new List<MenuItem>()
                {
                    new MenuItem()
                    {
                        Title = "Load Game", 
                        Execute = () => PrintGame(GameDb.LoadDataFromDb(boardId))
                    },
                    new MenuItem()
                    {
                        Title = "Delete Game", 
                        Execute = () => DeleteGame(boardId)
                    },
                    new MenuItem()
                    {
                        Title = "Go Back", 
                        Execute = () => _saveMenu.Run()
                    }
                }
            };
            
            loadMenu.Run();
        }

        private static void CreateGameOverMenu(GameBoard board, bool won)
        {
            var gameOverMenu = new Menu
            {
                Title = !won ? "GAME OVER! YOU LOST!" : "GAME OVER! CONGRATULATIONS, YOU WON!",
                Execute = () => BoardUI.PrintBoard(board),
                MenuItems = new List<MenuItem>()
                {
                    new MenuItem() {Title = "Start New Game", Execute = StartGameMenu.Run},
                    new MenuItem() {Title = "Return to Main Menu", Execute = MainMenu.Run},
                    new MenuItem() {Title = "Exit", Execute = QuitGame}
                }
            };

            gameOverMenu.Run();
        }

        private static void SaveGame(GameBoard board)
        {
            Console.Clear();
            Console.WriteLine("Saving Game");
            Console.WriteLine("Please Write Name For Your Game And Press Enter");
            Console.WriteLine("Leave Field Empty And Press Enter To Cancel Saving And Return To Menu");
            Console.Write(">");
            var title = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(title))
            {
                GameDb.InsertDataToDb(board, title);
                
                // JSON
                // SaveGameJson.SaveGameState(board, title);
            }
            else
            {
                _gameMenu.Run();
            }

        }
        
        private static void DeleteGame(int boardId)
        {
            GameDb.DeleteGameFromDb(boardId);
            // JSON
            // SaveGameJson.DeleteGame(fileName);

            CreateSaveMenu();
            _saveMenu.Run();
        }

        private static void QuitGame()
        {
            Environment.Exit(0);
        }
    }
}