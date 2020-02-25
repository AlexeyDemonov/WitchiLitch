using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScoreManager))]
public class ScoreManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Label($"Current record: {UnityEngine.PlayerPrefs.GetInt("ScoreRecord", /*by default*/0)}");

        if (GUILayout.Button("Reset record"))
        {
            UnityEngine.PlayerPrefs.SetInt("ScoreRecord", 0);
        }
    }
}