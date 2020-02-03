using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSpeedDefiner : MonoBehaviour
{
    //====================================================================
    //Fields
    public float StartScrollSpeed;
    public float BaseIncreaseSpeedBy;

    private float _currentScrollSpeed;

    //====================================================================
    //Events
    public event Action<float> ScrollSpeedChanged;

    //====================================================================
    //Properties
    public float CurrentScrollSpeed
    {
        get => _currentScrollSpeed;

        private set
        {
            if(value != _currentScrollSpeed)
            {
                _currentScrollSpeed = value;
                ScrollSpeedChanged?.Invoke(_currentScrollSpeed);
            }
        }
    }

    //====================================================================
    //Methods
    private void Awake()
    {
        _currentScrollSpeed = StartScrollSpeed;
    }

    public void IncreaseSpeed()
    {
        CurrentScrollSpeed += BaseIncreaseSpeedBy;
    }

    public void IncreaseSpeed(float increaseBy)
    {
        CurrentScrollSpeed += increaseBy;
    }

    public void DecreaseSpeed(float decreaseBy)
    {
        CurrentScrollSpeed -= decreaseBy;
    }

    public void StopScroll()
    {
        CurrentScrollSpeed = 0f;
    }
}
