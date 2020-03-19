using System.Collections.Generic;
using DAL;
using Newtonsoft.Json;
using GameBoard = Domain.GameBoard;

namespace SaveGame
{
    public class GameDb
    {
        public static void InsertDataToDb(GameEngine.GameBoard board, string boardName)
        {
            using var ctx = new AppDbContext();
            
            // Create GameBoard model
            var gameBoardModel = new GameBoard()
            {
                BoardName = boardName,
                JsonString = JsonConvert.SerializeObject(board)
            };

            // Insert GameBoard model and save changes
            ctx.GameBoards.Add(gameBoardModel);
            ctx.SaveChanges();
        }

        public static GameEngine.GameBoard LoadDataFromDb(int boardId)
        {
            using var ctx = new AppDbContext();
            var gameBoard = new GameEngine.GameBoard();

            foreach (var board in ctx.GameBoards)
            {
                if (board.GameBoardId == boardId)
                {
                    gameBoard = JsonConvert.DeserializeObject<GameEngine.GameBoard>(board.JsonString);
                }
            }

            return gameBoard;
        }

        public static Dictionary<int, string> GetSavedGameNames()
        {
            using var ctx = new AppDbContext();
            var savedGames = new Dictionary<int, string>();
            
            foreach (var board in ctx.GameBoards)
            {
                savedGames.Add(board.GameBoardId, board.BoardName);
            }

            return savedGames;
        }

        public static void DeleteGameFromDb(int boardId)
        {
            using var ctx = new AppDbContext();
            
            foreach (var gameBoard in ctx.GameBoards)
            {
                if (gameBoard.GameBoardId == boardId)
                {
                    ctx.Remove(gameBoard);
                }
            }

            ctx.SaveChanges();
        }
    }
}