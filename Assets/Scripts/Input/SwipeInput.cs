using UnityEngine;

public abstract class SwipeInput : MonoBehaviour {

    [SerializeField]
    private Game game;

    [SerializeField]
    private float MinSwipeLength = 50.0f;

    private Vector2 start;

    private bool down = false;
    
    void Awake() {
        init();
	}

    protected virtual void init()
    {
        enabled = isSupported();
    }

    protected abstract bool isSupported();

    void Update()
    {
        if (isInputDown())
        {
            start = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            down = true;
            return;
        }

        if (down && isInputUp())
        {
            down = false;

            Vector2 end = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (Mathf.Abs(end.x - start.x) < MinSwipeLength || Mathf.Abs(end.y - start.y) < MinSwipeLength)
            {
                if (end.x - start.x > MinSwipeLength)
                {
                    game.MoveRight();
                    return;
                }
                if (end.x - start.x < -MinSwipeLength)
                {
                    game.MoveLeft();
                    return;
                }
                if (end.y - start.y > MinSwipeLength)
                {
                    game.MoveUp();
                    return;
                }
                if (end.y - start.y < -MinSwipeLength)
                {
                    game.MoveDown();
                    return;
                }
            }
        }
    }

    protected abstract bool isInputDown();

    protected abstract bool isInputUp();
}
