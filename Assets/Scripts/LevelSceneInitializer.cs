﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSceneInitializer : MonoBehaviour
{
    public BaseScroller[] Scrollers;
    public ScrollSpeedDefiner ScrollSpeedDefiner;
    public DashToScrollConverter DashToScrollConverter;
    public PlayerController PlayerController;
    public PlayerAnimatorController PlayerAnimatorController;
    public PlayerHitDetectionSystem PlayerHitDetectionSystem;
    public PlayerDirectionDefiner PlayerDirectionDefiner;
    public VerticalFollower[] VerticalFollowers;


    private void Awake()
    {
        foreach (var scroller in Scrollers)
        {
            scroller.Request_ScrollSpeed += ScrollSpeedDefiner.GetCurrentScrollSpeed;
            ScrollSpeedDefiner.ScrollSpeedChanged += scroller.Handle_ScrollSpeedChanged;
        }

        DashToScrollConverter.Request_IncreaseScrollSpeed += ScrollSpeedDefiner.IncreaseSpeed;
        DashToScrollConverter.Request_DecreaseScrollSpeed += ScrollSpeedDefiner.DecreaseSpeed;

        PlayerController.PlayerAction += DashToScrollConverter.Handle_PlayerAction;
        PlayerController.PlayerAction += PlayerAnimatorController.Handle_PlayerAction;
        PlayerController.PlayerAction += PlayerHitDetectionSystem.Handle_PlayerAction;

        PlayerHitDetectionSystem.PlayerCrashed += PlayerController.Handle_PalyerCrashed;
        PlayerHitDetectionSystem.PlayerCrashed += PlayerAnimatorController.Handle_PlayerCrashed;
        //PlayerHitDetectionSystem.PlayerCrashed += ScrollSpeedDefiner.StopScroll;

        PlayerHitDetectionSystem.PlayerHittedObject += PlayerController.Handle_PlayerHittedObject;
        PlayerHitDetectionSystem.PlayerHittedObject += PlayerAnimatorController.Handle_PlayerHittedObject;

        foreach (var follower in VerticalFollowers)
        {
            follower.Request_PlayerDirection += PlayerDirectionDefiner.GetCurrentDirection;
            PlayerDirectionDefiner.DirectionChanged += follower.Handle_DirectionChanged;
        }

        PlayerDirectionDefiner.DirectionChanged += PlayerAnimatorController.Handle_PlayerDirectionChanged;
    }
}
