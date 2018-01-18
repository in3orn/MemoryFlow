using UnityEngine;
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

        void OnEnable()
        {
            levelController.OnStarted += ProcessLevelStarted;
            levelController.OnLevelCompleted += ProcessLevelCompleted;

            livesController.OnLivesUpdated += ProcessLivesUpdated;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnStarted -= ProcessLevelStarted;
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
                int index = Mathf.FloorToInt(Random.value * ((float)completedTexts.Length - 0.01f));
                text.text = completedTexts[index];
                animator.SetTrigger("OnShow");
            }
        }

        private void ProcessLivesUpdated()
        {
            perfect = false;
        }
    }
}
