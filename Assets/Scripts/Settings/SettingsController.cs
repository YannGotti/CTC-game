using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Tooltip("Choose which GameSettings asset to use")]
    public GameSettings _settings;

    [SerializeField]
    public static GameSettings s;

    [SerializeField] Toggle _toggleFullscreen;
    [SerializeField] Toggle _toggleBloom;
    [SerializeField] Toggle _toggleVignette;

    [SerializeField] PostProcessVolume _postProcessVolume;

    [SerializeField] Bloom _bloom;
    [SerializeField] Vignette _vignette;

    [SerializeField] Slider _allSoundValue;
    [SerializeField] Slider _effectSoundValue;
    [SerializeField] Slider _musicSoundValue;

    [SerializeField] AudioSource _audioSource;


    void Awake()
    {
        if (s == null) s = _settings;
    }

    private void Start()
    {
        _postProcessVolume = GetComponent<PostProcessVolume>();

        _postProcessVolume.profile.TryGetSettings(out _bloom);
        _postProcessVolume.profile.TryGetSettings(out _vignette);

        LoadSettings();
    }

    private void LoadSettings()
    {
        if (_toggleFullscreen != null)
        {
            _toggleFullscreen.isOn = s.FullscreenMode;
            _toggleBloom.isOn = s.Bloom;
            _toggleVignette.isOn = s.VignetteEffect;
        }

        Screen.fullScreen = s.FullscreenMode;
        _bloom.active = s.Bloom;
        _vignette.active = s.VignetteEffect;

        _musicSoundValue.value= s.MusicSoundValue;
        _allSoundValue.value= s.AllSoundValue;
        _effectSoundValue.value= s.EffectSoundValue;
    }

    public void OnFullscreenMode(bool mode) => Screen.fullScreen =  s.FullscreenMode = _toggleFullscreen.isOn;

    public void OnBloom(bool mode) => _bloom.active = s.Bloom = _toggleBloom.isOn;

    public void OnVignetteMode(bool mode) => _vignette.active = s.VignetteEffect = _toggleVignette.isOn;

    public void OnSliderAllSoundValue() => _audioSource.volume = s.AllSoundValue = _allSoundValue.value;
    public void OnSliderEffectSoundValue() => s.EffectSoundValue = _effectSoundValue.value;
    public void OnSliderMusicSoundValue() => _audioSource.volume = s.MusicSoundValue = _musicSoundValue.value;

}
