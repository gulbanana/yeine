using System;

namespace Yeine.Arena
{
    public class STArena : IArena
    {
        public void Submit(Match m, Action<MatchResult> f)
        {
            f(m.PlayMatch());
        }
    }
}