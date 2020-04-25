using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialShower : MonoBehaviour
{
    public Button ShowTutorialButton;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        ShowTutorialButton?.onClick.AddListener(ShowTutorial);
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        var isTutorialShowed = UnityEngine.PlayerPrefs.GetInt("IsTutorialShowed", /*by default:*/0) == 1 ? true : false;

        if (!isTutorialShowed)
        {
            UnityEngine.PlayerPrefs.SetInt("IsTutorialShowed", 1);
            ShowTutorial();
        }
    }

    void ShowTutorial()
    {
        SceneManager.LoadScene(2);
    }
}
