using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Showable))]
public class Player : MonoBehaviour
{
    public delegate void MovedAction();
    public event MovedAction OnMoved;

    public delegate void DiedAction();
    public event DiedAction OnDied;

    enum StateEnum
    {
        Idle = 0,
        Moving
    }

    [SerializeField]
    private float moveDuration = 0.5f;

    private Queue<Vector2> moves;

    private StateEnum state;

    private CircleCollider2D circleCollider;

	private Showable showable;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
		showable = GetComponent<Showable> ();

        moves = new Queue<Vector2>(2);
    }

	public void Init(Vector2 position, float showInterval, float hideInterval)
    {
        state = StateEnum.Idle;
        
        transform.position = position;
        transform.localScale = Vector3.one;

		showable.ShowDelay = position.x / Field.SIZE + position.y / Field.SIZE * showInterval;
		showable.HideDelay = position.x / Field.SIZE + position.y / Field.SIZE * hideInterval;

        circleCollider.enabled = true;
    }

    public bool CanMove()
    {
        return state != StateEnum.Moving;
    }

    public void Move(Vector2 vector)
    {
        if (state != StateEnum.Moving)
        {
            StartCoroutine(move(transform.position, (Vector2) transform.position + vector));
        }
    }

    private IEnumerator move(Vector2 start, Vector2 end)
    {
        state = StateEnum.Moving;

        float t = 0f;
        while(true)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector2.Lerp(start, end, t);

            if (t >= 1) break;
            yield return null;
        }

        state = StateEnum.Idle;
        OnMoved();
    }

	public void Show()
	{
		showable.Show ();
	}

	public void Hide()
	{
		showable.Hide ();
	}
}
