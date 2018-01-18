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
        private Image lifeRenderer;

        [SerializeField]
        private int lifeIndex;

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
            Color c = lifeRenderer.color;
            float a = livesController.Lives > lifeIndex ? 1f : 0f;
            lifeRenderer.color = new Color(c.r, c.g, c.b, a);
        }
    }
}
