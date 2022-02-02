using System.Threading;

namespace Core
{
    public static class Counter
    {
        private static int _counterValue = 0;

        public static void Increment()
        {
            Interlocked.Increment(ref _counterValue);
        }

        public static int CounterValue => _counterValue;
    }
}
