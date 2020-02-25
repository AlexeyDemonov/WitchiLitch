using System;

public class SettingsController
{
    static SettingsController _singleInstance;
    SettingsEventArgs _currentSettings;

    public static SettingsController GetInstance() => _singleInstance;

    public event Action<SettingsEventArgs> SettingsChanged;

    static SettingsController()
    {
        if (_singleInstance == null)//Actually this 'if' statement is not needed, this ctor will be called only once
            _singleInstance = new SettingsController();
    }

    private SettingsController()
    {
        _currentSettings = LoadSettings();
    }

    SettingsEventArgs LoadSettings()
    {
        SettingsEventArgs container = new SettingsEventArgs();
        container.MusicOn = Load("MusicOn");
        container.SoundOn = Load("SoundOn");

        return container;
    }

    bool Load(string settingName)
    {
        int value = UnityEngine.PlayerPrefs.GetInt(settingName, /*by default:*/1);
        return value == 1;
    }

    public bool MusicOn => _currentSettings.MusicOn;
    public bool SoundOn => _currentSettings.SoundOn;

    public SettingsEventArgs Handle_LoadSettingsRequest() => _currentSettings;

    public void Handle_SaveSettingsRequest(SettingsEventArgs args)
    {
        SaveSettings(args);
    }

    void SaveSettings(SettingsEventArgs container)
    {
        _currentSettings = container;
        SettingsChanged?.Invoke(container);

        Save("MusicOn", container.MusicOn);
        Save("SoundOn", container.SoundOn);
    }

    void Save(string settingName, bool value)
    {
        int valueToSave = (value == true) ? 1 : 0;
        UnityEngine.PlayerPrefs.SetInt(settingName, valueToSave);
    }
}