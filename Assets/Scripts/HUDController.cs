using UnityEngine;
using UnityEngine.UI;

using Dev.Krk.MemoryFlow.Game;

public class HUDController : MonoBehaviour {

    [SerializeField]
    private HeartImage[] hearts;

    [SerializeField]
    private Text score;

    [SerializeField]
    private GameController game;

    void Start()
    {
        game.OnFailed += hideHeart;
    }

    private void updateScore()
    {
		foreach (HeartImage heart in hearts) {
			heart.Show ();
		}
    }

    private void hideHeart()
    {
        hearts[game.Lives].Hide();
    }
}
