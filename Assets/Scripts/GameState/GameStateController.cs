using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryFlow.Game;
using Dev.Krk.MemoryFlow.Game.State;
using Dev.Krk.MemoryFlow.Summary;
using Dev.Krk.MemoryFlow.Resources;

namespace Dev.Krk.MemoryFlow.State
{
    public class GameStateController : MonoBehaviour
    {
        public enum StateEnum
        {
            Gameplay = 0,
            Summary,
            Settings
        }

        public UnityAction<StateEnum> OnStateChanged;

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

            game.OnFlowCompleted += ProcessFlowCompleted;
            game.OnLevelFailed += ProcessLevelFailed;
        }

        void OnDisable()
        {
            if (initializer != null)
            {
                initializer.OnInitialized -= ProcessResourcesInitialized;
            }

            if (game != null)
            {
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
            if (OnStateChanged != null) OnStateChanged(StateEnum.Gameplay);
        }

        public void ShowSummary()
        {
            StartCoroutine(ChangeState(summary.Show));
            if (OnStateChanged != null) OnStateChanged(StateEnum.Summary);
        }

        public void ShowShop()
        {
            if (OnStateChanged != null) OnStateChanged(StateEnum.Settings);
        }

        public void ShowSettings()
        {
            StartCoroutine(ChangeState(settings.Show));
            if (OnStateChanged != null) OnStateChanged(StateEnum.Settings);
        }

        private void ProcessFlowCompleted()
        {
            //TODO some congratulations
            ShowSummary();
        }

        private void ProcessLevelFailed()
        {
            ShowSummary();
        }

        private void ProcessResourcesInitialized()
        {
            if (scoreController.LevelScore > 0)
            {
                progressController.NextFlow(scoreController.LevelScore);
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