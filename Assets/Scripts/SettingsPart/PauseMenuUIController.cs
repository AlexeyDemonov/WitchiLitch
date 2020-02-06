using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUIController : MonoBehaviour
{
    public Button PauseButton;
    public GameObject ControlsUI;
    public GameObject PauseUI;
    public Button ResumeButton;
    public Button RestartButton;
    public Button ExitButton;
    public float OnCrushDelayBeforeShowUI;

    WaitForSeconds _onCrushDelay;
    Coroutine _showUIAfterDelay;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        PauseButton.onClick.AddListener(PauseGame);
        ResumeButton.onClick.AddListener(ResumeGame);
        RestartButton.onClick.AddListener(RestartGame);
        ExitButton.onClick.AddListener(ExitGame);

        if(PauseUI.activeSelf)
            PauseUI.SetActive(false);

        _onCrushDelay = new WaitForSeconds(OnCrushDelayBeforeShowUI);
    }

    // Sent to all game objects when the player pauses
    private void OnApplicationPause(bool pause)
    {
        if(pause == true)
            PauseGame();
    }

    public void Handle_PlayerCrashed()
    {
        if(_showUIAfterDelay == null)//Prevent double invoking
        {
            ResumeButton.onClick.RemoveListener(ResumeGame);
            ResumeButton.gameObject.SetActive(false);
            _showUIAfterDelay = StartCoroutine(ShowUIAfterDelay());
        }
    }

    IEnumerator ShowUIAfterDelay()
    {
        yield return _onCrushDelay;
        _showUIAfterDelay = null;
        PauseGame();
    }

    void PauseGame()
    {
        if(_showUIAfterDelay != null)
        {
            StopCoroutine(_showUIAfterDelay);
            _showUIAfterDelay = null;
        }

        ControlsUI.SetActive(false);
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
        ControlsUI.SetActive(true);
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ExitGame()
    {
        ResumeGame();
        SceneManager.LoadScene(0);//Main menu
    }
}
