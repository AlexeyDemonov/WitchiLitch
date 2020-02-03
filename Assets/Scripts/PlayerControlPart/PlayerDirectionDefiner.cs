using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionDefiner : MonoBehaviour
{
    public Rigidbody2D PlayerRigidbody;
    public float DeadZone;

    public event Action<PlayerDirection> DirectionChanged;

    public PlayerDirection CurrentDirection { get; private set; }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        CurrentDirection = PlayerDirection.UNDEFINED;
    }

    // Update is called once per frame
    void Update()
    {
        var direction = DefinePlayerDirection(PlayerRigidbody);

        if(direction != CurrentDirection)
        {
            CurrentDirection = direction;
            DirectionChanged?.Invoke(direction);
        }
    }

    PlayerDirection DefinePlayerDirection(Rigidbody2D playerRB)
    {
        float playerY = playerRB.velocity.y;

        if (Mathf.Abs(playerY) > DeadZone)
            return playerY > 0f ? PlayerDirection.Rising : PlayerDirection.Falling;
        else
            return PlayerDirection.Staying;
    }
}
