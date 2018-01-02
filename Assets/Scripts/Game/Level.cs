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

    public delegate void MovedAction();
    public event MovedAction OnMoved;

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
	private Finishable finish;

    [SerializeField]
    private GameObject center;

    [SerializeField]
    private float finishDuration = 1f;

    private Vector2 playerActualPosition;

    private Queue<Vector2> queuedMoves;

    private Queue<Field> queuedFields;


    public int HorizontalLength {
		get { return fieldMap.HorizontalLength; }
	}

	public int VerticalLength {
		get { return fieldMap.VerticalLength; }
	}

    void Awake()
    {
        queuedMoves = new Queue<Vector2>(5);
        queuedFields = new Queue<Field>(5);
    }

    void Start()
    {
        player.OnMoved += afterMove;

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
        playerActualPosition = player.transform.position;

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
		return CanMove() && canMoveLeft(player);
    }

    public bool CanMoveRight()
    {
        return CanMove() && canMoveRight(player);
    }

    public bool CanMoveUp()
    {
        return CanMove() && canMoveUp(player);
    }

    public bool CanMoveDown()
    {
        return CanMove() && canMoveDown(player);
    }

    public bool CanMove()
    {
        return state == StateEnum.Showing || state == StateEnum.Playing;
    }

    private bool canMoveLeft(Player player) {
		Vector2 position = getFieldPosition (playerActualPosition) + Vector2.left;
		return fieldMap.CanMoveLeft ((int) position.x, (int) position.y);
	}

    private bool canMoveRight(Player player) {
		Vector2 position = getFieldPosition (playerActualPosition);
		return fieldMap.CanMoveRight ((int) position.x, (int) position.y);
	}

    private bool canMoveUp(Player player) {
		Vector2 position = getFieldPosition (playerActualPosition);
		return fieldMap.CanMoveUp ((int) position.x, (int) position.y);
	}

    private bool canMoveDown(Player player) {
		Vector2 position = getFieldPosition (playerActualPosition) + Vector2.down;
		return fieldMap.CanMoveDown ((int) position.x, (int) position.y);
	}

    public void MoveLeft()
    {
        if (CanMoveLeft())
        {
            move(getLeftField(player), Vector2.left);
        }
    }

    public void MoveRight()
    {
        if (CanMoveRight())
        {
            move(getRightField(player), Vector2.right);
        }
    }

    public void MoveUp()
    {
        if (CanMoveUp())
        {
            move(getUpField(player), Vector2.up);
        }
    }

    public void MoveDown()
    {
        if (CanMoveDown())
        {
            move(getDownField(player), Vector2.down);
        }
    }

    private void move(Field field, Vector2 vector)
    {
        playerActualPosition += vector * Field.SIZE;

        if (queuedFields.Count == 0 && player.CanMove())
        {
            performMove(field, vector);
        }
        else
        {
            queuedFields.Enqueue(field);
            queuedMoves.Enqueue(vector);
        }

        OnMoved();
    }

    private void performMove(Field field, Vector2 vector)
    {
        if (performAction(field))
        {
            player.Move(vector * Field.SIZE);
        }
        else
        {
            playerActualPosition = player.transform.position;
            queuedFields.Clear();
            queuedMoves.Clear();
        }
    }

    private void afterMove()
    {
        if(queuedMoves.Count > 0)
        {
            performMove(queuedFields.Dequeue(), queuedMoves.Dequeue());
        }
    }

	private Field getLeftField(Player player)
	{
		Vector2 position = getFieldPosition(playerActualPosition) + Vector2.left;
		return fieldMap.GetHorizontalField((int)position.x, (int)position.y);
	}

	private Field getRightField(Player player)
	{
		Vector2 position = getFieldPosition(playerActualPosition);
        return fieldMap.GetHorizontalField((int)position.x, (int)position.y);
    }

	public Field getUpField(Player player)
	{
		Vector2 position = getFieldPosition(playerActualPosition);
        return fieldMap.GetVerticalField((int)position.x, (int)position.y);
    }

	private Field getDownField(Player player)
	{
		Vector2 position = getFieldPosition(playerActualPosition) + Vector2.down;
        return fieldMap.GetVerticalField((int)position.x, (int)position.y);
    }

    private Vector2 getFieldPosition(Vector2 position)
    {
        return position / Field.SIZE;
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
		finish.Show ();
	}

	private void hideActors()
	{
		player.Hide ();
		finish.Hide ();
	}
}
