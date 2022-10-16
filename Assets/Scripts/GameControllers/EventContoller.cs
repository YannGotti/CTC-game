using UnityEngine;
using UnityEngine.Events;

public class EventContoller : MonoBehaviour
{
    public static EventContoller singleton = null;

    private void Awake() { if (singleton == null) singleton = this; }

    public UnityEvent<GameObject> OnSlideCell = new UnityEvent<GameObject>();

    public UnityEvent<GameObject , int> OnDownCell = new UnityEvent<GameObject, int>();

    public UnityEvent OnGameOver = new UnityEvent();

    public UnityEvent<int, int, Vector2, int> AnimationSlideCell = new UnityEvent<int, int, Vector2, int>();

    public UnityEvent<Transform, string> MoveHeadSnake = new();

    public UnityEvent OnComboCell = new UnityEvent();

    public UnityEvent StartGame = new UnityEvent();
}
