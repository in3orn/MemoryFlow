using UnityEngine;
using Dev.Krk.MemoryFlow.Data.Initializers;
using Dev.Krk.MemoryFlow.Data;

namespace Dev.Krk.MemoryFlow.Game.State
{
    public class ProgressController : MonoBehaviour
    {
        [SerializeField]
        private FlowsDataInitializer flowsDataController;

        private int level;

        private int map;

        public int Level { get { return level; } }

        public int Map { get { return map; } }

        void Start()
        {
        }

        public bool IsLevelCompleted()
        {
            return map == flowsDataController.Data.Flows[level].Levels.Length;
        }

        public bool IsFlowCompleted()
        {
            return level == flowsDataController.Data.Flows.Length ||
                flowsDataController.Data.Flows[level].ScoreLock != flowsDataController.Data.Flows[level + 1].ScoreLock;
        }

        public void NextMap()
        {
            map++;
        }

        public void NextLevel()
        {
            map = 0;
            level++;
        }

        public void NextFlow(int score)
        {
            map = 0;

            if (level == flowsDataController.Data.Flows.Length || score < flowsDataController.Data.Flows[level+1].ScoreLock)
            {
                ResetFlow();
            }
            else
            {
                level++;
            }
        }

        public void ResetFlow()
        {
            map = 0;

            if (level == flowsDataController.Data.Flows.Length)
                level--;

            while(level > 0)
            {
                if (flowsDataController.Data.Flows[level].ScoreLock != flowsDataController.Data.Flows[level - 1].ScoreLock)
                    break;

                level--;
            }
        }
    }
}