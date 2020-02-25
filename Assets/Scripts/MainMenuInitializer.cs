using UnityEngine;

public class MainMenuInitializer : MonoBehaviour
{
    public MainMenuUIController MainMenuUIController;
    public BaseScroller[] Scrollers;
    public ScrollSpeedDefiner ScrollSpeedDefiner;

    MusicBox _musicBox;
    SettingsController _settingsController;

    static bool _firstLaunchPassed;

    private void Awake()
    {
        foreach (var scroller in Scrollers)
        {
            scroller.Request_ScrollSpeed += ScrollSpeedDefiner.GetCurrentScrollSpeed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _musicBox = MusicBox.GetInstance();
        _settingsController = SettingsController.GetInstance();

        var settings = _settingsController.Handle_LoadSettingsRequest();

        MainMenuUIController.AcceptSettings(settings);

        if (!_firstLaunchPassed)
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