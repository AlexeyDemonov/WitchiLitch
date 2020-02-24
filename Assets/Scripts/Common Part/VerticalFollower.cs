using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalFollower : MonoBehaviour
{
    public Transform Player;
    public float RiseOffset;
    public float FallOffset;
    [Tooltip("Leave 0 to disable lerping")]
    [Range(0f,100f)]
    public float LerpingSpeed;

    float _ourStartY;
    bool _lerping;
    bool _offset;

    const float DEAD_ZONE = 0.2f;

    Vector3 _ourCurrentPosition;
    PlayerDirection _currentPlayerDirection;


    public event Func<PlayerDirection> Request_PlayerDirection;

    public void Handle_DirectionChanged(PlayerDirection newDireciton)
    {
        _currentPlayerDirection = newDireciton;
    }


    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        _ourCurrentPosition = this.transform.position;
        _ourStartY = _ourCurrentPosition.y;
        _lerping = LerpingSpeed > 0f;
        _offset = RiseOffset != 0f || FallOffset != 0;

        if(Request_PlayerDirection != null)
            _currentPlayerDirection = Request_PlayerDirection.Invoke();
        else
            _currentPlayerDirection = PlayerDirection.UNDEFINED;
    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        float targetY = Player.position.y;
        _ourCurrentPosition = this.transform.position;
        float ourCurrentY = _ourCurrentPosition.y;

        float yToMoveTo = CalculateYPosToMoveTo(targetY);

        if(yToMoveTo < _ourStartY)
            yToMoveTo = _ourStartY;

        if(Mathf.Abs(yToMoveTo - ourCurrentY) > DEAD_ZONE)
        {
            MoveToDefindedHight(yToMoveTo);
        }
    }

    float CalculateYPosToMoveTo(float targetY)
    {
        if(!_offset || _currentPlayerDirection == PlayerDirection.Staying)
        {
            return targetY;
        }
        else
        {
            switch (_currentPlayerDirection)
            {
                case PlayerDirection.Rising:   return targetY + RiseOffset;
                case PlayerDirection.Falling:  return targetY - FallOffset;
                default: /*case PlayerDirection.UNDEFINED:*/ return targetY;
            }
        }
    }

    void MoveToDefindedHight(float targetY)
    {
        Vector3 newPosition = new Vector3(_ourCurrentPosition.x, targetY, _ourCurrentPosition.z);

        if(_lerping)
            this.transform.position = Vector3.Lerp(_ourCurrentPosition, newPosition, LerpingSpeed * Time.deltaTime);
        else
            this.transform.position = newPosition;
    }
}
