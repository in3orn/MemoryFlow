using System.Collections;
using UnityEngine;
using Dev.Krk.MemoryFlow.Game;
using Dev.Krk.MemoryFlow.Game.State;
using Dev.Krk.MemoryFlow.Summary;
using Dev.Krk.MemoryFlow.Resources;

namespace Dev.Krk.MemoryFlow.State
{
    public class GameStateController : MonoBehaviour
    {
        private delegate void StateChangeAction();

        [Header("Settings")]
        [SerializeField]
        private float delay;

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

        [SerializeField]
        private ProgressController progressController;

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
            StartCoroutine(ChangeState(game.StartNewRun));
        }

        public void ShowSummary()
        {
            StartCoroutine(ChangeState(summary.Show));
        }

        public void ShowShop()
        {
            StartCoroutine(ChangeState(shop.Show));
        }

        public void ShowSettings()
        {
            StartCoroutine(ChangeState(settings.Show));
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
                progressController.ResetFlow(scoreController.GlobalScore);
                ShowSummary();
            }
            else
            {
                PlayGame();
            }
        }

        private IEnumerator ChangeState(StateChangeAction action)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);
            action();
        }
    }
}