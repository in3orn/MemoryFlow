using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.Krk.MemoryFlow.Game.State
{
    public class LivesController : MonoBehaviour
    {
        public UnityAction OnLivesUpdated;

        [SerializeField]
        private int startLives;

        [SerializeField]
        private float resetInterval;

        private int lives;

        public int Lives { get { return lives; } }

        public void DecreaseLives()
        {
            lives--;
            if(OnLivesUpdated != null) OnLivesUpdated();
        }

        public void ResetLives()
        {
            StartCoroutine(ResetLivesInternal());
        }

        private IEnumerator ResetLivesInternal()
        {
            while (lives < startLives)
            {
                lives++;
                if (OnLivesUpdated != null) OnLivesUpdated();

                yield return new WaitForSeconds(resetInterval);
            }
        }
    }
}