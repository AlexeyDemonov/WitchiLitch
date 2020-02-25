using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public abstract void Handle_Hit(HitDirection hitDirection);
}