using System;
using UnityEngine;

public abstract class UIToActionRequestConverter : MonoBehaviour
{
    public event Action<PlayerActionType> Request_Action;

    protected void RaiseActionRequest(PlayerActionType actionType)
    {
        Request_Action?.Invoke(actionType);
    }
}