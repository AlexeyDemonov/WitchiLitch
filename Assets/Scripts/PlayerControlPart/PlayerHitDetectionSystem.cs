using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerHitDetectionSystem : MonoBehaviour
{
    public float PlatformSideHitRange;
    public float EnemySideHitRange;
    public GroundChecker EnemyOnJumpHitChecker;

    public bool DrawPlatformSideHitGizmo;
    public bool DrawEnemySideHitGizmo;

    public event Action PlayerCrashed;
    public event Action<HitDetectionEventArgs> PlayerHittedObject;

    bool _dashing;
    Collider2D _ourCollider;

    // Start is called just before any of the Update methods is called the first time
    void Start()
    {
        _dashing = false;
        _ourCollider = GetComponent<Collider2D>();
    }

    public void Handle_PlayerAction(PlayerActionType actionType)
    {
        switch (actionType)
        {
            case PlayerActionType.Jump: CheckIfJumpHitEnemyUnderPlayer();   break;
            case PlayerActionType.DashForward:      _dashing = true;    break;
            case PlayerActionType.DashForwardEnd:   _dashing = false;   break;
            case PlayerActionType.DashDown: /*Do nothing*/  break;
            default:    /*Do nothing*/    break;
        }
    }

    // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
    void OnCollisionEnter2D(Collision2D collision)
    {
        var hittedObject = collision.gameObject;
        var objectType   = DefineHittedObjectType(collision.transform);
        var hitPoint     = collision.GetContact(0).point;
        var hitDirection = DefineDirection(hitPoint, objectType);
        
        var eventArgs = new HitDetectionEventArgs() { HittedObject = hittedObject, HittedObjectType = objectType, HitPoint = hitPoint, HitDirection = hitDirection };

        if(IsACrash(eventArgs))
            PlayerCrashed?.Invoke();
        else
            PlayerHittedObject?.Invoke(eventArgs);
    }

    HittedObjectType DefineHittedObjectType(Transform hittedObject)
    {
        if(hittedObject.CompareTag("Enemy"))
            return HittedObjectType.Enemy;
        else/*if (hittedObject.CompareTag("Platform"))*/
            return HittedObjectType.Platform;
    }

    HitDirection DefineDirection(Vector2 hitPoint, HittedObjectType objectType)
    {
        Vector2 ourPosition = this.transform.position;

        float offset = objectType == HittedObjectType.Enemy ? EnemySideHitRange : PlatformSideHitRange;

        if (Mathf.Abs(ourPosition.y - hitPoint.y) < offset)
            return hitPoint.x > ourPosition.x ? HitDirection.RightSide : HitDirection.LeftSide;
        else
            return ourPosition.y > hitPoint.y ? HitDirection.Above : HitDirection.Below;
    }

    bool IsACrash(HitDetectionEventArgs args)
    {
        if(args.HitDirection == HitDirection.RightSide)
        {
            if(args.HittedObjectType == HittedObjectType.Platform)
                return true;

            if (args.HittedObjectType == HittedObjectType.Enemy && _dashing == false)
                return true;
        }
        
        /*else*/
        return false;
    }

    void CheckIfJumpHitEnemyUnderPlayer()
    {
        var hit = EnemyOnJumpHitChecker.GroundCheckHit();
        
        if(hit.collider != null && DefineHittedObjectType(hit.transform) == HittedObjectType.Enemy)
        {
            var eventArgs = new HitDetectionEventArgs() { HittedObject = hit.collider.gameObject, HittedObjectType = HittedObjectType.Enemy, HitPoint = hit.point, HitDirection = HitDirection.Above };
            PlayerHittedObject?.Invoke(eventArgs);
        }
    }

    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if((DrawPlatformSideHitGizmo || DrawEnemySideHitGizmo) && Application.isPlaying)
        {
            Vector3 currentPosition = this.transform.position;

            if(DrawPlatformSideHitGizmo)
            {
                Gizmos.DrawCube(new Vector3(currentPosition.x, currentPosition.y + PlatformSideHitRange, currentPosition.z), new Vector3(2f, 0.1f, 0.1f));
                Gizmos.DrawCube(new Vector3(currentPosition.x, currentPosition.y - PlatformSideHitRange, currentPosition.z), new Vector3(2f, 0.1f, 0.1f));
            }

            if (DrawEnemySideHitGizmo)
            {
                Gizmos.DrawCube(new Vector3(currentPosition.x, currentPosition.y + EnemySideHitRange, currentPosition.z), new Vector3(2f, 0.1f, 0.1f));
                Gizmos.DrawCube(new Vector3(currentPosition.x, currentPosition.y - EnemySideHitRange, currentPosition.z), new Vector3(2f, 0.1f, 0.1f));
            }
        }
    }
}
