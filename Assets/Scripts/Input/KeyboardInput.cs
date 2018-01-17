using UnityEngine;
using System.Collections;

using Dev.Krk.MemoryFlow.Game;

public class KeyboardInput : MonoBehaviour {

	[SerializeField]
	private GameController game;

    void Awake()
    {
        init();
    }

    protected void init()
    {
        enabled = isSupported();
    }

    protected bool isSupported()
    {
        return Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer;
    }

    void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			game.MoveUp ();
			return;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			game.MoveDown ();
			return;
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			game.MoveLeft();
			return;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			game.MoveRight ();
			return;
		}
	}
}
