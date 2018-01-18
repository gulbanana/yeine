namespace Yeine.Arena
{
    class Bot
    {
        public readonly IStrategy Strategy;
        public readonly IEvaluator Evaluator;

        public Bot(IStrategy strategy, IEvaluator evaluator)
        {
            Strategy = strategy;
            Evaluator = evaluator;
        }

        public override string ToString() => $"{Strategy}+{Evaluator}";
    }
}