using UnityEngine;

public class SoundEffectsBox : MonoBehaviour
{
    public AudioClip[] JumpSounds;
    public AudioClip[] HitSounds;
    public AudioClip[] DashSounds;
    public AudioClip[] FruitSmashSounds;
    public AudioClip[] DieSounds;
    public AudioClip EasterEggFallSound;

    [Range(0f, 1f)]
    public float SoundEffectsVolume;

    Vector3 _position;
    bool _muted;

    private void Awake()
    {
        _position = this.transform.position;
    }

    public void AcceptSettings(SettingsEventArgs args)
    {
        _muted = !args.SoundOn;
    }

    public void Handle_PlayerAction(PlayerActionType actionType)
    {
        switch (actionType)
        {
            case PlayerActionType.Jump:
                PlayRandomSound(JumpSounds);
                break;

            case PlayerActionType.DashForward:
            case PlayerActionType.DashDown:
                PlayRandomSound(DashSounds);
                break;
        }
    }

    public void Handle_PlayerHittedObject(HitDetectionEventArgs args)
    {
        switch (args.HittedObjectType)
        {
            case HittedObjectType.Platform:
                PlayRandomSound(HitSounds);
                break;

            case HittedObjectType.Enemy:/*Presume it means that player smashed the enemy, otherwise PlayerCrashed event would have been invoked*/
                PlayRandomSound(FruitSmashSounds);
                break;
        }
    }

    public void Handle_PlayerCrashed()
    {
        PlayRandomSound(DieSounds);
    }

    public void Handle_PlayerFallen()
    {
        if (UnityEngine.Random.Range(0, 5) == 0)
            PlayRandomSound(EasterEggFallSound);
        else
            PlayRandomSound(DieSounds);
    }

    void PlayRandomSound(params AudioClip[] sounds)
    {
        if (_muted)
            return;

        int length = sounds.Length;

        AudioClip soundToPlay = null;

        if (length == 0)
            return;
        else if (length > 1)
            soundToPlay = sounds[UnityEngine.Random.Range(0, length)];
        else/*if(length == 1)*/
            soundToPlay = sounds[0];

        //I could not find info on performance issues with this approach, but it instantiates AudioSource to play sound and then disposes it, so keep an eye on GC allocations
        //https://docs.unity3d.com/ScriptReference/AudioSource.PlayClipAtPoint.html
        AudioSource.PlayClipAtPoint(soundToPlay, _position, SoundEffectsVolume);
    }
}