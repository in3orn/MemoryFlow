using UnityEngine;
using Dev.Krk.MemoryFlow.Game.State;
using Dev.Krk.MemoryFlow.Data.Controller;

public class Game : MonoBehaviour
{
	public delegate void StartedAction();
	public event StartedAction OnStarted;

    public delegate void FinishedAction();
    public event FinishedAction OnFinished;

    public delegate void FailedAction();
    public event FailedAction OnFailed;

    [SerializeField]
    private Level level;

    [SerializeField]
    private int startLives = 3;

	private int lives;

    [SerializeField]
    private int minLevelDifficulty = 10;

    [SerializeField]
    private int failedLevelDrop = 3;

    private int redGems;

    private int greenGems;

    private int yellowGems;

    private int currentLevel;

    private int currentDifficulty;

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
    private LevelsDataController levelsController;

    [SerializeField]
    private FlowsDataController flowsController;

    public int GreenGems
    {
        get { return greenGems; }
    }

    public int YellowGems
    {
        get { return yellowGems; }
    }

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

        levelsController.OnInitialized += ProcessOnInitialized;
        flowsController.OnInitialized += ProcessOnInitialized;
    }

    void OnDisable()
    {
        if(level != null)
        {
            level.OnMoved -= updateTutorial;
            level.OnFinished -= ProcessLevelCompleted;
            level.OnFailed -= ProcessLevelFailed;
            level.OnDied -= updateLives;
        }

        if(levelsController != null)
        {
            levelsController.OnInitialized -= ProcessOnInitialized;
        }

        if (flowsController != null)
        {
            flowsController.OnInitialized -= ProcessOnInitialized;
        }
    }

    void Update()
    {
        if(!tutorialShown && Time.time - startLevelTime > showTutorialDelay)
        {
            tutorialCanvas.Show();
            tutorialShown = true;
        }
    }

    private void ProcessOnInitialized()
    {
        //TODO should listen for progress & fieldmap factory?
        if (!levelsController.Initialized) return;
        if (!flowsController.Initialized) return;

        if (progressController.Flow == 0)
        {
            StartNewRun();
        }
        else
        {
            gameOverCanvas.Show();
        }
    }

	public void StartNewRun()
	{
		currentLevel = 0;
		startLevel ();

		gameOverCanvas.Hide ();
	}

    private void ProcessLevelCompleted()
    {
        greenGems++;
        progressController.NextLevel();

        if (progressController.IsFlowCompleted())
        {
            progressController.NextFlow();

            if (progressController.IsGameCompleted())
            {
                progressController.ResetFlow(greenGems);

                //OnFlowCompleted();
                gameOverCanvas.Show();
            }
            else
            {
                //OnLevelCompleted();
                gameOverCanvas.Show();
            }
        }
        else
        {
            startLevel();
            OnFinished();
        }
    }

    public void ProcessLevelFailed()
    {
        progressController.ResetFlow(greenGems);
        gameOverCanvas.Show();
    }

	public void updateLives()
	{
		lives--;
		if (lives <= 0)
		{
			level.FailLevel ();
		}
		OnFailed();
	}

    private void startLevel()
    {
		lives = startLives;
		level.Clear ();

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
