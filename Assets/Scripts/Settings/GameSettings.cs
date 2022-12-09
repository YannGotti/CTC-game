using UnityEngine;

[CreateAssetMenu(fileName = "DataSettings", menuName = "Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    [Header("VideoSettings")]
    public bool FullscreenMode;
    public bool Bloom;
    public bool VignetteEffect;

    [Header("AudioSettings")]
    public float AllSoundValue;
    public float EffectSoundValue;
    public float MusicSoundValue;
}
