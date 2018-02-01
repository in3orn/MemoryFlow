using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Game.GUI
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private Text levelLabel;

        [SerializeField]
        private Animator[] animators;

        void Start()
        {
        }
        
        void OnEnable()
        {
            scoreController.OnLevelUpdated += ProcessLevelUpdated;
            scoreController.OnInitialized += UpdateText;
        }

        void OnDisable()
        {
            if(scoreController != null)
            {
                scoreController.OnLevelUpdated -= ProcessLevelUpdated;
                scoreController.OnInitialized -= UpdateText;
            }
        }

        private void ProcessLevelUpdated()
        {
            UpdateText();
            AnimateScore("UpdateLevel");
        }

        private void UpdateText()
        {
            NumberFormatInfo format = CultureInfo.InvariantCulture.NumberFormat.Clone() as NumberFormatInfo;
            format.NumberGroupSeparator = " ";
            levelLabel.text = (scoreController.Level + 1).ToString("#,0", format);
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
