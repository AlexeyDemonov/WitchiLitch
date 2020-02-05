using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    Collider2D _collider;
    Animator _animator;
    bool _colliderDisabled;

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

    public void Handle_Hit(HitDirection hitDirection)
    {
        DisableCollider();

        switch (hitDirection)
        {
            case HitDirection.Above:
                _animator.SetTrigger("DieToDown");
                break;
            case HitDirection.Below:
                _animator.SetTrigger("DieToUp");
                break;
            case HitDirection.RightSide:
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
