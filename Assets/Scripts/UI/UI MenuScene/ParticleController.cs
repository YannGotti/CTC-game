using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem _particle;

    private void Start()
    {
        _particle = GetComponent<ParticleSystem>();

        StartCoroutine(ChangeShapeRotation());
    }

    private IEnumerator ChangeShapeRotation()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.5f);

            var particleShape = _particle.shape;
            particleShape.rotation = new Vector3(0, 0, Random.Range(350, 370));

            yield return null;
        }
    }
}
