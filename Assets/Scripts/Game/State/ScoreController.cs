using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryFlow.Resources;

namespace Dev.Krk.MemoryFlow.Game.State
{
    public class ScoreController : ResourcesInitializer
    {
        public UnityAction OnScoreUpdated;
        public UnityAction OnScoreTransferred;

        private readonly string SCORE = "Score";

        [SerializeField]
        private int transferIterations;

        [SerializeField]
        private float transferFrequency;

        private int globalScore;

        private int currentScore;

        public int GlobalScore { get { return globalScore; } }

        public int CurrentScore { get { return currentScore; } }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveData();
        }

        void OnApplicationQuit()
        {
            SaveData();
        }

        public override void Init()
        {
            LoadData();
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

                if (OnScoreTransferred != null) OnScoreTransferred();

                yield return new WaitForSeconds(transferFrequency);
            }

            globalScore += currentScore;
            currentScore = 0;

            if (OnScoreTransferred != null) OnScoreTransferred();
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt(SCORE, globalScore);
        }

        private void LoadData()
        {
            globalScore = PlayerPrefs.GetInt(SCORE);
        }
    }
}