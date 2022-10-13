using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipUI : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string _textHelp;

    [SerializeField] private GameObject _prefab; 
    [SerializeField] private GameObject _toolTip;

    private Transform _canvasTransform;

    private void Start()
    {
        _canvasTransform = GameObject.Find("Canvas").transform; 
    }
    private void OnMouseDown()
    {
        if (_prefab == null)
            return;

        if (_textHelp == null)
            return;

        if (_toolTip != null)
            return;

        var position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

        _toolTip = Instantiate(_prefab, position, Quaternion.identity, _canvasTransform);
        _toolTip.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = _textHelp; 
    }

    private void OnMouseExit()
    {
        Destroy(_toolTip);
    }
}
