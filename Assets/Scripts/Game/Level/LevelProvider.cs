using UnityEngine;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Game.Level
{
    public class LevelProvider : MonoBehaviour
    {
        [SerializeField]
        private JsonFieldMapDataFactory mapDataFactory;

        public FieldMapData GetMapData(int level)
        {
            FieldMapData data = mapDataFactory.Create(level);

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
            else if(r < 0.5f)
            {
                data.ReflectByCenter();
            }

            return data;
        }
    }

}