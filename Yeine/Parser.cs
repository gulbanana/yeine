using System;
using Yeine.State;

namespace Yeine
{
    /// <summary>updates state and signals responses</returns>
    public class Parser
    {
        private readonly Game state;
        
        public Parser(Game state)
        {
            this.state = state;
        }

        /// <returns>time to respond, or null if no response is required</returns>
        public int? Command(string line)
        {
            var parts = line.Split(' ');

            switch (parts[0])
            {
                case "action":
                    return int.Parse(parts[2]);

                case "settings":
                    Settings(parts[1], parts[2]);
                    return null;

                case "update":
                    if (parts[1].Equals("game"))
                    {
                        UpdateGame(parts[2], parts[3]);
                    }
                    else
                    {
                        UpdatePlayer(parts[1], parts[2], parts[3]);
                    }
                    return null;

                default:
                    Console.Error.WriteLine($"unknown command {parts[0]}");
                    return null;
            }
        }

        private void Settings(string key, string value)
        {
            switch (key)
            {
                case "timebank":
                    int time = int.Parse(value);
                    state.MaxTimebank = time;
                    state.CurrentTimebank = time;
                    break;

                case "time_per_move":
                    state.TimePerMove = int.Parse(value);
                    break;

                case "player_names":
                    string[] names = value.Split(',');
                    state.Player0.Name = names[0];
                    state.Player1.Name = names[1];
                    break;

                case "your_bot":
                    state.MyName = value;
                    break;

                case "your_botid":
                    state.MyID = value;
                    state.TheirID = (2 - (int.Parse(value) + 1)) + "";
                    break;

                case "field_width":
                    state.FieldWidth = int.Parse(value);
                    break;

                case "field_height":
                    state.FieldHeight = int.Parse(value);
                    break;

                case "max_rounds":
                    state.MaxRounds = int.Parse(value);
                    break;

                default:
                    Console.Error.WriteLine($"unknown settings key '{key}'");
                    break;
            }
        }

        private void UpdateGame(string key, string value)
        {
            switch (key)
            {
                case "round":
                    state.RoundNumber = int.Parse(value);
                    break;

                case "field":
                    state.ParseField(value);
                    break;

                default:
                    Console.Error.WriteLine($"unknown update key 'game {key}'");
                    break;
            }
        }

        private void UpdatePlayer(string name, string key, string value)
        {
            var player = state.Player0.Name == name ? state.Player0 : state.Player1;

            switch (key)
            {
                case "living_cells":
                    player.LivingCells = int.Parse(value);
                    break;

                case "move":
                    player.LastMove = value;
                    break;

                default:
                    Console.Error.WriteLine($"unknown update key '{name} {key}'");
                    break;
            }
        }
    }
}