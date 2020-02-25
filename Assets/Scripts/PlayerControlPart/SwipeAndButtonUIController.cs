using UnityEngine;
using UnityEngine.UI;

public class SwipeAndButtonUIController : UIToActionRequestConverter
{
    public Button DashButton;
    public RectTransform TouchPanel;
    public float MinSwipeDistance;

    Vector2 _startPosition;
    Vector2 _endPosition;

    // Start is called before the first frame update
    void Start()
    {
        DashButton.onClick.AddListener(SendDash);

        _startPosition = Vector2.zero;
        _endPosition = Vector2.zero;
    }

    void SendDash()
    {
        base.RaiseActionRequest(PlayerActionType.DashForward);
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
                    _startPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    _endPosition = touch.position;
                    HandleTouch(_startPosition, _endPosition);
                    break;
            }
        }
    }

    void HandleTouch(Vector2 startPos, Vector2 endPos)
    {
        bool startInsidePanel = IsInsideTouchPanel(startPos);
        bool endInsidePanel = IsInsideTouchPanel(endPos);

        if (startInsidePanel && endInsidePanel)
        {
            bool swipeDown = IsUserSwipedDown(startPos, endPos);

            if (swipeDown)
                base.RaiseActionRequest(PlayerActionType.DashDown);
            else
                base.RaiseActionRequest(PlayerActionType.Jump);
        }
    }

    bool IsInsideTouchPanel(Vector2 pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(TouchPanel, pos);
    }

    bool IsUserSwipedDown(Vector2 startPos, Vector2 endPos)
    {
        bool down = startPos.y > endPos.y;
        bool swipe = Mathf.Abs(startPos.y - endPos.y) > MinSwipeDistance;

        return down && swipe;
    }
}