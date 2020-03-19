using System;

namespace GameEngine
{
    public class GameMode
    {
        public static GameBoard Beginner()
        {
            return new GameBoard();
        }
        
        public static GameBoard Intermediate()
        {
            return new GameBoard(16, 16, 40);
        }
        
        public static GameBoard Advanced()
        {
            return new GameBoard(24, 24, 90);
        }

        public static GameBoard Custom()
        {
            var userRowInt = -1;
            var userColInt = -1;
            var userMineInt = -1;
            // rows
            Console.WriteLine("Enter rows: ");
            while(!int.TryParse(Console.ReadLine(), out userRowInt) 
                  || userRowInt < 1 || userRowInt > 40)
            {
                Console.WriteLine("You entered an invalid number! Rows must be a number between 1 and 40.");
                Console.Write(">");
            }
            // columns
            Console.WriteLine("Enter columns: ");
            while(!int.TryParse(Console.ReadLine(), out userColInt) 
                  || userColInt < 1 || userColInt > 40)
            {
                Console.WriteLine("You entered an invalid number! Columns must be a number between 1 and 40.");
                Console.Write(">");
            }
            // mines
            Console.WriteLine("Enter mines: ");
            while(!int.TryParse(Console.ReadLine(), out userMineInt) || userMineInt < 0)
            {
                Console.WriteLine("You entered an invalid number! Mines must be a number at least 0.");
                Console.Write(">");
            }

            if (userMineInt >= userRowInt * userColInt) userMineInt = userRowInt * userColInt - 1;
            
            var gameBoard = new GameBoard(userRowInt, userColInt, userMineInt);
            
            return gameBoard;
        }
    }
}