﻿using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class RegularEnemyController : EnemyController
{
    Collider2D _collider;
    Animator _animator;
    bool _colliderDisabled;

    public static event Action EnemyDown;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    void DisableCollider()
    {
        if/*yet*/(!_colliderDisabled)
        {
            _colliderDisabled = true;
            _collider.enabled = false;
        }
    }

    void StopMoving()
    {
        //I'm not very proud of this approach but it solves more problems than it causes
        //this.gameObject.GetComponentInParent<EnemyMover>()?.Stop();
        //or use this? Escobar axiom indeed - I don't like them both
        SendMessageUpwards("Stop", null, SendMessageOptions.DontRequireReceiver);
    }

    public override void Handle_Hit(HitDirection hitDirection)
    {
        DisableCollider();
        StopMoving();
        PlayAnimation(hitDirection);
        EnemyDown?.Invoke();
    }

    private void PlayAnimation(HitDirection hitDirection)
    {
        switch (hitDirection)
        {
            case HitDirection.Above:
                _animator.SetTrigger("DieToDown");
                break;

            case HitDirection.Below:
                _animator.SetTrigger("DieToUp");
                break;

            case HitDirection.RightSide:
            case HitDirection.LeftSide:
                _animator.SetTrigger("DieToSide");
                break;
        }
    }

    // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DisableCollider();
    }
}