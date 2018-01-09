using System;
using System.IO;
using Yeine.API;
using Yeine.State;

namespace Yeine
{
    public class Parser
    {
        private readonly Game state;
        
        public Parser(Game state)
        {
            this.state = state;
        }

        public void Settings(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case "timebank":
                        int time = int.Parse(value);
                        state.MaxTimebank = time;
                        state.Timebank = time;
                        break;
                    case "time_per_move":
                        state.TimePerMove = int.Parse(value);
                        break;
                    case "player_names":
                        string[] playerNames = value.Split(',');
                        foreach (string playerName in playerNames)
                            state.Players.Add(playerName, new Player(playerName));
                        break;
                    case "your_bot":
                        state.MyName = value;
                        break;
                    case "your_botid":
                        state.Field.MyID = value;
                        state.Field.OpponentID = (2 - (int.Parse(value) + 1)) + "";
                        break;
                    case "field_width":
                        state.Field.Width = int.Parse(value);
                        break;
                    case "field_height":
                        state.Field.Height = int.Parse(value);
                        break;
                    case "max_rounds":
                        state.MaxRounds = int.Parse(value);
                        break;
                    default:
                        Console.Error.Write($"Cannot parse settings input with key '{key}'");
                        break;
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"Cannot parse settings value '{value}' for key '{key}'");
            }
        }

        public void UpdateGame(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case "round":
                        state.RoundNumber = int.Parse(value);
                        break;
                    case "field":
                        state.Field.ParseFromString(value);
                        break;
                    default:
                        Console.Error.WriteLine($"Cannot parse game data input with key '{key}'");
                        break;
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"Cannot parse game data value '{value}' for key '{key}'");
            }
        }

        public void UpdatePlayer(string playerName, string key, string value)
        {
            Player player;
            
            if (!state.Players.TryGetValue(playerName, out player))
            {
                Console.Error.WriteLine($"Could not find player with name '{playerName}'");
                return;
            }

            try
            {
                switch (key)
                {
                    case "living_cells":
                        player.LivingCells = int.Parse(value);
                        break;
                    case "move":
                        player.previousMove = value;
                        break;
                    default:
                        Console.Error.WriteLine(
                            $"Cannot parse {playerName} data input with key '{key}'");
                        break;
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine(
                    $"Cannot parse {playerName} data value '{value}' for key '{key}'");
            }
        }
    }
}