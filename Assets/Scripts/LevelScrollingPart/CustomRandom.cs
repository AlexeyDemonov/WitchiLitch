using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRandom : MonoBehaviour
{
    public int MinDistance;
    public int MaxAttempts;

    System.Random _random;
    int _lastResult;

    private void Awake()
    {
        _lastResult = -MinDistance;

        var time = DateTime.Now;
        int seed = time.Hour + time.Minute + time.Second;
        _random = new System.Random(seed);
    }

    public int Range(int min, int max)
    {
        int result = default(int);

        for (int i = 0; i < MaxAttempts; i++)
        {
            result = _random.Next(min, max);

            if (Math.Abs(result - _lastResult) > MinDistance)
                break;
        }

        _lastResult = result;
        return result;
    }
}
