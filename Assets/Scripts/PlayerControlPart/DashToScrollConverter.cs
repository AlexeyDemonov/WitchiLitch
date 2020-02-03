using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashToScrollConverter : MonoBehaviour
{
    public float DashScrollSpeedIncrease;

    public event Action<float> Request_IncreaseScrollSpeed;
    public event Action<float> Request_DecreaseScrollSpeed;

    public void Handle_PlayerAction(PlayerActionType playerActionType)
    {
        switch (playerActionType)
        {
            case PlayerActionType.DashForward:
                Request_IncreaseScrollSpeed?.Invoke(DashScrollSpeedIncrease);
                break;
            case PlayerActionType.DashForwardEnd:
                Request_DecreaseScrollSpeed?.Invoke(DashScrollSpeedIncrease);
                break;
            default: /*Do nothing*/
                break;
        }
    }
}
