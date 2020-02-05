using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitRegistrationSystem : MonoBehaviour
{
    public void Handle_EnemyHit(HitDetectionEventArgs args)
    {
        if(args.HittedObjectType == HittedObjectType.Enemy)
        {
            var enemyController = args.HittedObject.GetComponent<EnemyController>();
            enemyController.Handle_Hit(args.HitDirection);
        }
    }
}
