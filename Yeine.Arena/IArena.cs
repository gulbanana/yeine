using System;

namespace Yeine.Arena
{
    public interface IArena
    {
        void Submit(Match m, Action<MatchResult> f);
    }
}