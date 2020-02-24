using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownEnemyMover : EnemyMover
{
    public float MoveSpeed;
    public float MoveUpBy;
    public float MoveDownBy;
    public bool DrawGizmos;

    Vector3 _currentPosition;
    float _startY;
    float _upBorder;
    float _downBorder;
    bool _movingUp;

    // Start is called before the first frame update
    void Start()
    {
        _currentPosition = this.transform.localPosition;
        _startY = _currentPosition.y;
        _upBorder = _startY + MoveUpBy;
        _downBorder = _startY - MoveDownBy;

        RandomizeStartingPosition();
    }

    void RandomizeStartingPosition()
    {
        int up = 1;
        /*int middle = 0;*/
        int down = -1;

        int randomResult = UnityEngine.Random.Range(-1, 2);

        if(randomResult == up)
        {
            Vector3 startingPosition = new Vector3(_currentPosition.x, _upBorder, _currentPosition.z);
            this.transform.localPosition = startingPosition;
            _movingUp = false;
        }
        else if (randomResult == down)
        {
            Vector3 startingPosition = new Vector3(_currentPosition.x, _downBorder, _currentPosition.z);
            this.transform.localPosition = startingPosition;
            _movingUp = false;
        }
        else/*if(randomResult == middle)*/
        {
            _movingUp = (UnityEngine.Random.Range(0, 2) == 0) ? true : false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(base.MoveAllowed)
        {
            if(_movingUp)
                this.transform.localPosition += Vector3.up * MoveSpeed * Time.deltaTime;
            else
                this.transform.localPosition += Vector3.down * MoveSpeed * Time.deltaTime;

            _currentPosition = this.transform.localPosition;

            if(IsPassedBound)
                ChangeDirection();
        }
    }

    bool IsPassedBound
    {
        get
        {
            if(_movingUp)
                return _currentPosition.y > _upBorder;
            else
                return _currentPosition.y < _downBorder;
        }
    }

    void ChangeDirection()
    {
        _movingUp = !_movingUp;
    }

    private void OnDrawGizmos()
    {
        if(DrawGizmos)
        {
            Vector3 currentPosition = this.transform.position;

            Gizmos.DrawCube(new Vector3(currentPosition.x, _upBorder, currentPosition.z), new Vector3(2f, 0.1f, 0.1f));
            Gizmos.DrawCube(new Vector3(currentPosition.x, _downBorder, currentPosition.z), new Vector3(2f, 0.1f, 0.1f));
        }
    }
}