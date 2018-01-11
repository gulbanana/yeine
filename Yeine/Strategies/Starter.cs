using System;
using System.Collections.Generic;
using System.Linq;
using Yeine.API;
using Yeine.State;

namespace Yeine.Strategies
{
    /// <summary>Adapted from Riddles.io sample code as a control</summary>
    public class Starter : IStrategy
    {
        private Random Random;

        public Starter()
        {
            Random = new Random();
        }

        /// <summary>Performs a random Birth or Kill move (if possible)</summary>
        public Move Act(Game state)
        {
            var cellMap = CreateMap(state);

            if (Random.NextDouble() < 0.5 && cellMap[state.OurID].Count > 1)
            {
                return DoRandomBirthMove(state, cellMap);
            }
            else
            {
                return DoRandomKillMove(state, cellMap);
            }
        }

        /// <summary>Selects one dead cell and two of own living cells a random to birth a new cell on at the point of the dead cell</summary>
        private Move DoRandomBirthMove(Game state, Dictionary<char, List<Point>> cellMap)
        {
            var myId = state.OurID;
            var deadCells = cellMap['.'];
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

        /// <summary>Selects one random living cell on the field and kills it</summary>
        private Move DoRandomKillMove(Game state, Dictionary<char, List<Point>> cellMap)
        {
            var myId = state.OurID;
            var opponentId = state.TheirID;
            var livingCells = cellMap[myId].Concat(cellMap[opponentId]).ToList();

            if (livingCells.Count <= 0)
            {
                return Move.Pass();
            }

            var randomLiving = livingCells[Random.Next(livingCells.Count)];

            return Move.Kill(randomLiving);
        }

        private Dictionary<char, List<Point>> CreateMap(Game state)
        {
            var cellMap = new Dictionary<char, List<Point>>()
            {
                {'.', new List<Point>()},
                {'0', new List<Point>()},
                {'1', new List<Point>()},
            };

            for (int x = 0; x < state.Field.Width; x++)
            {
                for (int y = 0; y < state.Field.Height; y++)
                {
                    var cell = state.Field.Cells[x,y];

                    cellMap[cell].Add(new Point(x, y));
                }
            }

            return cellMap;
        }
    }
}