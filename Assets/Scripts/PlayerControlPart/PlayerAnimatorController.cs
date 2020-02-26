using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    public Animator Animator;
    public GroundChecker GroundChecker;

    AnimState _currentState;
    PlayerDirection _currentDireciton;

    string[] _stateNames;
    int _stateNamesLength;

    // Start is called before the first frame update
    void Start()
    {
        _currentState = AnimState.Run;

        //Reason described in ApplyNewState method
        //(-)Expensive (-)Configuration in runtime (+)Changes of AnimState enum won't affect this script
        //var list = new List<string>(Enum.GetNames(typeof(AnimState)));
        //list.Remove("UNDEFINED");
        //_stateNames = list.ToArray();
        //_stateNamesLength = _stateNames.Length;

        //(+)Cheaper (+)Configuration in compile-time (-)Changes of AnimState enum will affect this script
        _stateNames = new string[] { "Run", "Jump", "MidAir", "Fall", "Dash", "DashDown", "HitGround", "HitCeiling", "Die" };
        _stateNamesLength = _stateNames.Length;
    }

    public AnimState Handle_CurrentPlayerStateRequest() => _currentState;

    public void Handle_PlayerAction(PlayerActionType actionType)
    {
        switch (actionType)
        {
            case PlayerActionType.Jump:             ConsiderApplyingNewState(AnimState.Jump);   break;
            case PlayerActionType.DashForward:      ConsiderApplyingNewState(AnimState.Dash);   break;
            case PlayerActionType.DashForwardEnd:   if (_currentState != AnimState.Die) { DefineCurrentState(); }   break;
            case PlayerActionType.DashDown:         ConsiderApplyingNewState(AnimState.DashDown);   break;
        }
    }

    public void Handle_PlayerCrashed()
    {
        ConsiderApplyingNewState(AnimState.Die);
    }

    public void Handle_PlayerHittedObject(HitDetectionEventArgs args)
    {
        switch (args.HitDirection)
        {
            case HitDirection.Above: ConsiderApplyingNewState(AnimState.HitGround); break;
            case HitDirection.Below: ConsiderApplyingNewState(AnimState.HitCeiling); break;
            default: /*Do nothing*/  break;
        }
    }

    public void Handle_PlayerDirectionChanged(PlayerDirection newDirection)
    {
        switch (newDirection)
        {
            case PlayerDirection.Rising: if (_currentState != AnimState.Jump) { ConsiderApplyingNewState(AnimState.Jump); } break;
            case PlayerDirection.Falling: ConsiderApplyingNewState(AnimState.Fall); break;
            case PlayerDirection.Staying: ConsiderApplyingNewState(AnimState.Run); break;
        }

        _currentDireciton = newDirection;
    }

    public void Handle_AnimationEnded()
    {
        //Bug: Sometimes this event is called even though animation already switched, this is a workaround
        switch (_currentState)
        {
            case AnimState.HitGround:
            case AnimState.HitCeiling:
                DefineCurrentState();
                break;

            case AnimState.Die:
                /*Ignore*/
                break;

            default:
                ApplyNewState(_currentState);
                break;
        }
    }

    void ApplyNewState(AnimState newState)
    {
        //Bug: In rare cases two animation triggers may be set at the same time resulting in unwanted animation switch behaviour, this is a workaround
        for (int i = 0; i < _stateNamesLength; i++)
        {
            Animator.ResetTrigger(_stateNames[i]);
        }

        Animator.SetTrigger(newState.ToString());
        _currentState = newState;
    }

    void DefineCurrentState()
    {
        switch (_currentDireciton)
        {
            case PlayerDirection.Rising:
                ApplyNewState(AnimState.Jump);
                break;

            case PlayerDirection.Falling:
                ApplyNewState(AnimState.Fall);
                break;

            default:
                {
                    if (GroundChecker.IsGrounded)
                        ApplyNewState(AnimState.Run);
                    else
                        ApplyNewState(AnimState.MidAir);
                }
                break;
        }
    }

    void ConsiderApplyingNewState(AnimState newState)
    {
        switch (_currentState)
        {
            case AnimState.UNDEFINED:
                Debug.LogError("PlayerAnimatorController.ConsiderApplyingNewState: _currentState is UNDEFINED");
                break;

            case AnimState.Run:         RunStateRules(newState);        break;
            case AnimState.Jump:        JumpStateRules(newState);       break;
            case AnimState.MidAir:      MidAirStateRules(newState);     break;
            case AnimState.Fall:        FallStateRules(newState);       break;
            case AnimState.Dash:        DashStateRules(newState);       break;
            case AnimState.DashDown:    DashDownStateRules(newState);   break;
            case AnimState.HitGround:   HitGroundStateRules(newState);  break;
            case AnimState.HitCeiling:  HitCeilingStateRules(newState); break;
            case AnimState.Die:         DieStateRules(newState);        break;
            default:
                Debug.LogError("PlayerAnimatorController.ConsiderApplyingNewState: _currentState is unknown");
                break;
        }
    }

    void RunStateRules(AnimState newState)
    {
        switch (newState)
        {
            case AnimState.Run:
                /*Ignore*/
                break;

            case AnimState.Jump:
            case AnimState.MidAir:
            case AnimState.Fall:
            case AnimState.Dash:
            case AnimState.DashDown:
                ApplyNewState(newState);
                break;

            case AnimState.HitGround:
            case AnimState.HitCeiling:
                /*Ignore*/
                break;

            case AnimState.Die:
                ApplyNewState(newState);
                break;
        }
    }

    void JumpStateRules(AnimState newState)
    {
        switch (newState)
        {
            case AnimState.Run:
                ApplyNewState(AnimState.MidAir);
                break;

            case AnimState.Jump:
            case AnimState.MidAir:
            case AnimState.Fall:
            case AnimState.Dash:
            case AnimState.DashDown:
            case AnimState.HitGround:
            case AnimState.HitCeiling:
            case AnimState.Die:
                ApplyNewState(newState);
                break;
        }
    }

    void MidAirStateRules(AnimState newState)
    {
        switch (newState)
        {
            case AnimState.Run:
            case AnimState.Jump:
                ApplyNewState(newState);
                break;

            case AnimState.MidAir:
                /*Ignore*/
                break;

            case AnimState.Fall:
            case AnimState.Dash:
            case AnimState.DashDown:
            case AnimState.HitGround:
            case AnimState.HitCeiling:
            case AnimState.Die:
                ApplyNewState(newState);
                break;
        }
    }

    void FallStateRules(AnimState newState)
    {
        switch (newState)
        {
            case AnimState.Run:
            case AnimState.Jump:
            case AnimState.MidAir:
                ApplyNewState(newState);
                break;

            case AnimState.Fall:
                /*Ignore*/
                break;

            case AnimState.Dash:
            case AnimState.DashDown:
            case AnimState.HitGround:
            case AnimState.HitCeiling:
            case AnimState.Die:
                ApplyNewState(newState);
                break;
        }
    }

    void DashStateRules(AnimState newState)
    {
        switch (newState)
        {
            case AnimState.Run:
                /*Ignore*/
                break;

            case AnimState.Jump:
                ApplyNewState(newState);
                break;

            case AnimState.MidAir:
            case AnimState.Fall:
                /*Ignore*/
                break;

            case AnimState.Dash:
            case AnimState.DashDown:
                ApplyNewState(newState);
                break;

            case AnimState.HitGround:
            case AnimState.HitCeiling:
                /*Ignore*/
                break;

            case AnimState.Die:
                ApplyNewState(newState);
                break;
        }
    }

    void DashDownStateRules(AnimState newState)
    {
        switch (newState)
        {
            case AnimState.Run:
                /*Ignore*/
                break;

            case AnimState.Jump:
                ApplyNewState(newState);
                break;

            case AnimState.MidAir:
            case AnimState.Fall:
                /*Ignore*/
                break;

            case AnimState.Dash:
                ApplyNewState(newState);
                break;

            case AnimState.DashDown:
                /*Ignore*/
                break;

            case AnimState.HitGround:
                ApplyNewState(newState);
                break;

            case AnimState.HitCeiling:
                /*Ignore*/
                break;

            case AnimState.Die:
                ApplyNewState(newState);
                break;
        }
    }

    void HitGroundStateRules(AnimState newState)
    {
        switch (newState)
        {
            case AnimState.Run:
                /*Ignore*/
                break;

            case AnimState.Jump:
                ApplyNewState(newState);
                break;

            case AnimState.MidAir:
            case AnimState.Fall:
                /*Ignore*/
                break;

            case AnimState.Dash:
            case AnimState.DashDown:
                ApplyNewState(newState);
                break;

            case AnimState.HitGround:
            case AnimState.HitCeiling:
                /*Ignore*/
                break;

            case AnimState.Die:
                ApplyNewState(newState);
                break;
        }
    }

    void HitCeilingStateRules(AnimState newState)
    {
        HitGroundStateRules(newState);
    }

    void DieStateRules(AnimState newState)
    {
        /*Ignore all*/
    }
}