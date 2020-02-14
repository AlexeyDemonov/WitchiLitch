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
    public FallChecker FallChecker;
    public PauseMenuUIController PauseMenuUIController;
    public PlayerDirectionDefiner PlayerDirectionDefiner;
    public VerticalFollower[] VerticalFollowers;
    public EnemyHitRegistrationSystem EnemyHitRegistrationSystem;
    public UIToActionRequestConverter UIToActionRequestConverter;
    public SoundEffectsBox SoundEffectsBox;

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

        PlayerHitDetectionSystem.PlayerHittedObject += PlayerController.Handle_PlayerHittedObject;
        PlayerHitDetectionSystem.PlayerHittedObject += PlayerAnimatorController.Handle_PlayerHittedObject;
        PlayerHitDetectionSystem.PlayerHittedObject += EnemyHitRegistrationSystem.Handle_EnemyHit;

        PlayerHitDetectionSystem.PlayerCrashed += PlayerController.Handle_PalyerCrashed;
        PlayerHitDetectionSystem.PlayerCrashed += PlayerAnimatorController.Handle_PlayerCrashed;
        PlayerHitDetectionSystem.PlayerCrashed += ScrollSpeedDefiner.StopScroll;
        PlayerHitDetectionSystem.PlayerCrashed += PauseMenuUIController.Handle_PlayerCrashed;

        FallChecker.Fallen += PlayerController.Handle_PalyerCrashed;
        FallChecker.Fallen += PlayerAnimatorController.Handle_PlayerCrashed;
        FallChecker.Fallen += ScrollSpeedDefiner.StopScroll;
        FallChecker.Fallen += PauseMenuUIController.Handle_PlayerCrashed;

        foreach (var follower in VerticalFollowers)
        {
            follower.Request_PlayerDirection += PlayerDirectionDefiner.GetCurrentDirection;
            PlayerDirectionDefiner.DirectionChanged += follower.Handle_DirectionChanged;
        }

        PlayerDirectionDefiner.DirectionChanged += PlayerAnimatorController.Handle_PlayerDirectionChanged;

        UIToActionRequestConverter.Request_Action += PlayerController.Handle_ActionRequest;

        //Play audio
        MusicBox.GetInstance()?.Play(1);

        var settings = SettingsController.GetInstance().Handle_LoadSettingsRequest();
        SoundEffectsBox.AcceptSettings(settings);

        PlayerController.PlayerAction += SoundEffectsBox.Handle_PlayerAction;
        PlayerHitDetectionSystem.PlayerHittedObject += SoundEffectsBox.Handle_PlayerHittedObject;
        PlayerHitDetectionSystem.PlayerCrashed += SoundEffectsBox.Handle_PlayerCrashed;
        FallChecker.Fallen += SoundEffectsBox.Handle_PlayerFallen;
    }
}
