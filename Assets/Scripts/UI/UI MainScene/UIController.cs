using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _pausePrefab;
    [SerializeField] private AudioSource[] _allAudio;

    [Header("Settings")]
    [SerializeField] private float _cooldown;

    private GameObject _currentPause;

    private Slider _sliderAudio;

    private Transform _canvas;

    private bool _timeCooldown = true;
    private bool _isPause = false;


    private void Start()
    {
        _pauseButton.onClick.AddListener(OpenPause);
        _canvas = GameObject.Find("Canvas").transform;
        EventContoller.singleton.OnComboCell.AddListener(OnComboCell);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && _timeCooldown)
        {
            _timeCooldown = false;
            _isPause = !_isPause;

            if (_isPause) OpenPause();

            if (!_isPause) ClosePause();
        }
    }

    private void OpenPause() => StartCoroutine(OpenPauseUI());
    private void ClosePause() => StartCoroutine(ClosePauseUI());

    private IEnumerator OpenPauseUI()
    {
        StartCoroutine(IsEscape());

        yield return new WaitForSeconds(0.05f);

        if (_currentPause != null) yield break;

        _currentPause = Instantiate(_pausePrefab, _canvas);

        _sliderAudio = _currentPause.GetComponentInChildren<Slider>();

        _sliderAudio.onValueChanged.AddListener(SliderValue);

        _sliderAudio.value = MiddleVolumeSound();

        yield break;
    }

    private float MiddleVolumeSound()
    {
        float middleVolume = 0;

        foreach (var sound in _allAudio)
            middleVolume += sound.volume;

        middleVolume /= _allAudio.Length;

        return middleVolume;
    }

    private IEnumerator ClosePauseUI()
    {
        StartCoroutine(IsEscape());

        yield return new WaitForSeconds(0.05f);

        _sliderAudio = null;
        if (_currentPause == null) yield break;

        Destroy(_currentPause);

        yield break;

    }

    private IEnumerator IsEscape()
    {
        yield return new WaitForSeconds(_cooldown);

        _timeCooldown = true;

        yield break;
    }

    private void OnComboCell()
    {
        //Debug.Log("Комбо");
    }

    public void SliderValue(float value)
    {
        foreach (var sound in _allAudio)
            sound.volume = value;
    }
    



}
