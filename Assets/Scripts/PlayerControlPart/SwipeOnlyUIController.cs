using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeOnlyUIController : UIToActionRequestConverter
{
    public float MinSwipeDistance;

    Vector2 _swipeStartPosition;

    //Nested
    enum Gesture
    {
        Tap, SwipeUp, SwipeDown
    }

    // Start is called before the first frame update
    void Start()
    {
        _swipeStartPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _swipeStartPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    var swipeEndPosition = touch.position;
                    HandleTouch(_swipeStartPosition, swipeEndPosition);
                    break;
            }
        }
    }

    void HandleTouch(Vector2 startPos, Vector2 endPos)
    {
        var gesture = DefineGesture(startPos, endPos);
        PlayerActionType action = default;

        switch (gesture)
        {
            case Gesture.Tap:
                action = PlayerActionType.DashForward;
                break;
            case Gesture.SwipeUp:
                action = PlayerActionType.Jump;
                break;
            case Gesture.SwipeDown:
                action = PlayerActionType.DashDown;
                break;
        }

        base.RaiseActionRequest(action);
    }

    Gesture DefineGesture(Vector2 startPos, Vector2 endPos)
    {
        bool swipe = Mathf.Abs(startPos.y - endPos.y) > MinSwipeDistance;

        if(swipe)
            return startPos.y > endPos.y ? Gesture.SwipeDown : Gesture.SwipeUp;
        else
            return Gesture.Tap;
    }
}
