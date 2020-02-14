using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInitializer : MonoBehaviour
{
    public MainMenuUIController MainMenuUIController;
    MusicBox _musicBox;
    SettingsController _settingsController;

    static bool _firstLaunchPassed;

    // Start is called before the first frame update
    void Start()
    {
        _musicBox = MusicBox.GetInstance();
        _settingsController = SettingsController.GetInstance();

        var settings = _settingsController.Handle_LoadSettingsRequest();

        MainMenuUIController.AcceptSettings(settings);

        if(!_firstLaunchPassed)
        {
            _musicBox.AcceptSettings(settings);
            _settingsController.SettingsChanged += _musicBox.AcceptSettings;
            _firstLaunchPassed = true;
        }

        MainMenuUIController.SettingsChanged += _settingsController.Handle_SaveSettingsRequest;

        _musicBox.Play(0);
    }

    // This function is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        MainMenuUIController.SettingsChanged -= _settingsController.Handle_SaveSettingsRequest;
    }
}
