using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScroller : BaseScroller
{
    [Header("Starting part")]
    public SpriteRenderer StartingPart;
    [Header("Regular level")]
    public SpriteRenderer[] LevelParts;
    public int PartsCount;
    [Header("Boss part")]
    public SpriteRenderer BossLevel;
    public bool DrawGizmos;

    Vector3 _startingPosition;
    Vector3 _currentPosition;
    Queue<SpriteRenderer> _partsChain;
    SpriteRenderer _lastPart;
    float _bound;

    bool _advancedMode;
    Queue<int> _spawnedIndexes;
    bool[] _spawned;

    bool _bossMode;
    int _nonBossPartsLeft;

    public event Action BossLevelReady;

    public void Handle_BossLevelStartRequest()
    {
        if (_bossMode == false)
        {
            _bossMode = true;
            _nonBossPartsLeft = PartsCount;
        }
    }

    public void Handle_BossLevelEndRequest()
    {
        if(_bossMode == true)
            _bossMode = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.RequestScrollSpeed();

        _startingPosition = this.transform.position;
        _currentPosition = _startingPosition;
        _partsChain = new Queue<SpriteRenderer>();

        _advancedMode = LevelParts.Length > PartsCount;
        _bossMode = false;

        if(_advancedMode)
        {
            _spawnedIndexes = new Queue<int>();
            _spawned = new bool[LevelParts.Length];
        }

        PrebuildLevel();
    }

    void PrebuildLevel()
    {
        //Instantiate starting part
        var instanceOfStartingPart = Instantiate<SpriteRenderer>(StartingPart, _startingPosition, Quaternion.identity);

        //Add starting part
        _partsChain.Enqueue(instanceOfStartingPart);
        _lastPart = instanceOfStartingPart;

        //Add other parts
        for (int i = 1; i < PartsCount; i++)
            AddNextPartToChain(_partsChain, ref _lastPart);

        //Attach
        foreach (var part in _partsChain)
            part.transform.SetParent(this.transform);

        //Calculate bound
        _bound = _startingPosition.x - GetFullSize(StartingPart);
    }

    // Update is called once per frame
    void Update()
    {
        if(base.CurrentScrollSpeed != 0f)
        {
            this.transform.position += Vector3.left * base.CurrentScrollSpeed * Time.deltaTime;
            _currentPosition = this.transform.position;

            if (IsPassedBound)
                UpdateLevel();
        }
    }

    bool IsPassedBound
    {
        get => _currentPosition.x < _bound;
    }

    void UpdateLevel()
    {
        //Unattach children
        foreach (var part in _partsChain)
            part.transform.SetParent(null);

        //Remove last
        var partToDelete = _partsChain.Dequeue();
        Destroy(partToDelete.gameObject);

        if(_advancedMode && _spawnedIndexes.Count == PartsCount)
        {
            var index = _spawnedIndexes.Dequeue();
            _spawned[index] = false;
        }

        if(_bossMode && _nonBossPartsLeft > 0)
        {
            _nonBossPartsLeft--;

            if/*now*/(_nonBossPartsLeft == 0)
                BossLevelReady?.Invoke();
        }

        //Add new
        AddNextPartToChain(_partsChain, ref _lastPart);

        //Move back
        float currentX = _currentPosition.x;
        float delta = Mathf.Abs(currentX - _bound);
        Vector3 newPosition = new Vector3(_startingPosition.x - delta, _startingPosition.y, _startingPosition.z);
        this.transform.position = newPosition;

        //Calculate new bound
        _bound = _startingPosition.x - GetFullSize(_partsChain.Peek());

        //Reattach
        foreach (var part in _partsChain)
            part.transform.SetParent(this.transform);
    }

    float GetHalfSize(SpriteRenderer sprite) => sprite.bounds.extents.x;
    float GetFullSize(SpriteRenderer sprite) => sprite.bounds.extents.x * 2f;

    void AddNextPartToChain(Queue<SpriteRenderer> chain, ref SpriteRenderer lastElement)
    {
        var nextPart = DefineNextPart();
        var offset = GetHalfSize(_lastPart) + GetHalfSize(nextPart);
        var positionForNextPart = new Vector3(_lastPart.transform.position.x + offset, _startingPosition.y, _startingPosition.z);
        var instanceOfNextPart = Instantiate<SpriteRenderer>(nextPart, positionForNextPart, Quaternion.identity);
        chain.Enqueue(instanceOfNextPart);
        lastElement = instanceOfNextPart;
    }

    SpriteRenderer DefineNextPart()
    {
        if(_bossMode)
        {
            return BossLevel;
        }
        else if(_advancedMode)
        {
            var length = LevelParts.Length;
            var index = UnityEngine.Random.Range(0, length);
            
            while/*already*/(_spawned[index])
            {
                index++;

                if(index == length)
                    index = 0;
            }

            _spawnedIndexes.Enqueue(index);
            _spawned[index] = true;

            return LevelParts[index];
        }
        else
        {
            var index = UnityEngine.Random.Range(0, LevelParts.Length);
            return LevelParts[index];
        }
    }

    private void OnDrawGizmos()
    {
        if (DrawGizmos && Application.isPlaying)
        {
            Gizmos.DrawCube(new Vector3(_bound, _startingPosition.y, _startingPosition.z), new Vector3(0.1f, 2f, 0.1f));
            Gizmos.DrawSphere(_currentPosition, 0.5f);
        }
    }
}
