using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadedManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadedPanel;  
    [SerializeField] private Slider _progressBar;  
    [SerializeField] private TMPro.TextMeshProUGUI _progressBarValue;

    [SerializeField] private int _loadScene;

    private void Awake()
    {
        _loadedPanel.SetActive(false); 
    }

    public void LoadScene()
    {
        _loadedPanel.SetActive(true);

        StartCoroutine(LoadSceneAsync(_loadScene));
    }

    public void CloseGame()
    {
        Application.Quit(); 
    }

    private IEnumerator LoadSceneAsync(int countScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(countScene);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress);

            _progressBar.value = progress;
            _progressBarValue.text = (progress * 100f).ToString("F0") + "%"; 

            yield return null; 
        }

        yield break;
    }
}
