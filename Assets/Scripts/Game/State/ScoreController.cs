using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryFlow.Resources;
using System;

namespace Dev.Krk.MemoryFlow.Game.State
{
    public class ScoreController : ResourcesInitializer
    {
        public UnityAction OnLevelUpdated;
        public UnityAction OnScoreUpdated;
        public UnityAction OnScoreTransferred;

        private readonly string LEVEL = "Level";
        private readonly string SCORE = "Score";

        [SerializeField]
        private int transferIterations;

        [SerializeField]
        private float transferFrequency;

        private int level;

        private int levelScore;

        private int currentScore;

        private int maxLevelScore;


        public int Level { get { return level; } }

        public int LevelScore { get { return levelScore; } }

        public int CurrentScore { get { return currentScore; } }

        public int MaxLevelScore { get { return maxLevelScore; } }


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
                levelScore += dScore;
                currentScore -= dScore;

                UpdateLevel();

                if (OnScoreTransferred != null) OnScoreTransferred();

                yield return new WaitForSeconds(transferFrequency);
            }

            levelScore += currentScore;
            currentScore = 0;

            UpdateLevel();

            if (OnScoreTransferred != null) OnScoreTransferred();
        }

        private void UpdateLevel()
        {
            if (levelScore >= maxLevelScore)
            {
                levelScore -= maxLevelScore;
                level++;
                UpdateMaxLevelScore();

                if (OnLevelUpdated != null) OnLevelUpdated();
            }
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt(LEVEL, level);
            PlayerPrefs.SetInt(SCORE, levelScore);
        }

        private void LoadData()
        {
            level = PlayerPrefs.GetInt(LEVEL);
            levelScore = PlayerPrefs.GetInt(SCORE);

            if (level < 1) level = 1;

            UpdateMaxLevelScore();
        }

        private void UpdateMaxLevelScore()
        {
            maxLevelScore = level * 10; //TODO some more sophisticated function :P
        }
    }
}