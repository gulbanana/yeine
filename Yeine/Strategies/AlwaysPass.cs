using System;
using Yeine.API;
using Yeine.State;

namespace Yeine.Strategies
{
    public class AlwaysPass : IStrategy
    {
        public Move Act(Game state, Action<string> report) => Move.Pass();

        public override string ToString() => nameof(AlwaysPass);
    }
}