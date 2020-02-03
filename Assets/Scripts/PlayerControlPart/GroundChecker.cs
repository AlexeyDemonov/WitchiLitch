using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public float GroundDistance;
    public int LayerMask;

    public bool IsGrounded
    {
        get
        {
            return GroundCheckHit().collider != null;
        }
    }

    public RaycastHit2D GroundCheckHit()
    {
        return Physics2D.Raycast(this.transform.position, -this.transform.up, GroundDistance, LayerMask);
    }
}
