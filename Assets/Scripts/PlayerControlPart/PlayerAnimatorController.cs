﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    public Animator Animator;

    AnimState _currentState;
    AnimState _queuedState;

    // Start is called before the first frame update
    void Start()
    {
        _currentState = AnimState.Run;
        _queuedState = AnimState.UNDEFINED;
    }

    public void Handle_PlayerAction(PlayerActionType actionType)
    {
        switch (actionType)
        {
            case PlayerActionType.Jump:             ConsiderApplyingNewState(AnimState.Jump);   break;
            case PlayerActionType.DashForward:      ConsiderApplyingNewState(AnimState.Dash);   break;
            case PlayerActionType.DashForwardEnd:   ConsiderApplyingNewState(AnimState.MidAir);     break;
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
            case HitDirection.Above:    ConsiderApplyingNewState(AnimState.HitGround);     break;
            case HitDirection.Below:    ConsiderApplyingNewState(AnimState.HitCeiling);    break;
            case HitDirection.Side: /*Do nothing*/  break;
        }
    }

    public void Handle_PlayerDirectionChanged(PlayerDirection newDirection)
    {
        switch (newDirection)
        {
            case PlayerDirection.Rising:    if (_currentState != AnimState.Jump) { ConsiderApplyingNewState(AnimState.Jump); }   break;
            case PlayerDirection.Falling:   ConsiderApplyingNewState(AnimState.Fall);   break;
            case PlayerDirection.Staying:   ConsiderApplyingNewState(AnimState.Run);    break;
        }
    }

    public void Handle_AnimationEnded()
    {
        if(_queuedState != AnimState.UNDEFINED)
        {
            ApplyNewState(_queuedState);
            _queuedState = AnimState.UNDEFINED;
        }
    }

    void QueueNextState(AnimState nextState)
    {
        _queuedState = nextState;
    }

    void ApplyNewState(AnimState newState)
    {
        _currentState = newState;
        Animator.SetTrigger(newState.ToString());
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
            case AnimState.MidAir:
                ApplyNewState(newState);
                break;
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
                QueueNextState(newState);
                break;
            case AnimState.Jump:
                ApplyNewState(newState);
                break;
            case AnimState.MidAir:
            case AnimState.Fall:
                QueueNextState(newState);
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
