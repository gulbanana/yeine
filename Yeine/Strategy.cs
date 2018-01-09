using System;
using System.Collections.Generic;
using System.Linq;
using Yeine.API;
using Yeine.State;

namespace Yeine
{
    public class Strategy
    {
        private Random Random;

        public Strategy()
        {
            Random = new Random();
        }

        public Move DoMove(Game state)
        {
            var cellMap = state.Field.GetCellMapping();

            if (Random.NextDouble() < 0.5)
            {
                return DoRandomBirthMove(state, cellMap);
            }
            else
            {
                return DoRandomKillMove(state, cellMap);
            }
        }

        /**
         * Selects one dead cell and two of own living cells a random to birth a new cell
         * on at the point of the dead cell
         */
        private Move DoRandomBirthMove(Game state, Dictionary<string, List<Point>> cellMap)
        {
            var myId = state.Field.MyID;
            var deadCells = cellMap["."];
            var myCells = new List<Point>(cellMap[myId]);

            if (deadCells.Count <= 0 || myCells.Count < 2)
            {
                return DoRandomKillMove(state, cellMap);
            }

            var randomBirth = deadCells[Random.Next(deadCells.Count)];

            var sacrificePoints = new List<Point>();
            for (int i = 0; i < 2; i++)
            {
                int randomIndex = Random.Next(myCells.Count);
                var randomSacrifice = myCells[randomIndex];
                sacrificePoints.Add(randomSacrifice);
                myCells.RemoveAt(randomIndex);
            }

            return Move.Birth(randomBirth, sacrificePoints[0], sacrificePoints[1]);
        }

        /**
         * Selects one random living cell on the field and kills it
         */
        private Move DoRandomKillMove(Game state, Dictionary<string, List<Point>> cellMap)
        {
            var myId = state.Field.MyID;
            var opponentId = state.Field.OpponentID;
            var livingCells = cellMap[myId].Concat(cellMap[opponentId]).ToList();

            if (livingCells.Count <= 0)
            {
                return Move.Pass();
            }

            var randomLiving = livingCells[Random.Next(livingCells.Count)];

            return Move.Kill(randomLiving);
        }
    }
}