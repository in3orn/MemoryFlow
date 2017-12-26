using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
	public delegate void StartedAction();
	public event StartedAction OnStarted;

    public delegate void FinishedAction();
    public event FinishedAction OnFinished;

    public delegate void FailedAction();
    public event FailedAction OnFailed;

	public delegate void DiedAction();
	public event DiedAction OnDied;

    private enum StateEnum
    {
        Idle = 0,
        Showing,
        Playing,
        Finished,
        Failed
    }

    private StateEnum state;

    [SerializeField]
	private LevelProvider levelProvider;

	[SerializeField]
	private FieldMap fieldMap;

    [SerializeField]
    private Player player;

	[SerializeField]
	private Showable start;

    [SerializeField]
	private Finishable finish;

    [SerializeField]
    private GameObject center;

    [SerializeField]
    private float finishDuration = 1f;

	public int HorizontalLength {
		get { return fieldMap.HorizontalLength; }
	}

	public int VerticalLength {
		get { return fieldMap.VerticalLength; }
	}

    void Start()
    {
		fieldMap.OnShown += startGame;
		fieldMap.OnHidden += FinishGame;

		finish.OnFinished += FinishLevel;
    }

    public void Init(int level)
    {
		fieldMap.Init (levelProvider.GetMapData(level));

		int sx = fieldMap.HorizontalLength - 1;
		int sy = fieldMap.VerticalLength - 1;

		finish.Init(new Vector2 (sx, sy) * Field.SIZE, fieldMap.ShowInterval, fieldMap.HideInterval);
		player.Init (Vector2.zero, fieldMap.ShowInterval, fieldMap.HideInterval);

        initCenter();

        state = StateEnum.Idle;
		fieldMap.ShowPreview ();
		showActors ();

		OnStarted ();
    }

	public void Clear() {
		fieldMap.Clear ();
	}

    private void initCenter()
    {
		center.transform.position = new Vector2((fieldMap.HorizontalLength - 1) * Field.SIZE, (fieldMap.VerticalLength - 1) * Field.SIZE) * 0.5f;
    }

    public bool CanMoveLeft()
    {
		return CanMove() && CanMoveLeft(player);
    }

    public bool CanMoveRight()
    {
        return CanMove() && CanMoveRight(player);
    }

    public bool CanMoveUp()
    {
        return CanMove() && CanMoveUp(player);
    }

    public bool CanMoveDown()
    {
        return CanMove() && CanMoveDown(player);
    }

	public bool CanMoveLeft(Player player) {
		Vector2 position = getFieldPosition (player) + Vector2.left;
		return fieldMap.CanMoveLeft ((int) position.x, (int) position.y);
	}

	public bool CanMoveRight(Player player) {
		Vector2 position = getFieldPosition (player);
		return fieldMap.CanMoveRight ((int) position.x, (int) position.y);
	}

	public bool CanMoveUp(Player player) {
		Vector2 position = getFieldPosition (player);
		return fieldMap.CanMoveUp ((int) position.x, (int) position.y);
	}

	public bool CanMoveDown(Player player) {
		Vector2 position = getFieldPosition (player) + Vector2.down;
		return fieldMap.CanMoveDown ((int) position.x, (int) position.y);
	}

    public bool CanMove()
    {
        return (state == StateEnum.Showing || state == StateEnum.Playing) && player.CanMove();
    }

    public void MoveLeft()
    {
        if (CanMoveLeft())
        {
			if(performAction (getLeftField (player))) player.MoveLeft();
        }
    }

    public void MoveRight()
    {
        if (CanMoveRight())
        {
			if(performAction (getRightField (player))) player.MoveRight();
        }
    }

    public void MoveUp()
    {
        if (CanMoveUp())
        {
			if(performAction (getUpField (player))) player.MoveUp();
        }
    }

    public void MoveDown()
    {
        if (CanMoveDown())
        {
			if(performAction (getDownField (player))) player.MoveDown();
        }
    }

	private Field getLeftField(Player player)
	{
		Vector2 pos = getFieldPosition(player) + Vector2.left;
		int x = (int)pos.x;
		int y = (int)pos.y;
		return fieldMap.GetHorizontalField(x, y);
	}

	private Field getRightField(Player player)
	{
		Vector2 pos = getFieldPosition(player);
		int x = (int)pos.x;
		int y = (int)pos.y;
		return fieldMap.GetHorizontalField(x, y);
	}

	public Field getUpField(Player player)
	{
		Vector2 pos = getFieldPosition(player);
		int x = (int)pos.x;
		int y = (int)pos.y;
		return fieldMap.GetVerticalField(x, y);
	}

	private Field getDownField(Player player)
	{
		Vector2 pos = getFieldPosition(player) + Vector2.down;
		int x = (int)pos.x;
		int y = (int)pos.y;
		return fieldMap.GetVerticalField(x, y);
	}

    private Vector2 getFieldPosition(Player player)
    {
        int x = Mathf.RoundToInt(player.transform.position.x / Field.SIZE);
        int y = Mathf.RoundToInt(player.transform.position.y / Field.SIZE);

        return new Vector2(x, y);
    }

	private bool performAction(Field field)
    {
		if (state == StateEnum.Showing)
		{
			state = StateEnum.Playing;
			fieldMap.ShowPlayMode();
		}
		if (!field.Valid) {
			field.Break ();
			RestartLevel ();

			return false;
		} else {
			field.Unmask ();
		}

		return true;
    }

	private void startGame() {
		if (state == StateEnum.Idle || state == StateEnum.Failed) {
			state = StateEnum.Showing;
		}
	}

    public void FinishLevel()
    {
        state = StateEnum.Finished;
		fieldMap.Hide();
		hideActors ();
    }

	public void RestartLevel()
	{
		OnDied ();
	}

    public void FailLevel()
    {
        state = StateEnum.Failed;
		fieldMap.Hide();
		hideActors ();
    }

	public void FinishGame() {
		StartCoroutine (finishGame ());
	}

	private IEnumerator finishGame()
    {
		yield return new WaitForSeconds (finishDuration);
        switch (state)
        {
            case StateEnum.Finished:
                OnFinished();
                break;
            case StateEnum.Failed:
                OnFailed();
                break;
        }
    }

	private void showActors() 
	{
		player.Show ();
		start.Show ();
		finish.Show ();
	}

	private void hideActors()
	{
		player.Hide ();
		start.Hide ();
		finish.Hide ();
	}
}
