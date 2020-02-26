using UnityEngine;

public class SwipeOnlyUIController : UIToActionRequestConverter
{
    public float MinSwipeDistance;

    Vector2 _swipeStartPosition;
    bool _gestureInProgress;

    //Nested
    enum Gesture
    {
        UNDEFINED, SwipeUp, SwipeDown
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
                    _gestureInProgress = true;
                    break;

                case TouchPhase.Moved:
                    if (_gestureInProgress)
                    {
                        var currentTouchPosition = touch.position;
                        var gesture = DefineGesture(_swipeStartPosition, currentTouchPosition);

                        if (gesture != Gesture.UNDEFINED)
                        {
                            _gestureInProgress = false;

                            switch (gesture)
                            {
                                case Gesture.SwipeUp:
                                    base.RaiseActionRequest(PlayerActionType.Jump);
                                    break;
                                case Gesture.SwipeDown:
                                    base.RaiseActionRequest(PlayerActionType.DashDown);
                                    break;
                            }
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    if (_gestureInProgress)
                    {
                        _gestureInProgress = false;
                        base.RaiseActionRequest(PlayerActionType.DashForward);
                    }
                    break;
            }
        }
    }

    Gesture DefineGesture(Vector2 startPos, Vector2 endPos)
    {
        bool swipe = Mathf.Abs(startPos.y - endPos.y) > MinSwipeDistance;

        if (swipe)
            return startPos.y > endPos.y ? Gesture.SwipeDown : Gesture.SwipeUp;
        else
            return Gesture.UNDEFINED;
    }
}