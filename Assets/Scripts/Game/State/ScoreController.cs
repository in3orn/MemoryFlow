using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryFlow.Resources;

namespace Dev.Krk.MemoryFlow.Game.State
{
    public class ScoreController : ResourcesInitializer
    {
        public UnityAction OnScoreUpdated;

        private readonly string SCORE = "Score";

        [SerializeField]
        private int transferIterations;

        [SerializeField]
        private float transferFrequency;

        private int globalScore;

        private int currentScore;

        public int GlobalScore { get { return globalScore; } }

        public int CurrentScore { get { return currentScore; } }

        void OnDisable()
        {
            PlayerPrefs.SetInt(SCORE, globalScore);
        }

        public override void Init()
        {
            globalScore = PlayerPrefs.GetInt(SCORE);
            initialized = true;
            if (OnInitialized != null) OnInitialized();
        }

        public void IncreaseScore()
        {
            currentScore++;
            if (OnScoreUpdated != null) OnScoreUpdated();
        }

        public void TransferScore()
        {
            StartCoroutine(TransferScoreInternal());
        }

        private IEnumerator TransferScoreInternal()
        {
            int dScore = currentScore > transferIterations ? currentScore / transferIterations : 1;
            while (currentScore > dScore)
            {
                globalScore += dScore;
                currentScore -= dScore;

                if (OnScoreUpdated != null) OnScoreUpdated();

                yield return new WaitForSeconds(transferFrequency);
            }

            globalScore += currentScore;
            currentScore = 0;

            if (OnScoreUpdated != null) OnScoreUpdated();
        }
    }
}