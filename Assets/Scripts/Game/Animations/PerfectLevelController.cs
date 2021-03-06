﻿using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Game.Animations
{
    public class PerfectLevelController : MonoBehaviour
    {
        [SerializeField]
        private LevelController levelController;

        [SerializeField]
        private LivesController livesController;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Text text;

        [SerializeField]
        private string[] completedTexts;

        private bool perfect;

        private int index;

        void OnEnable()
        {
            levelController.OnLevelStarted += ProcessLevelStarted;
            levelController.OnLevelCompleted += ProcessLevelCompleted;

            livesController.OnLivesUpdated += ProcessLivesUpdated;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnLevelStarted -= ProcessLevelStarted;
                levelController.OnLevelCompleted -= ProcessLevelCompleted;
            }

            if (livesController != null)
            {
                livesController.OnLivesUpdated -= ProcessLivesUpdated;
            }
        }

        private void ProcessLevelStarted()
        {
            perfect = true;
        }

        private void ProcessLevelCompleted()
        {
            if (perfect)
            {
                index += Random.Range(1, completedTexts.Length-1);
                index %= completedTexts.Length;
                text.text = completedTexts[index];
                animator.SetTrigger("OnShow");
            }
        }

        private void ProcessLivesUpdated()
        {
            perfect = livesController.Lives == livesController.StartLives;
        }
    }
}
