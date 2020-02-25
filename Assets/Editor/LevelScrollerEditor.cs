using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelScroller))]
public class LevelScrollerEditor : Editor
{
    LevelScroller _targetInstance;

    // This function is called when the script is started
    private void Awake()
    {
        _targetInstance = (LevelScroller)target;

        _targetInstance.BossLevelReady += () => Debug.Log("Boss level ready");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Start boss level"))
        {
            _targetInstance.Handle_BossLevelStartRequest();
        }

        if (GUILayout.Button("Stop boss level"))
        {
            _targetInstance.Handle_BossLevelEndRequest();
        }
    }
}