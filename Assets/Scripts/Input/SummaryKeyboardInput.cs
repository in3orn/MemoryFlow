using UnityEngine;
using Dev.Krk.MemoryFlow.Game.State;

namespace Dev.Krk.MemoryFlow.Inputs
{
    public class SummaryKeyboardInput : GameStateDependentInput
    {
        [Header("Dependencies")]
        [SerializeField]
        private ThemeController themeController;

        void Awake()
        {
            Init();
        }

        protected void Init()
        {
            enabled = IsSupported();
        }

        protected bool IsSupported()
        {
            return Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer;
        }

        protected override void UpdateInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                themeController.PrevTheme();
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                themeController.NextTheme();
                return;
            }
        }
    }
}