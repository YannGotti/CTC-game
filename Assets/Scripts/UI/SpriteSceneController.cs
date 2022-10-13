using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSceneController : MonoBehaviour
{
    [SerializeField] private Color colorEnd1; 
    [SerializeField] private Color colorEnd2;
    [SerializeField] private float _speedAnimation = 2.5f; 

    private bool _isColor;

    private Image _imageLogo;

    private void Start()
    {
        _imageLogo = GetComponent<Image>();
    }

    private void Update()
    {
        if (_isColor)
        {
            _imageLogo.color = Color.Lerp(_imageLogo.color, colorEnd1, _speedAnimation * Time.deltaTime);
            _imageLogo.rectTransform.localScale = Vector3.Lerp(_imageLogo.rectTransform.localScale, new Vector3(1.5f, 1.5f), _speedAnimation * Time.deltaTime);
        }
        else
        {
            _imageLogo.color = Color.Lerp(_imageLogo.color, colorEnd2, _speedAnimation * Time.deltaTime);
            _imageLogo.rectTransform.localScale = Vector3.Lerp(_imageLogo.rectTransform.localScale, new Vector3(1, 1), _speedAnimation * Time.deltaTime);
        }
    }

    private void OnMouseEnter()
    {
        _isColor = true; 
    }

    private void OnMouseExit()
    {
        _isColor = false;
    }
}
