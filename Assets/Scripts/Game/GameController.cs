using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryFlow.Game.State;
using Dev.Krk.MemoryFlow.Data.Initializers;

namespace Dev.Krk.MemoryFlow.Game
{
    public class GameController : MonoBehaviour
    {
        public UnityAction OnFlowCompleted { get; set; }
        public UnityAction OnGameCompleted { get; set; }
        public UnityAction OnLevelFailed { get; set; }

        public delegate void StartedAction();
        public event StartedAction OnStarted;

        public delegate void FailedAction();
        public event FailedAction OnFailed;

        [SerializeField]
        private LevelController level;

        [SerializeField]
        private int startLives = 3;

        private int lives;

        [SerializeField]
        private int minLevelDifficulty = 10;

        [SerializeField]
        private int failedLevelDrop = 3;



        [SerializeField]
        PopUpCanvas gameOverCanvas;



        [SerializeField]
        PopUpCanvas tutorialCanvas;

        [SerializeField]
        private float showTutorialDelay = 5;

        private float startLevelTime;

        private bool tutorialShown = false;



        [SerializeField]
        private ProgressController progressController;

        [SerializeField]
        private FlowsDataInitializer flowsController;

        [SerializeField]
        private ScoreController scoreController;

        public int Lives
        {
            get { return lives; }
        }

        void Start()
        {
        }

        void OnEnable()
        {
            level.OnMoved += updateTutorial;
            level.OnFinished += ProcessLevelCompleted;
            level.OnFailed += ProcessLevelFailed;
            level.OnDied += updateLives;
        }

        void OnDisable()
        {
            if (level != null)
            {
                level.OnMoved -= updateTutorial;
                level.OnFinished -= ProcessLevelCompleted;
                level.OnFailed -= ProcessLevelFailed;
                level.OnDied -= updateLives;
            }
        }

        void Update()
        {
            if (!tutorialShown && Time.time - startLevelTime > showTutorialDelay)
            {
                tutorialCanvas.Show();
                tutorialShown = true;
            }
        }

        public void StartNewRun()
        {
            startLevel();

            gameOverCanvas.Hide();
        }

        private void ProcessLevelCompleted()
        {
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
                startLevel();
            }
        }

        public void ProcessLevelFailed()
        {
            progressController.ResetFlow(scoreController.GlobalScore);
            if (OnLevelFailed != null) OnLevelFailed();
        }

        public void updateLives()
        {
            lives--;
            if (lives <= 0)
            {
                level.FailLevel();
            }
            OnFailed();
        }

        private void startLevel()
        {
            lives = startLives;
            level.Clear();

            FlowData flowData = flowsController.Data.Flows[progressController.Flow];
            level.Init(flowData.Levels[progressController.Level]);

            startLevelTime = Time.time;
            tutorialShown = false;
        }

        private void updateTutorial()
        {
            if (tutorialShown)
            {
                tutorialCanvas.Hide();
            }
            else
            {
                tutorialShown = true;
            }
        }

        public void MoveLeft()
        {
            level.MoveLeft();
        }

        public void MoveRight()
        {
            level.MoveRight();
        }

        public void MoveUp()
        {
            level.MoveUp();
        }

        public void MoveDown()
        {
            level.MoveDown();
        }
    }
}