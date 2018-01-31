using System;
using UnityEngine;

namespace Dev.Krk.MemoryFlow.Data
{
    [Serializable]
    public class ThemeData
    {
        public enum ColorEnum
        {
            Main = 0,
            Second,
            BkgMain,
            BkgSecond
        }

        public int Id;

        [SerializeField]
        private Color[] colors;

        //TODO music & sounds :)

        public bool Locked;

        public int UnlockLevel;

        //TODO other unlocks


        public Color GetColor(ColorEnum color)
        {
            return colors[(int)color];
        }
    }
}