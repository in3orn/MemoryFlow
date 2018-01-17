﻿using UnityEngine;
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
            scoreLabel.text = scoreController.GlobalScore.ToString("# ##0");
        }
    }
}
