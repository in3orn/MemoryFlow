using System;

namespace Dev.Krk.MemoryFlow.Data
{
    [Serializable]
    public class FlowData
    {
        public int Level;

        public LevelData[] Levels;
    }
}