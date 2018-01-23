using UnityEngine;
using Dev.Krk.MemoryFlow.Game;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Sounds
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField]
        private SoundPlayer soundPlayer;

        [SerializeField]
        private LevelController levelController;

        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private int numOfButtons = 3;
        private int buttonIndex;

        [SerializeField]
        private int numOfSlides = 4;
        private int slideIndex;

        private Vector2 prevVector;

        void Start()
        {

        }

        void OnEnable()
        {
            levelController.OnPlayerMoved += ProcessPlayerMoved;
            levelController.OnPlayerFailed += ProcessPlayerFailed;

            levelController.OnLevelCompleted += ProcessLevelCompleted;
            levelController.OnLevelFailed += ProcessLevelFailed;

            scoreController.OnScoreTransfered += ProcessScoreTransferred;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnPlayerMoved -= ProcessPlayerMoved;
                levelController.OnPlayerFailed -= ProcessPlayerFailed;

                levelController.OnLevelCompleted -= ProcessLevelCompleted;
                levelController.OnLevelFailed -= ProcessLevelFailed;
            }

            if(scoreController != null)
            {
                scoreController.OnScoreTransfered -= ProcessScoreTransferred;
            }
        }

        public void PlayButtonPressed()
        {
            buttonIndex += Random.Range(0, numOfButtons - 1);
            buttonIndex %= numOfButtons;

            soundPlayer.PlaySound(SoundPlayer.SoundId.Button1 + buttonIndex);
        }

        private void ProcessPlayerMoved(Vector2 vector)
        {
            slideIndex += Random.Range(0, numOfSlides - 1);
            slideIndex %= numOfSlides;

            if(IsDirectionChanged(vector))
            {
                soundPlayer.PlaySound(SoundPlayer.SoundId.Slide1 + slideIndex);
            }

            prevVector = vector;
        }

        private bool IsDirectionChanged(Vector2 vector)
        {
            return (vector - prevVector).magnitude > 0.5f;
        }

        private void ProcessPlayerFailed()
        {
            soundPlayer.PlaySound(SoundPlayer.SoundId.Failure);
        }

        private void ProcessLevelCompleted()
        {
            prevVector = Vector2.zero;
            soundPlayer.PlaySound(SoundPlayer.SoundId.LevelComplete);
        }

        private void ProcessLevelFailed()
        {
            soundPlayer.PlaySound(SoundPlayer.SoundId.LevelFailed);
        }

        private void ProcessScoreTransferred()
        {
            soundPlayer.PlaySound(SoundPlayer.SoundId.ScoreTransfer);
        }
    }
}