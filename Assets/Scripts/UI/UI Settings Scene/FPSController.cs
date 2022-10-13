using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _fpsText; 

    private void Start() => 
        _fpsText = GetComponent<TMPro.TextMeshProUGUI>();

    private void Update() => 
        _fpsText.text = $"FPS: {(int)(1 / Time.deltaTime)}"; 
}
