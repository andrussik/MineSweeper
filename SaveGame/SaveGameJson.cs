using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using GameEngine;
using Newtonsoft.Json;

namespace SaveGame
{
    public class SaveGameJson
    {
        public static readonly string SaveFolder = Directory.GetCurrentDirectory() + "/SavedGames/";
        private const int SavingTime = 1500;
        private const int LoadingTime = 2000;
        
        public static void SaveGameState(GameBoard gameBoard, string fileName)
        {
            fileName = fileName.Trim().ToLower();
            if (!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }

            var saved = false;
            if (File.Exists($"{SaveFolder}{fileName}.json"))
            {
                Console.WriteLine("This name already exists. Do you want to overwrite it?");
                Console.WriteLine("Y - Yes");
                Console.WriteLine("Press any other key to go back.");
                Console.Write(">");
                var cmd = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (cmd == "Y")
                {
                    Console.WriteLine($"Saving game: {fileName}");
                    File.WriteAllText($@"{SaveFolder}{fileName}.json", JsonConvert.SerializeObject(gameBoard));
                    saved = true;
                    Thread.Sleep(SavingTime);
                }
            }
            else
            {
                Console.WriteLine($"Saving game: {fileName}");
                File.WriteAllText($@"{SaveFolder}{fileName}.json", JsonConvert.SerializeObject(gameBoard));
                saved = true;
                Thread.Sleep(SavingTime);
            }
            
            // Check that file was saved
            if (saved)
            {
                if (File.Exists($"{SaveFolder}{fileName}.json"))
                {
                    Console.WriteLine($"Game {fileName} saved successfully!");
                    Thread.Sleep(SavingTime);
                    Console.WriteLine("Press any key to continue game.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"ERROR! Game {fileName} was not saved. Please try again!");
                    Thread.Sleep(SavingTime);
                    Console.WriteLine("Press any key to continue game.");
                    Console.ReadLine();
                }
            }
        }

        public static GameBoard LoadGameState(string fileName)
        {
            var jsonString = File.ReadAllText($"{SaveFolder}{fileName}.json");
            var gameBoard = JsonConvert.DeserializeObject<GameBoard>(jsonString);
            Console.WriteLine($"Loading saved game: {fileName}");
            Thread.Sleep(LoadingTime);
            return gameBoard;
        }

        public static void DeleteGame(string fileName)
        {
            if (File.Exists($"{SaveFolder}{fileName}.json"))
            {
                File.Delete($"{SaveFolder}{fileName}.json");
                if (!File.Exists($"{SaveFolder}{fileName}.json"))
                {
                    Console.WriteLine($"Game {fileName} deleted successfully!");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"ERROR! Game {fileName} was not deleted! Please try again.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("ERROR! File does not exist!");
                Console.WriteLine("Press any key to go back.");
            }
        }

        public static List<string> GetSavedFileNames()
        {
            if (!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }
            
            var fileArray = Directory
                .EnumerateFiles(SaveFolder, "*.json")
                .Select(Path.GetFileNameWithoutExtension)
                .ToArray();
            var savedFileNames = new List<string>(fileArray);
            return savedFileNames;
        }
    }
}