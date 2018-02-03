using UnityEngine;

namespace Dev.Krk.MemoryFlow.Game.State
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField]
        private float delay;

        [SerializeField]
        Animator[] animators;

        private float startTime;

        private bool activated;

        private bool shown;

        void Start()
        {
        }

        void Update()
        {
            if (activated && !shown && Time.time - startTime > delay)
            {
                Show();
            }
        }

        public void Activate()
        {
            if(!activated)
            {
                startTime = Time.time;
                activated = true;
            }
        }

        public void Deactivate()
        {
            if(activated)
            {
                activated = false;
                Hide();
            }
        }

        public void Show()
        {
            if (!shown)
            {
                shown = true;
                foreach (var animator in animators)
                    animator.SetBool("shown", shown);
            }
        }

        public void Hide()
        {
            startTime = Time.time;

            if (shown)
            {
                shown = false;
                foreach (var animator in animators)
                    animator.SetBool("shown", shown);
            }
        }
    }
}
