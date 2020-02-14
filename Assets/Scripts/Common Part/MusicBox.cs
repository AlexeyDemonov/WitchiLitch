using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicBox : MonoBehaviour
{
    public AudioClip[] AudioClips;
    public float FadeStep;
    [Range(0f,1f)]
    public float Volume;

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
        while(_player.volume > 0f)
        {
            _player.volume -= FadeStep;
            yield return null;
        }

        //Change clip
        _player.Stop();
        _player.clip = AudioClips[newClipIndex];
        _player.Play();

        //Fade in
        while (_player.volume < Volume)
        {
            _player.volume += FadeStep;
            yield return null;
        }

        _fadeOutFadeinCoroutine = null;
    }
}
