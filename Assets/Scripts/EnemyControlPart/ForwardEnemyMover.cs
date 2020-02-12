using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardEnemyMover : EnemyMover
{
    public float MoveSpeed;
    public float RandomizeMoveSpeedBy;

    float _actualMoveSpeed;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        if(RandomizeMoveSpeedBy != 0f)
        {
            float randomSpeedAddition = UnityEngine.Random.Range(-RandomizeMoveSpeedBy, RandomizeMoveSpeedBy);
            _actualMoveSpeed = MoveSpeed + randomSpeedAddition;
        }
        else
            _actualMoveSpeed = MoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(base.MoveAllowed == true)
        {
            this.transform.localPosition += Vector3.left * _actualMoveSpeed * Time.deltaTime;
        }
    }
}
