using UnityEngine;

namespace Dev.Krk.MemoryFlow.Game.State
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField]
        PopUpCanvas tutorialCanvas;

        [SerializeField]
        private float delay;

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
                shown = true;
                tutorialCanvas.Show();
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

        public void Hide()
        {
            startTime = Time.time;

            if (shown)
            {
                shown = false;
                tutorialCanvas.Hide();
            }
        }
    }
}
