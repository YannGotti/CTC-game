using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LoadSettings : MonoBehaviour
{
    [Tooltip("Choose which GameSettings asset to use")]
    public GameSettings _settings;

    [SerializeField]
    public static GameSettings s;

    [SerializeField] private List<AudioSource> _allSounds;
    [SerializeField] private List<AudioSource> _effectSounds;
    [SerializeField] private List<AudioSource> _musicSounds;

    private void Awake()
    {
        if (s == null) s = _settings;
    }

    private void Start() => Load();

    private void Load()
    {

        PostProcessVolume _postProcessVolume = GetComponent<PostProcessVolume>();

        _postProcessVolume.profile.TryGetSettings(out Bloom bloom);
        _postProcessVolume.profile.TryGetSettings(out Vignette vignette);

        LoadBloom(bloom);

        LoadVignette(vignette);

        LoadFullscreenMode();

        LoadAllSoundValue();

        LoadEffectSoundValue();

        LoadMusicSoundValue();
    }


    private void LoadMusicSoundValue()
    {
        if (_musicSounds == null) return;

        foreach (var item in _musicSounds)
            item.volume = s.MusicSoundValue;
    }

    private void LoadEffectSoundValue()
    {
        if (_effectSounds == null) return;

        foreach (var item in _effectSounds)
            item.volume = s.EffectSoundValue;
    }

    private void LoadAllSoundValue()
    {
        if (_allSounds == null) return;

        foreach (var item in _allSounds)
            item.volume = s.AllSoundValue;
    }

    private void LoadBloom(Bloom bloom) => bloom.active = s.Bloom;

    private void LoadVignette(Vignette vignette) => vignette.active = s.VignetteEffect;

    private void LoadFullscreenMode() => Screen.fullScreen = s.FullscreenMode;

}
