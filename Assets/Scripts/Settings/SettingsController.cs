using UnityEditor;
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


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
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
    }

    public void OnFullscreenMode(bool mode) => Screen.fullScreen =  s.FullscreenMode = _toggleFullscreen.isOn;

    public void OnBloom(bool mode) => _bloom.active = s.Bloom = _toggleBloom.isOn;

    public void OnVignetteMode(bool mode) => _vignette.active = s.VignetteEffect = _toggleVignette.isOn;




}
