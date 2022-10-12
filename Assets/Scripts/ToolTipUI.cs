using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipUI : MonoBehaviour
{
    [SerializeField] private string _textHelp;

    [SerializeField] private float _offSet; 

    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _toolTip; 

    private Vector3 _positionMouse;

    void Start()
    {

    }

    private void OnMouseEnter()
    {
        _positionMouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)); 

        _toolTip = Instantiate(_prefab, _positionMouse + new Vector3(_offSet, _offSet), Quaternion.identity);
    }

    private void OnMouseExit()
    {
        Destroy(_toolTip);
    }
}
