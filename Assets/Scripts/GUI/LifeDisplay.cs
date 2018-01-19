using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Game.GUI
{
    public class LifeDisplay : MonoBehaviour
    {
        [SerializeField]
        private LivesController livesController;

        [SerializeField]
        private int lifeIndex;

        private Animator animator;

        private bool shown;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            UpdateLife();
        }
        
        void OnEnable()
        {
            livesController.OnLivesUpdated += UpdateLife;
        }

        void OnDisable()
        {
            if(livesController != null)
            {
                livesController.OnLivesUpdated -= UpdateLife;
            }
        }

        private void UpdateLife()
        {
            if (!shown && livesController.Lives > lifeIndex)
            {
                animator.SetTrigger("Show");
                shown = true;
            }
            else if(shown && livesController.Lives <= lifeIndex)
            {
                animator.SetTrigger("Hide");
                shown = false;
            }
        }
    }
}
