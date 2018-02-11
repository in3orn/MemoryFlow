using System;

namespace Dev.Krk.MemoryFlow.Data
{
    [Serializable]
    public class ShapeData
    {
        public string Name;
        public PointData[] HorizontalFields;
        public PointData[] VerticalFields;
    }
}