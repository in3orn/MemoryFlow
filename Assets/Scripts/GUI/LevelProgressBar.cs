using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Game.GUI
{
    public class LevelProgressBar : MonoBehaviour
    {
        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private Image progressBar;

        [SerializeField]
        private Animator[] animators;

        void Start()
        {
        }

        void OnEnable()
        {
            scoreController.OnScoreTransferred += ProcessScoreUpdated;
            scoreController.OnInitialized += UpdateProgress;
        }

        void OnDisable()
        {
            if (scoreController != null)
            {
                scoreController.OnLevelUpdated -= ProcessScoreUpdated;
                scoreController.OnInitialized -= UpdateProgress;
            }
        }

        private void ProcessScoreUpdated()
        {
            UpdateProgress();
            AnimateScore("UpdateLevel");
        }

        private void UpdateProgress()
        {
            progressBar.fillAmount = Mathf.Clamp(scoreController.LevelScore / (float) scoreController.MaxLevelScore, 0f, 1f);
        }

        private void AnimateScore(string trigger)
        {
            foreach (var animator in animators)
            {
                animator.SetTrigger(trigger);
            }
        }
    }
}
