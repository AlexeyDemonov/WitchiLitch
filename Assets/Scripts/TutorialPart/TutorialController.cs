using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public Button[] TutorialButtons;
    public GameObject[] TutorialPanels;
    public GameObject[] TutorialScenes;

    private int _index = 0;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        foreach (var button in TutorialButtons)
        {
            button.onClick.AddListener(ShowNextTutorial);
        }
    }

    void ShowNextTutorial()
    {
        TutorialPanels[_index].SetActive(false);
        TutorialScenes[_index].SetActive(false);

        _index++;

        if(_index < TutorialPanels.Length)
        {
            TutorialPanels[_index].SetActive(true);
            TutorialScenes[_index].SetActive(true);
        }
        else
        {
            ExitTutorial();
        }
    }

    void ExitTutorial()
    {
        SceneManager.LoadScene(0);
    }
}
