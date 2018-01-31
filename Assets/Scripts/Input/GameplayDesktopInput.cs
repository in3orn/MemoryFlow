﻿using UnityEngine;

namespace Dev.Krk.MemoryFlow.Inputs
{
    public class GameplayDesktopInput : GameplaySwipeInput
    {

        protected override bool IsSupported()
        {
            return Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer;
        }

        protected override bool IsInputDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        protected override bool IsInputUp()
        {
            return Input.GetMouseButtonUp(0);
        }
    }
}