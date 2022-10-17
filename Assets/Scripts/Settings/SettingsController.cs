using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [Tooltip("Choose which GameSettings asset to use")]
    public GameSettings _settings;

    [SerializeField]
    public static GameSettings s;
    public static SettingsController instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("A previously awakened Settings MonoBehaviour exists!", gameObject);
        }
        if (s == null)
        {
            s = _settings;
        }
    }
}
