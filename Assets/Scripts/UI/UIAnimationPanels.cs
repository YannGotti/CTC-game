using System.Collections;
using UnityEngine;

public class UIAnimationPanels : MonoBehaviour
{
    [SerializeField] private float _speedAnimation;

    private Transform _gameObject;


    private void Start()
    {
        _speedAnimation = 10f;
        _gameObject = gameObject.transform;
    }

    private void OnMouseEnter()
    {
        StopAllCoroutines();
        StartCoroutine(AnimationPanel());
    }

    private void OnMouseExit()
    {
        StopAllCoroutines();
        StartCoroutine(ResetPanel());
    }

    private IEnumerator AnimationPanel()
    {
        float timer = 0;

        while (timer < _speedAnimation)
        {

            _gameObject.localEulerAngles = Vector3.Lerp(_gameObject.localEulerAngles, new Vector3(5, 5), _speedAnimation * Time.deltaTime);

            timer += Time.deltaTime;

            yield return null;
        }

        yield break;
    }

    private IEnumerator ResetPanel()
    {
        float timer = 0;

        while (timer < _speedAnimation)
        {
            _gameObject.localEulerAngles = Vector3.Lerp(_gameObject.localEulerAngles, Vector3.zero, _speedAnimation * Time.deltaTime);

            timer += Time.deltaTime;

            yield return null;
        }

        yield break;
    }


}
