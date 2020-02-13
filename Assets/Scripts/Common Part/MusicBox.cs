using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicBox : MonoBehaviour
{
    public AudioClip[] AudioClips;
    public float FadeStep;
    public float MaxVolume;

    static MusicBox _singleInstance;
    int _curentlyPlaying;
    AudioSource _player;

    Coroutine _fadeOutFadeinCoroutine;

    public static MusicBox GetInstance() => _singleInstance;

    private void Awake()
    {
        if(_singleInstance == null)
        {
            _singleInstance = this;
            _curentlyPlaying = -1;
            _player = GetComponent<AudioSource>();
            _player.volume = MaxVolume;
            DontDestroyOnLoad(this.gameObject);
        }
        else/*this is a second instance*/
        {
            Destroy(this.gameObject);
        }
    }

    public void Play(int clipIndex)
    {
        if(clipIndex != _curentlyPlaying && clipIndex < AudioClips.Length)
        {
            if(_fadeOutFadeinCoroutine != null)
            {
                StopCoroutine(_fadeOutFadeinCoroutine);
            }

            _fadeOutFadeinCoroutine = StartCoroutine(FadeOutFadeIn(clipIndex));
            _curentlyPlaying = clipIndex;
        }
    }

    IEnumerator FadeOutFadeIn(int newClipIndex)
    {
        //Fade out
        do
        {
            _player.volume -= FadeStep;
            yield return null;
        } while(_player.volume > 0f);


        //Change clip
        _player.Stop();
        _player.clip = AudioClips[newClipIndex];
        _player.Play();

        //Fade in
        do
        {
            _player.volume += FadeStep;
            yield return null;
        } while (_player.volume < MaxVolume);

        _fadeOutFadeinCoroutine = null;
    }
}
