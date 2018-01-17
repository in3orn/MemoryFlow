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

        [SerializeField]
        private PopUpCanvas menu;

        private StateEnum state;

        void Start()
        {
            state = StateEnum.Idle;
        }

        public void Show()
        {
            if(state == StateEnum.Idle && scoreController.CurrentScore > 0)
            {
                menu.HideImmediately();
            }
            else
            {
                menu.ShowImmediately();
            }
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
                menu.Show();
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