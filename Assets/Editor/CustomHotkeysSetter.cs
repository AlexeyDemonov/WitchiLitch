using UnityEngine;
using UnityEditor;

public class CustomHotkeysSetter
{
    [MenuItem("Edit/CustomHotkeys/PlayOrStop _F5")]
    static void PlayOrStopGame()
    {
        if (!EditorApplication.isPlaying)
            EditorApplication.ExecuteMenuItem("File/Save");

        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    [MenuItem("GameObject/CustomHotkeys/ResetTransform #R")]
    static void ResetTransform()
    {
        foreach (var gameObject in Selection.gameObjects)
        {
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;
        }
    }

    [MenuItem("GameObject/CustomHotkeys/ToggleActive #A")]
    static void ToggleActive()
    {
        foreach (var gameObject in Selection.gameObjects)
        {
            gameObject.SetActive( !gameObject.activeSelf );
        }
    }
}