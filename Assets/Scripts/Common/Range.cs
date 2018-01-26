using System;

namespace Dev.Krk.MemoryFlow.Common
{
    [Serializable]
    public class Range<T>
    {
        public T Min;

        public T Max;
    }
}