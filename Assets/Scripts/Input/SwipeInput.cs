using UnityEngine;

using Dev.Krk.MemoryFlow.Game;

public abstract class SwipeInput : MonoBehaviour {

    [SerializeField]
    private GameController game;

    [SerializeField]
    private float MinSwipeLength = 10.0f;

    [SerializeField]
    private float MinSwipeDiff = 5.0f;

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
            Vector2 swipe = end - start;

            float ax = Mathf.Abs(swipe.x);
            float ay = Mathf.Abs(swipe.y);
            if (ax > MinSwipeLength && ax - ay > MinSwipeDiff)
            {
                if (swipe.x > 0.0F)
                {
                    game.MoveRight();
                    return;
                }
                if (swipe.x < 0.0F)
                {
                    game.MoveLeft();
                    return;
                }
            }
            if (ay > MinSwipeLength && ay - ax > MinSwipeDiff)
            {
                if (swipe.y > 0.0F)
                {
                    game.MoveUp();
                    return;
                }
                if (swipe.y < 0.0F)
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
