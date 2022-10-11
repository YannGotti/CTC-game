using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [Header("Fields")]
    [SerializeField] private Button _pauseButton;

    private void Start()
    {
        _pauseButton.onClick.AddListener(PauseUI);
        EventContoller.singleton.OnComboCell.AddListener(OnComboCell);
    }

    private void PauseUI()
    {
        //Debug.Log("aSD");
    }

    private void OnComboCell()
    {
        //Debug.Log("Комбо");
    }



}
