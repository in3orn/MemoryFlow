﻿using UnityEngine;

namespace Dev.Krk.MemoryFlow.Summary
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField]
        private PopUpCanvas shop;

        void Start()
        {
        }

        public void Show()
        {
            shop.Show();
        }

        public void Hide()
        {
            shop.Hide();
        }
    }
}