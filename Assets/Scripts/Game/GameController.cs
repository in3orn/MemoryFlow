using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryFlow.Game.State;
using Dev.Krk.MemoryFlow.Data.Initializers;

namespace Dev.Krk.MemoryFlow.Game
{
    public class GameController : MonoBehaviour
    {
        public UnityAction OnFlowCompleted;
        public UnityAction OnGameCompleted;
        public UnityAction OnLevelFailed;

        [SerializeField]
        private LevelController levelController;
        
        [SerializeField]
        private ProgressController progressController;

        [SerializeField]
        private FlowsDataInitializer flowsController;

        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private LivesController livesController;

        [SerializeField]
        private TutorialController tutorialController;

        void Start()
        {
        }

        void OnEnable()
        {
            levelController.OnMoved += ProcessPlayerMoved;
            levelController.OnFinished += ProcessLevelCompleted;
            levelController.OnFailed += ProcessLevelFailed;
            levelController.OnDied += ProcessPlayerDied;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnMoved -= ProcessPlayerMoved;
                levelController.OnFinished -= ProcessLevelCompleted;
                levelController.OnFailed -= ProcessLevelFailed;
                levelController.OnDied -= ProcessPlayerDied;
            }
        }

        public void StartNewRun()
        {
            livesController.ResetLives();
            StartLevel();
        }

        private void ProcessPlayerMoved()
        {
            tutorialController.Hide();
        }

        private void ProcessLevelCompleted()
        {
            tutorialController.Deactivate();
            scoreController.IncreaseScore();
            progressController.NextLevel();

            if (progressController.IsFlowCompleted())
            {
                progressController.NextFlow();

                if (progressController.IsGameCompleted())
                {
                    progressController.ResetFlow(scoreController.GlobalScore);

                    if (OnGameCompleted != null) OnGameCompleted();
                }
                else
                {
                    if (OnFlowCompleted != null) OnFlowCompleted();
                }
            }
            else
            {
                StartLevel();
            }
        }

        public void ProcessLevelFailed()
        {
            tutorialController.Deactivate();
            progressController.ResetFlow(scoreController.GlobalScore);
            if (OnLevelFailed != null) OnLevelFailed();
        }

        public void ProcessPlayerDied()
        {
            livesController.DecreaseLives();
            if (livesController.Lives <= 0)
            {
                levelController.FailLevel();
            }
        }

        private void StartLevel()
        {
            tutorialController.Activate();
            levelController.Clear();

            FlowData flowData = flowsController.Data.Flows[progressController.Flow];
            levelController.Init(flowData.Levels[progressController.Level]);
        }

        public void MoveLeft()
        {
            levelController.MoveLeft();
        }

        public void MoveRight()
        {
            levelController.MoveRight();
        }

        public void MoveUp()
        {
            levelController.MoveUp();
        }

        public void MoveDown()
        {
            levelController.MoveDown();
        }
    }
}