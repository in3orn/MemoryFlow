using UnityEngine;
using Dev.Krk.MemoryFlow.Game;
using Dev.Krk.MemoryFlow.Game.State;
using Dev.Krk.MemoryFlow.Summary;
using Dev.Krk.MemoryFlow.Resources;

namespace Dev.Krk.MemoryFlow.State
{
    public class GameStateController : MonoBehaviour
    {
        [Header("Sections")]
        [SerializeField]
        private GameController game;

        [SerializeField]
        private SummaryController summary;

        [SerializeField]
        private SettingsController settings;

        [SerializeField]
        private ShopController shop;

        [Header("Resources")]
        [SerializeField]
        private ResourcesInitializer initializer;

        [SerializeField]
        private ScoreController scoreController;

        void OnEnable()
        {
            initializer.OnInitialized += ProcessResourcesInitialized;

            game.OnGameCompleted += ProcessGameCompleted;
            game.OnFlowCompleted += ProcessFlowCompleted;
            game.OnLevelFailed += ProcessLevelFailed;
        }

        void OnDisable()
        {
            if(initializer != null)
            {
                initializer.OnInitialized -= ProcessResourcesInitialized;
            }

            if (game != null)
            {
                game.OnGameCompleted -= ProcessGameCompleted;
                game.OnFlowCompleted -= ProcessFlowCompleted;
                game.OnLevelFailed -= ProcessLevelFailed;
            }
        }

        void Start()
        {
            initializer.Init();
        }

        public void PlayGame()
        {
            game.StartNewRun();
        }

        public void ShowSummary()
        {
            summary.Show();
        }

        public void ShowShop()
        {
            shop.Show();
        }

        public void ShowSettings()
        {
            settings.Show();
        }

        private void ProcessGameCompleted()
        {
            //TODO add some congrats dialog or some reward
            //TODO remove bonus
            ShowSummary();
        }

        private void ProcessFlowCompleted()
        {
            ShowSummary();
        }

        private void ProcessLevelFailed()
        {
            ShowSummary();
        }

        private void ProcessResourcesInitialized()
        {
            if(scoreController.GlobalScore > 0)
            {
                ShowSummary();
            }
            else
            {
                PlayGame();
            }
        }
    }
}