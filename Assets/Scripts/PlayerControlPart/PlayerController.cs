using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public Button JumpButton;
    public Button DashButton;
    public Button FallButton;
    public float JumpForce;
    public float FallForce;
    public float DashDuration;
    public GroundChecker[] GroundCheckers;

    public event Action<PlayerActionType> PlayerAction;

    Rigidbody2D _rigidbody;
    WaitForSeconds _dashDuration;
    Coroutine _removeDashCoroutine;
    bool _airDashAllowed;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _dashDuration = new WaitForSeconds(DashDuration);

        JumpButton.onClick.AddListener(Jump);
        DashButton.onClick.AddListener(Dash);
        FallButton.onClick.AddListener(Fall);
    }

    public void Handle_PalyerCrashed()
    {
        if(isPlayerDashing)
        {
            StopCoroutine(_removeDashCoroutine);
            _removeDashCoroutine = null;
            _rigidbody.gravityScale = 1f;
        }

        JumpButton.onClick.RemoveListener(Jump);
        DashButton.onClick.RemoveListener(Dash);
        FallButton.onClick.RemoveListener(Fall);
    }

    public void Handle_PlayerHittedObject(HitDetectionEventArgs args)
    {
        if(args.HitDirection == HitDirection.Above)
        {
            AllowAirDash();
        }
    }

    bool isPlayerGrounded
    {
        get
        {
            foreach (var checker in GroundCheckers)
            {
                if(checker.IsGrounded)
                    return true;
            }

            /*else*/
            return false;
        }
    }

    bool isPlayerDashing
    {
        get => _removeDashCoroutine != null;
    }

    bool isDashAllowed
    {
        get
        {
            if(isPlayerGrounded)
            {
                return true;
            }
            else if(_airDashAllowed)
            {
                _airDashAllowed = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void AllowAirDash()
    {
        if(!_airDashAllowed)
            _airDashAllowed = true;
    }

    void Jump()
    {
        if(!isPlayerDashing && isPlayerGrounded)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            AllowAirDash();
            PlayerAction?.Invoke(PlayerActionType.Jump);
        }
    }

    void Dash()
    {
        if(!isPlayerDashing && isDashAllowed)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.gravityScale = 0f;
            _removeDashCoroutine = StartCoroutine(RemoveDashAfterDelay());
            PlayerAction?.Invoke(PlayerActionType.DashForward);
        }
    }

    IEnumerator RemoveDashAfterDelay()
    {
        yield return _dashDuration;

        _rigidbody.gravityScale = 1f;
        _removeDashCoroutine = null;
        PlayerAction?.Invoke(PlayerActionType.DashForwardEnd);
    }

    void Fall()
    {
        if (!isPlayerDashing && !isPlayerGrounded)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(new Vector2(0f, -FallForce), ForceMode2D.Impulse);
            PlayerAction?.Invoke(PlayerActionType.DashDown);
        }
    }
}
