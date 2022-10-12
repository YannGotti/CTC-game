using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _em;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _em = _particleSystem.emission;
    }

    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }

    private void OnCollisionEnter2D()
    {
        _particleSystem.Play();

        Debug.Log("коснулся");
    }
}
