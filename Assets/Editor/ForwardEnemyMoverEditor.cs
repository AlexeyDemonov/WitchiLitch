using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ForwardEnemyMover))]
public class ForwardEnemyMoverEditor : Editor
{
    ForwardEnemyMover _targetInstance;
    FieldInfo _actualMoveSpeedFieldInfo;

    // This function is called when the script is started
    private void Awake()
    {
        _targetInstance = (ForwardEnemyMover)target;
        _actualMoveSpeedFieldInfo = _targetInstance.GetType().GetField("_actualMoveSpeed", BindingFlags.Instance | BindingFlags.NonPublic);
    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var fieldValue = (float)_actualMoveSpeedFieldInfo.GetValue(_targetInstance);
        GUILayout.Label($"Actual speed: {fieldValue}");
    }
}
