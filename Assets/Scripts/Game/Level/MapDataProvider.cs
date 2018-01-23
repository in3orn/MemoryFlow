using UnityEngine;
using Dev.Krk.MemoryFlow.Data;
using Dev.Krk.MemoryFlow.Data.Initializers;

namespace Dev.Krk.MemoryFlow.Game.Level
{
    public class MapDataProvider : MonoBehaviour
    {
        [SerializeField]
        private FlowsDataInitializer flowsInitializer;

        [SerializeField]
        private JsonFieldMapDataFactory mapDataFactory;

        public FieldMapData GetMapData(int flow, int level)
        {
            FlowData flowData = flowsInitializer.Data.Flows[flow];
            LevelData levelData = flowData.Levels[level];
            return mapDataFactory.Create(levelData);
        }
    }

}