using System;
using Yeine.API;
using Yeine.State;

namespace Yeine
{
    public interface IStrategy
    {
        Move Act(Game state, Action<string> report);
    }
}