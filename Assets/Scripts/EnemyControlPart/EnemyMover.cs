using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMover : MonoBehaviour
{
    protected bool MoveAllowed = true;

    public void Stop()
    {
        MoveAllowed = false;
    }
}
