using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIToActionRequestConverter : MonoBehaviour
{
    public event Action<PlayerActionType> Request_Action;
    
    protected void RaiseActionRequest(PlayerActionType actionType)
    {
        Request_Action?.Invoke(actionType);
    }
}
