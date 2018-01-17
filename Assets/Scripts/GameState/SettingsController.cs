using UnityEngine;

namespace Dev.Krk.MemoryFlow.Summary
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField]
        private PopUpCanvas settings;

        void Start()
        {
        }

        public void Show()
        {
            settings.Show();
        }

        public void Hide()
        {
            settings.Hide();
        }
    }
}