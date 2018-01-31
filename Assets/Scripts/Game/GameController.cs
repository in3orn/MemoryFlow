using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Game
{
    public class GameController : MonoBehaviour
    {
        public UnityAction OnFlowCompleted;
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
            levelController.OnLevelCompleted += ProcessLevelCompleted;
            levelController.OnLevelFailed += ProcessLevelFailed;
            //levelController.OnLevelEnded+= ProcessLevelEnded;

            levelController.OnPlayerMoved += ProcessPlayerMoved;
            levelController.OnPlayerFailed += ProcessPlayerDied;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnLevelCompleted -= ProcessLevelCompleted;
                levelController.OnLevelFailed -= ProcessLevelFailed;
                //levelController.OnLevelEnded -= ProcessLevelEnded;

                levelController.OnPlayerMoved -= ProcessPlayerMoved;
                levelController.OnPlayerFailed -= ProcessPlayerDied;
            }
        }

        public void StartNewRun()
        {
            livesController.ResetLives();
            StartLevel();
        }

        private void ProcessPlayerMoved(Vector2 vector)
        {
            tutorialController.Hide();
        }

        public void ProcessLevelFailed()
        {
            tutorialController.Deactivate();
            progressController.ResetFlow();

            if (OnLevelFailed != null) OnLevelFailed();
        }

        private void ProcessLevelCompleted()
        {
            tutorialController.Deactivate();
            scoreController.IncreaseScore();
            progressController.NextMap();

            if (progressController.IsLevelCompleted())
            {
                progressController.NextLevel();

                if (progressController.IsFlowCompleted())
                {
                    progressController.NextFlow(scoreController.LevelScore);
                    if (OnFlowCompleted != null) OnFlowCompleted();
                }
            }
            else
            {
                StartCoroutine(StartLevelWithDelay());
            }
        }

        private IEnumerator StartLevelWithDelay()
        {
            yield return new WaitForSeconds(1.5f);
            StartLevel();
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
            levelController.Init(progressController.Level, progressController.Map);
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