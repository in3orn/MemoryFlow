using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Game.GUI
{
    public class GlobalScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private Text scoreLabel;

        [SerializeField]
        private Animator[] animators;

        void Start()
        {
        }
        
        void OnEnable()
        {
            scoreController.OnScoreTransferred += ProcessScoreTransferred;
            scoreController.OnInitialized += UpdateText;
        }

        void OnDisable()
        {
            if(scoreController != null)
            {
                scoreController.OnScoreTransferred -= ProcessScoreTransferred;
                scoreController.OnInitialized -= UpdateText;
            }
        }

        private void ProcessScoreTransferred()
        {
            UpdateText();
            AnimateScore("TransferScore");
        }

        private void UpdateText()
        {
            NumberFormatInfo format = CultureInfo.InvariantCulture.NumberFormat.Clone() as NumberFormatInfo;
            format.NumberGroupSeparator = " ";
            scoreLabel.text = scoreController.CurrentScore.ToString("#,0", format);
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
