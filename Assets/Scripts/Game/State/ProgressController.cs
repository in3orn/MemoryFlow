using UnityEngine;
using Dev.Krk.MemoryFlow.Data.Initializers;

namespace Dev.Krk.MemoryFlow.Game.State
{
    public class ProgressController : MonoBehaviour
    {
        [SerializeField]
        private FlowsDataInitializer flowsDataController;

        private int flow;

        private int level;

        public int Flow { get { return flow; } }

        public int Level { get { return level; } }

        void Start()
        {
        }

        public bool IsFlowCompleted()
        {
            return level == flowsDataController.Data.Flows[flow].Levels.Length;
        }

        public bool IsGameCompleted()
        {
            return flow == flowsDataController.Data.Flows.Length;
        }

        public void NextLevel()
        {
            level++;
        }

        public void NextFlow()
        {
            level = 0;
            flow++;
        }

        public void ResetFlow(int score)
        {
            level = 0;

            for (flow = 0; flow < flowsDataController.Data.Flows.Length; flow++)
            {
                if (flowsDataController.Data.Flows[flow].ScoreLock < 0) break;
                if (flowsDataController.Data.Flows[flow].ScoreLock > score) break;
            }
        }
    }
}