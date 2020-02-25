using System;
using UnityEngine;

public class PlayerDirectionDefiner : MonoBehaviour
{
    public Rigidbody2D PlayerRigidbody;
    public float DeadZone;

    PlayerDirection _currentDirection;

    PlayerDirection CurrentDirection
    {
        get => _currentDirection;

        set
        {
            _currentDirection = value;
            DirectionChanged?.Invoke(_currentDirection);
        }
    }

    public event Action<PlayerDirection> DirectionChanged;

    public PlayerDirection GetCurrentDirection()
    {
        return CurrentDirection;
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        _currentDirection = PlayerDirection.UNDEFINED;
    }

    // Update is called once per frame
    void Update()
    {
        var direction = DefinePlayerDirection(PlayerRigidbody);

        if (direction != CurrentDirection)
        {
            CurrentDirection = direction;
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