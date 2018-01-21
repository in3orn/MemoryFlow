using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryFlow.Game.State;

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
            levelController.OnLevelEnded+= ProcessLevelEnded;

            levelController.OnPlayerMoved += ProcessPlayerMoved;
            levelController.OnPlayerFailed += ProcessPlayerDied;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnLevelEnded -= ProcessLevelEnded;

                levelController.OnPlayerMoved -= ProcessPlayerMoved;
                levelController.OnPlayerFailed -= ProcessPlayerDied;
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

        private void ProcessLevelEnded()
        {
            if(levelController.State == LevelController.StateEnum.Failed)
            {
                ProcessLevelFailed();
            }
            else
            {
                ProcessLevelCompleted();
            }
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
            //TODO could be done directly in livesController? but what with failing level :P
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
            levelController.Init(progressController.Flow, progressController.Level);
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