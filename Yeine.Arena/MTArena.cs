using System;
using System.Threading;
using System.Threading.Tasks;

namespace Yeine.Arena
{
    public class MTArena : IArena
    {
        private readonly SemaphoreSlim inputThrottle;
        private readonly object outputLock;

        public MTArena()
        {
            inputThrottle = new SemaphoreSlim(8);
            outputLock = new object();
        }

        public void Submit(Match m, Action<MatchResult> f)
        {
            inputThrottle.Wait();
            Task.Run(() =>
            {
                var result = m.PlayMatch();
                inputThrottle.Release();

                lock(outputLock)
                {
                    f(result);
                }
            });
        }
    }
}