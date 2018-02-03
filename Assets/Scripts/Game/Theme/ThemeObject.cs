using UnityEngine;
using Dev.Krk.MemoryFlow.Game.State;
using Dev.Krk.MemoryFlow.Data;

namespace Dev.Krk.MemoryFlow.Game.Theme
{
    public abstract class ThemeObject : MonoBehaviour
    {
        [SerializeField]
        private ThemeData.ColorEnum colorType;

        [SerializeField]
        private ThemeController controller;

        public ThemeController Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        void Start()
        {
            UpdateColor();
        }

        void OnEnable()
        {
            if (controller != null)
            {
                controller.OnThemeUpdated += ProcessThemeUpdated;
                controller.OnInitialized += UpdateColor;
            }
        }

        void OnDisable()
        {
            if (controller != null)
            {
                controller.OnThemeUpdated -= ProcessThemeUpdated;
                controller.OnInitialized -= UpdateColor;
            }
        }

        private void ProcessThemeUpdated()
        {
            UpdateColor();
        }

        protected abstract void UpdateColor();

        protected Color GetColor()
        {
            return controller.GetCurrentTheme().GetColor(colorType);
        }
    }
}
