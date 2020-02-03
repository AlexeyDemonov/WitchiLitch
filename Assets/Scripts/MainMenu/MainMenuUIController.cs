using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public Button StartButton;
    public Button ExitButton;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        StartButton?.onClick.AddListener(StartGame);
        ExitButton?.onClick.AddListener(ExitGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
