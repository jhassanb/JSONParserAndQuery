using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace Project1
{
    //class for all of the statistics that are under visiting statistics and home statistics 
    public class GameStats
    {
        public string StatIDCode { get; set; }
        public string GameCode { get; set; }
        public int TeamCode { get; set; }
        public string GameDate { get; set; }
        public int RushYds { get; set; }
        public int RushAtt { get; set; }
        public int PassYds { get; set; }
        public int PassAtt { get; set; }
        public int PassComp { get; set; }
        public int Penalties { get; set; }
        public int PenaltYds { get; set; }
        public int FumblesLost { get; set; }
        public int InterceptionsThrown { get; set; }
        public int FirstDowns { get; set; }
        public int ThridDownAtt { get; set; }
        public int ThirdDownConver { get; set; }
        public int FourthDownAtt { get; set; }
        public int FourthDownConver { get; set; }
        public int TimePoss { get; set; }
        public int Score { get; set; }
    }

    //class for all game data put together 
    public class AllGameData
    {

        public bool Neutral { get; set; }
        public string VisTeamName { get; set; }
        public GameStats VisStats { get; set; }
        public string HomeTeamName { get; set; }
        public GameStats HomeStats { get; set; }
        public bool IsFinal { get; set; }
        public string Date { get; set; }

    }

    //reads and deserializes data from the JSON file into a C# object
    public class GameDataReader
    {
        public static List<AllGameData> ReadGameDataFromFile(string filePath)
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<AllGameData>>(jsonData);
        }
    }

    //class with function to add new game stats to the JSON file (does this by appending new object to the C# list and then serializing it and writing back to the JSON file)
    public class GameStatsManipulation
    {
        public static void AddNewGameStats(List<AllGameData> allGameDataList, string jsonFilePath)
        {
            Console.WriteLine("Enter visiting team name: ");
            string userVisTeamName = Console.ReadLine();

            Console.WriteLine("Enter home team name: ");
            string userHomeTeamName = Console.ReadLine();

            // Create a new game entry with only team names and game code
            AllGameData newGameData = new AllGameData
            {
                VisTeamName = userVisTeamName,
                HomeTeamName = userHomeTeamName
            };

            // Add the new game entry to the list
            allGameDataList.Add(newGameData);


            // Serialize and write back to the JSON file
            string updatedJsonData = JsonConvert.SerializeObject(allGameDataList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonFilePath, updatedJsonData);

            Console.WriteLine("New game entry added successfully!");
        }
    }

    //class with function that searches through all unique instances of game data and displays what the user asked for
    public class GameFinder
    {
        public void FindAndDisplayGameStats(List<AllGameData> allGameDataList, string userVisTeamName, string userHomeTeamName)
        {
            bool gameFound = false;

            foreach (var gameData in allGameDataList)
            {
                if ((gameData.VisTeamName.Equals(userVisTeamName, StringComparison.OrdinalIgnoreCase) && gameData.HomeTeamName.Equals(userHomeTeamName, StringComparison.OrdinalIgnoreCase)))
                {
                    gameFound = true;
                    Console.WriteLine("Visiting team vs Home team: " + gameData.VisTeamName + " vs " + gameData.HomeTeamName);
                    Console.WriteLine("Game Date: " + gameData.Date);
                    Console.WriteLine("End Score: " + gameData.VisStats.Score + " to " + gameData.HomeStats.Score);
                    Console.WriteLine("Visiting team stats: ");
                    GameStatsPrinter.PrintGameStats(gameData.VisStats);
                    Console.WriteLine("Home team stats: ");
                    GameStatsPrinter.PrintGameStats(gameData.HomeStats);
                    Console.WriteLine();
                }
            }

            if (!gameFound)
            {
                Console.WriteLine("No game found for the specified team names.");
            }
        }
    }

    //function to print all of the stats so we can reuse it for home and visiting team each time we print
    public class GameStatsPrinter
    {
        public static void PrintGameStats(GameStats gameStats)
        {
            Console.WriteLine("     Rushing Yards: " + gameStats.RushYds);
            Console.WriteLine("     Rushing Attempt: " + gameStats.RushAtt);
            Console.WriteLine("     Passing Yards: " + gameStats.PassYds);
            Console.WriteLine("     Passing Attempts: " + gameStats.PassAtt);
            Console.WriteLine("     Passing Completions: " + gameStats.PassComp);
            Console.WriteLine("     Penalties: " + gameStats.Penalties);
            Console.WriteLine("     Penalty Yards: " + gameStats.PenaltYds);
            Console.WriteLine("     Fumbles Lost: " + gameStats.FumblesLost);
            Console.WriteLine("     Interceptions Thrown: " + gameStats.InterceptionsThrown);
            Console.WriteLine("     First Downs: " + gameStats.FirstDowns);
            Console.WriteLine("     Thrid Down Attempts: " + gameStats.ThridDownAtt);
            Console.WriteLine("     Third Down Conversions: " + gameStats.ThirdDownConver);
            Console.WriteLine("     Four Down Attempts: " + gameStats.FourthDownAtt);
            Console.WriteLine("     Four Down Conversions: " + gameStats.FourthDownConver);
            Console.WriteLine("     Time of Possession: " + gameStats.TimePoss);
        }

    }

    internal class JSONProgram
    {
        static void Main()
        {
            //putting the file path for the JSON file that is going to be parsed (change the file path to the file on your computer so it works correctly)
            string jsonFilePath = "C:\\Users\\janna\\OneDrive\\Desktop\\CIS681\\team49ers_season2020_b.txt";

            //creation of a list of all the game data
            //done by deserializing the objects in the file so we can read it all as plain text
            List<AllGameData> allGameDataList = GameDataReader.ReadGameDataFromFile(jsonFilePath);

            //introduce user to the program
            Console.WriteLine("Welcome to the 2020 Football Season Hub!");

            bool exitProgram = false;

            while (!exitProgram)
            {
                //prompting the user to make a choice from the menu
                Console.WriteLine("What would you like to do? ");
                Console.WriteLine("1) Look up game stats");
                Console.WriteLine("2) Add new game stats");
                Console.WriteLine("3) Exit");
                string userChoice = Console.ReadLine();
                int userChoiceInt;
                int.TryParse(userChoice, out userChoiceInt);

                switch (userChoiceInt)
                {
                    //looking up statistics 
                    case 1:
                        //prompt user to enter the visiting team name
                        Console.WriteLine("Enter visiting team name: ");
                        string userVisTeamName = Console.ReadLine();

                        //prompt user to enter the home team name
                        Console.WriteLine("Enter home team name: ");
                        string userHomeTeamName = Console.ReadLine();

                        GameFinder gameFinder = new GameFinder();
                        gameFinder.FindAndDisplayGameStats(allGameDataList, userVisTeamName, userHomeTeamName);

                        break;
                    //adding new stats
                    case 2:
                        Console.WriteLine("Please enter the new game infromation you want to add to the records");
                        GameStatsManipulation.AddNewGameStats(allGameDataList, jsonFilePath);
                        break;
                    //exiting the program 
                    case 3:
                        exitProgram = true;
                        break;
                    //error handling if user puts in the a menu choice that doesn't exist
                    default:
                        Console.WriteLine("Invalid choice. Please choose a valid option.");
                        break;
                }
            }
        }
    }
}