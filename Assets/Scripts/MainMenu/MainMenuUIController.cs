using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public GameObject MainUI;
    public Button StartButton;
    public Button SettingsButton;
    public Button ExitButton;

    public GameObject SettingsUI;
    public Toggle MusicToggle;
    public Toggle SoundToggle;
    public Button CloseSettingsButton;

    public event Action<SettingsEventArgs> SettingsChanged;

    public void AcceptSettings(SettingsEventArgs args)
    {
        MusicToggle.isOn = args.MusicOn;
        SoundToggle.isOn = args.SoundOn;
    }

    void Start()
    {
        StartButton?.onClick.AddListener(StartGame);
        ExitButton?.onClick.AddListener(ExitGame);
        SettingsButton?.onClick.AddListener(ShowSettings);
        CloseSettingsButton?.onClick.AddListener(HideSettings);

        MusicToggle?.onValueChanged.AddListener(Handle_MusicSettingChanged);
        SoundToggle?.onValueChanged.AddListener(Handle_SoundSettingChanged);
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

    void ShowSettings()
    {
        MainUI.SetActive(false);
        SettingsUI.SetActive(true);
    }

    void HideSettings()
    {
        SettingsUI.SetActive(false);
        MainUI.SetActive(true);
    }

    void Handle_MusicSettingChanged(bool newMusicValue)
    {
        ReportSettings(music: newMusicValue, sound: SoundToggle.isOn);
    }

    void Handle_SoundSettingChanged(bool newSoundValue)
    {
        ReportSettings(music: MusicToggle.isOn, sound: newSoundValue);
    }

    void ReportSettings(bool music, bool sound)
    {
        var newSettings = new SettingsEventArgs() { MusicOn = music, SoundOn = sound };
        SettingsChanged?.Invoke(newSettings);
    }
}