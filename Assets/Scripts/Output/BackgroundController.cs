using Dev.Krk.MemoryFlow.Game;
using UnityEngine;

namespace Dev.Krk.MemoryFlow.Output
{
    [RequireComponent(typeof(Animator))]
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField]
        private LevelController levelController;

        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {

        }

        void OnEnable()
        {
            levelController.OnPlayerFailed += ProcessPlayerFailed;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnPlayerFailed -= ProcessPlayerFailed;
            }
        }

        private void ProcessPlayerFailed()
        {
            animator.SetTrigger("PlayerFailed");
        }
    }
}
