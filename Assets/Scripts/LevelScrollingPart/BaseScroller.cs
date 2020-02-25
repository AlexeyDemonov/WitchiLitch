using System;
using UnityEngine;

public abstract class BaseScroller : MonoBehaviour
{
    public float CurrentScrollSpeed;
    public float IncomingScrollSpeedMultiplier;

    public event Func<float> Request_ScrollSpeed;

    protected void RequestScrollSpeed()
    {
        if (Request_ScrollSpeed != null)
            CurrentScrollSpeed = Request_ScrollSpeed.Invoke() * IncomingScrollSpeedMultiplier;
    }

    public void Handle_ScrollSpeedChanged(float newSpeed)
    {
        CurrentScrollSpeed = newSpeed * IncomingScrollSpeedMultiplier;
    }
}