using UnityEngine;
using UnityEngine.Events;

public class EventContoller : MonoBehaviour
{
    public static EventContoller singleton = null;

    private void Awake()
    {
        if (singleton == null) singleton = this;
    }

    public UnityEvent<GameObject> OnSlideCell = new UnityEvent<GameObject>();

    public UnityEvent OnGameOver = new UnityEvent();

}
