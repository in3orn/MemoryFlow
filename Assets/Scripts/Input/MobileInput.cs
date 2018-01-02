using UnityEngine;

public class MobileInput : SwipeInput {

    protected override void init()
    {
        base.init();

        Input.multiTouchEnabled = false;
    }

    protected override bool isSupported()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    protected override bool isInputDown()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    protected override bool isInputUp()
    {
        return Input.touchCount <= 0 || Input.GetTouch(0).phase == TouchPhase.Ended;
    }
}
