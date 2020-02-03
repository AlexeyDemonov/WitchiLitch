using UnityEngine;

public class HitDetectionEventArgs
{
    public readonly GameObject HittedObject;
    public readonly HittedObjectType HittedObjectType;
    public readonly HitDirection HitDirection;
    public readonly Vector2 HitPoint;
}
