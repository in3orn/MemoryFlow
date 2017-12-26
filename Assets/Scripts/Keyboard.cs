using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour {

	[SerializeField]
	private Game game;

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
