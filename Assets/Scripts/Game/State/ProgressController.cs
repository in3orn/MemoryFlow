using UnityEngine;
using Dev.Krk.MemoryFlow.Data.Initializers;
using Dev.Krk.MemoryFlow.Data;

namespace Dev.Krk.MemoryFlow.Game.State
{
    public class ProgressController : MonoBehaviour
    {
        [SerializeField]
        private FlowsDataInitializer flowsDataController;

        private int flow;

        private int map;

        public int Flow { get { return flow; } }

        public int Map { get { return map; } }

        void Start()
        {
        }

        public bool IsFlowCompleted()
        {
            return map == flowsDataController.Data.Flows[flow].Levels.Length;
        }

        public void NextMap()
        {
            map++;
        }

        public void NextFlow(int level)
        {
            int max = Mathf.Min(flowsDataController.Data.Flows.Length - 1, level + 1);
            if (flow < max) flow++;
            map = 0;
        }

        public void ResetFlow(int level)
        {
            int min = Mathf.Max(1, level - 1);
            if (flow > min) flow--;
            else flow = min;
            map = 0;
        }
    }
}