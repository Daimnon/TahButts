using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    [Header("Music")]
    [Range(0.0f, 1.0f)][SerializeField] private float _musicVolume = 1.0f;
    public float MusicVolume => _musicVolume;

    [SerializeField] private AudioSource _musicSource;
    public AudioSource MusicSource => _musicSource;

    [SerializeField] private AudioClip _mainMenuMusicClip, _level01MusicClip;

    [Header("Ambience")]
    [Range(0.0f, 1.0f)][SerializeField] private float _ambienceVolume = 1.0f;
    public float AmbienceVolume => _ambienceVolume;

    [SerializeField] private AudioSource _ambienceSource;
    public AudioSource AmbienceSource => _ambienceSource;

    [Header("Pop Up")]
    [Range(0.0f, 1.0f)][SerializeField] private float _popUpVolume = 1.0f;
    public float PopUpVolume => _popUpVolume;

    [SerializeField] private AudioSource _popUpSource;
    public AudioSource PopUpSource => _popUpSource;

    [SerializeField] private AudioClip[] _popUpClips; // _popUpClips[0] = lose, _popUpClips[1] = true.
    public AudioClip[] PopUpClips => _popUpClips;

    private void Awake()
    {
        _instance = this;
        Initialize();
        /*DontDestroyOnLoad(this);*/
    }
    private void Start()
    {
        switch (CustomSceneManager.currentSceneID)
        {
            case SceneID.MainMenu:
                PlaySource(_musicSource);
                PlaySource(_ambienceSource);
                break;
            case SceneID.LevelSelect:
                PlaySource(_musicSource);
                PlaySource(_ambienceSource);
                break;
            case SceneID.Level01:
                PlaySource(_musicSource);
                break;
            case SceneID.Level02:
                break;
            case SceneID.Level03:
                break;
        }
        /*SceneManager.activeSceneChanged += OnSceneChanged;*/
    }
    /*private void OnSceneChanged(Scene current, Scene next)
    {
        switch (CustomSceneManager.currentSceneID)
        {
            case SceneID.MainMenu:
                break;
            case SceneID.LevelSelect:
                _ambienceSource.Stop();
                break;
            case SceneID.Level01:
                _musicSource.Stop();
                _musicSource.clip = _level01MusicClip;
                _musicSource.Play();
                break;
            case SceneID.Level02:
                break;
            case SceneID.Level03:
                break;
        }
    }*/
    
    private void Initialize()
    {
        _musicSource.volume = _musicVolume;
        _ambienceSource.volume = _ambienceVolume;
    }

    private IEnumerator FadeOutOneShot(AudioSource source, AudioClip clip, float fadeTime)
    {
        /* set-up coroutine */
        float time = 0;
        float startValue = source.volume;
        float endValue = 0;

        /* lowering volume to 0 over 'fadeTime' seconds */
        while (time < fadeTime)
        {
            source.volume = Mathf.Lerp(startValue, endValue, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }
        source.volume = endValue;

        /* returns source volume to 'startValue' after the 'fadeTime' ends*/
        yield return null;
        source.volume = startValue;
    }

    public void PlaySource(AudioSource source)
    {
        source.Play();
    }
    public void ChangeAudioClip(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
    }
    public void ChangeAudioClips(AudioSource source, AudioClip[] clips)
    {
        int clipIndex = 0;
        AudioClip clip;

        clipIndex = UnityEngine.Random.Range(0, clips.Length);
        clip = clips[clipIndex];

        source.PlayOneShot(clip);
    }
    public void ChangeMusicClip(AudioClip clip)
    {
        _musicSource.clip = clip;
    }
    public void ChangeAmbienceClip(AudioClip clip)
    {
        _ambienceSource.clip = clip;
    }
    public void ChangePopUpClip(bool isWin)
    {
        if (!isWin)
            _popUpSource.clip = _popUpClips[0];
        else
            _popUpSource.clip = _popUpClips[1];
    }
    public void StopSource(AudioSource source)
    {
        source.Stop();
    }
    public void PlayFadeOutOneShot(AudioSource source, AudioClip clip, float fadeTime)
    {
        StartCoroutine(FadeOutOneShot(source, clip, fadeTime));
    }
}
