using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCamera;

    void Start()
    {
        StartCoroutine(AnimationZoomCamera());
    }

    IEnumerator AnimationZoomCamera()
    {
        bool action = true;
        float _currentTimeCurve = 0;

        float _totalTimeCurve = _animationCamera.keys[_animationCamera.keys.Length - 1].time;

        while (action)
        {
            transform.position = new Vector3(0, 0, _animationCamera.Evaluate(_currentTimeCurve));

            _currentTimeCurve += Time.deltaTime;
            action = _totalTimeCurve >= _currentTimeCurve;

            yield return null;
        }

        yield break;
    }
    
}
