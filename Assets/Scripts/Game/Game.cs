using UnityEngine;

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
        level.OnFinished += startNextLevel;
		level.OnFailed += endGame;
		level.OnDied += updateLives;
    }

	public void StartNewRun()
	{
		currentLevel = 0;
		startLevel ();

		gameOverCanvas.Hide ();
	}

    private void startNextLevel()
    {
        greenGems++;
        currentLevel++;
        currentDifficulty++;
        startLevel();
        
        OnFinished();
    }

    public void endGame()
    {
        //TODO wait 18h for next live
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
        level.Init(currentLevel);
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
