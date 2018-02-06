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

            levelController.OnFlowCompleted += ProcessFlowCompleted;

            levelController.OnPlayerMoved += ProcessPlayerMoved;
            levelController.OnPlayerFailed += ProcessPlayerDied;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnLevelCompleted -= ProcessLevelCompleted;
                levelController.OnLevelFailed -= ProcessLevelFailed;

                levelController.OnFlowCompleted -= ProcessFlowCompleted;

                levelController.OnPlayerMoved -= ProcessPlayerMoved;
                levelController.OnPlayerFailed -= ProcessPlayerDied;
            }
        }

        public void StartNewRun()
        {
            ResetLevel();
            StartLevel();
        }

        private void ProcessPlayerMoved(Vector2 vector)
        {
            tutorialController.Hide();
        }

        public void ProcessLevelFailed()
        {
            tutorialController.Deactivate();
            progressController.ResetFlow(scoreController.Level);

            if (OnLevelFailed != null) OnLevelFailed();
        }

        private void ProcessLevelCompleted()
        {
            tutorialController.Deactivate();
            scoreController.IncreaseScore();
            progressController.NextMap();

            if (progressController.IsFlowCompleted())
            {
                levelController.CompleteFlow();
            }
            else
            {
                StartCoroutine(StartLevelWithDelay());
            }
        }

        private void ProcessFlowCompleted()
        {
            progressController.NextFlow(scoreController.Level);
            if (OnFlowCompleted != null) OnFlowCompleted();
        }

        private IEnumerator StartLevelWithDelay()
        {
            yield return new WaitForSeconds(1.5f);
            StartLevel();
        }

        public void ProcessPlayerDied()
        {
            if (progressController.Flow > 0)
            {
                livesController.DecreaseLives();
                if (livesController.Lives <= 0)
                {
                    levelController.FailLevel();
                }
            }
        }

        private void ResetLevel()
        {
            livesController.ResetLives();
            levelController.Reset();
        }

        private void StartLevel()
        {
            tutorialController.Activate();
            levelController.Clear();
            levelController.Init(progressController.Flow, progressController.Map);
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