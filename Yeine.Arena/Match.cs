using System;
using Yeine.State;
using Yeine.Strategies;

namespace Yeine.Arena
{
    public class Match
    {
        private readonly int verbosity;
        public readonly Field CurrentField;
        private int currentRound;
        private bool isP1turn;
        public int Cells0;
        public int Cells1;
        public readonly IStrategy P0;
        public readonly IStrategy P1;
        public readonly Game S0;
        public readonly Game S1;

        public Match(int verbosity, Field startingField, IStrategy player0, IStrategy player1)
        {
            this.verbosity = verbosity;
            this.CurrentField = startingField;
            this.currentRound = 1;
            this.P0 = player0;
            this.P1 = player1;
            this.S0 = new Game { OurName = "player0", OurID = '0', TheirID = '1' };
            this.S1 = new Game { OurName = "player1", OurID = '1', TheirID = '0' };
        }

        public GameResult PlayGame()
        {
            if (verbosity >= 1) Console.WriteLine($"==== {P0} vs {P1} ====");
            CurrentField.Evaluate('0', '1', out Cells0, out Cells1);

            while (currentRound <= 100)
            {
                if (PlayTurn() == TurnResult.GameOver) break;
            }

            var isDraw = currentRound > 100;
            
            if (verbosity >= 1)
            {
                var result = isDraw ? "DRAW" : (Cells0 > Cells1 ? P0 : P1).ToString() + " WIN";
                Console.WriteLine($"{result} in {currentRound-1} rounds - player0 {Cells0}, player1 {Cells1}");
            }

            return isDraw ? GameResult.Draw : Cells0 > Cells1 ? GameResult.Player0Win : GameResult.Player1Win;
        }

        public TurnResult PlayTurn()
        {
            if (!isP1turn)
            {
                var result = PlayTurn(P0, S0);
                isP1turn = true;
                return result;
            }
            else
            {
                var result = PlayTurn(P1, S1);
                isP1turn = false;
                currentRound++;
                return result;
            }
        }

        private TurnResult PlayTurn(IStrategy strat, Game state)
        {
            if (verbosity >= 2) Console.WriteLine(CurrentField.ToString());
            state.ParseField(CurrentField.Width, CurrentField.Height, CurrentField.ToString());
            state.RoundNumber = currentRound;

            var m = strat.Act(state);
            var v1 = EvaluatePosition(state, CurrentField);
            CurrentField.ProcessCommand(m, state.OurID);
            CurrentField.Simulate();
            var v2 = EvaluatePosition(state, CurrentField);
            if (verbosity >= 2) Console.WriteLine($"Round {currentRound}, {state.OurName} {m}, {(v1>0 ? "+" : "")}{v1}->{(v2>0 ? "+" : "")}{v2}");

            CurrentField.Evaluate('0', '1', out Cells0, out Cells1);
            return (Cells0 == 0 || Cells1 == 0) ? TurnResult.GameOver : TurnResult.Continue;
        }
        
        private int EvaluatePosition(Game state, Field position)
        {
            var ours = 0;
            var theirs = 0;

            position.Evaluate(state.OurID, state.TheirID, out ours, out theirs);

            return ours - theirs;
        }
    }
}