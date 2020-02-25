using System;
using UnityEngine;

public class HitDetectionEventArgs : EventArgs
{
    public GameObject HittedObject;
    public HittedObjectType HittedObjectType;
    public HitDirection HitDirection;
    public Vector2 HitPoint;
}