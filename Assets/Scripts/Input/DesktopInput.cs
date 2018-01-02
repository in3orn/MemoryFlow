using System;
using UnityEngine;

public class DesktopInput : SwipeInput {

    protected override bool isSupported()
    {
        return Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer;
    }

    protected override bool isInputDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    protected override bool isInputUp()
    {
        return Input.GetMouseButtonUp(0);
    }
}
