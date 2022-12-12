using UnityEngine;

public class AudioPlayController : MonoBehaviour
{

    [SerializeField] private AudioSource _buttonClick;

    void Start() => EventContoller.singleton.PlaySound.AddListener(PlaySound);

    private void PlaySound(AudioSource audio)
    {
        if (audio == null) return;

        audio.Play();
    }

    public void PlayButtonSound()
    {
        if (_buttonClick == null) return;

        _buttonClick.Play();
    }
}
