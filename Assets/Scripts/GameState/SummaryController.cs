using UnityEngine;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Summary
{
    public class SummaryController : MonoBehaviour
    {
        private enum StateEnum
        {
            Idle,
            Menu
        }

        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private PopUpCanvas summary;

        private StateEnum state;

        void Start()
        {
            state = StateEnum.Idle;
        }

        public void Show()
        {
            summary.Show();
        }

        public void Hide()
        {
            summary.Hide();
        }

        public void OnTapDown()
        {
            if(state == StateEnum.Idle)
            {
                scoreController.TransferScore();
                state = StateEnum.Menu;
            }
        }

        public void OnPlayPressed()
        {
            state = StateEnum.Idle;
            Hide();
        }

        public void OnSettingsPressed()
        {
            Hide();
        }

        public void OnShopPressed()
        {
            Hide();
        }
    }
}