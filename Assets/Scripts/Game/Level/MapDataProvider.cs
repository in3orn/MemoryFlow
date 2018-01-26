﻿using UnityEngine;
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

            FieldMapData data = mapDataFactory.Create(levelData);

            float r = Random.value;

            if (data.HorizontalFields.GetLength(0) == data.VerticalFields.GetLength(1))
            {
                if (r < 0.25f)
                {
                    data.ReflectByDiagonal();
                }
                else if (r < 0.5f)
                {
                    data.ReflectByContrdiagonal();
                }
                else if (r < 0.75f)
                {
                    data.ReflectByCenter();
                }
            }
            else if (r < 0.5f)
            {
                data.ReflectByCenter();
            }
            
            return data;
        }
    }

}