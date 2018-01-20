﻿using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Game.GUI
{
    public class CurrentScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private Text scoreLabel;

        void Start()
        {
            UpdateScoreLabel();
        }
        
        void OnEnable()
        {
            scoreController.OnScoreUpdated += UpdateScoreLabel;
        }

        void OnDisable()
        {
            if(scoreController != null)
            {
                scoreController.OnScoreUpdated -= UpdateScoreLabel;
            }
        }

        private void UpdateScoreLabel()
        {
            NumberFormatInfo format = CultureInfo.InvariantCulture.NumberFormat.Clone() as NumberFormatInfo;
            format.NumberGroupSeparator = " ";
            scoreLabel.text = scoreController.CurrentScore.ToString("#,0", format);
        }
    }
}
