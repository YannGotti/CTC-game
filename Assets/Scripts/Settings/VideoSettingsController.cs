using UnityEngine;

public class VideoSettingsController : MonoBehaviour
{
    public void OnFullScreenMode(bool mode) => SettingsController.s.FullscreenMode = mode;
    public void OnBloomMode(bool mode) => SettingsController.s.Bloom = mode;
    public void OnVignetteEffectMode(bool mode) => SettingsController.s.VignetteEffect = mode;


}
