using System.Collections;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [Header("Settings snake")]
    [SerializeField] private GameController _gameController;
    [SerializeField] private Sprite _colorSnake;
    [SerializeField] private int _lastPositionIndex = -1;
    [SerializeField] private int _currentPositionIndex;
    [SerializeField] private float _cooldown;

    [Header("Settings animation")]
    [SerializeField] private float _animationSpeed;

    private bool _cooldownMove = true;

    private Transform _headTransform;
    private Transform _bodyOneTransform;
    private Transform _bodyTwoTransform;


    private void Start()
    {
         EventContoller.singleton.OnDownCell.AddListener(OnDownCell);
        _headTransform = transform.GetChild(0);
        _bodyOneTransform = transform.GetChild(1);
        _bodyTwoTransform = transform.GetChild(2);
    }

    private void OnDownCell(GameObject obj, int index)
    {
        if (!_cooldownMove) return;

        StartCoroutine(CooldownMove());

        _currentPositionIndex = index;

        if (!_gameController.IsBorderCell(_lastPositionIndex, index)) return;

        if (!_gameController.IsMovePlayer(index, _colorSnake)) return;

        _colorSnake = _gameController.SelectSprite(index);

        string side = _gameController.SelectSide(_lastPositionIndex, index);

        Move(side);
        StartCoroutine(MoveSnakeBody());

        EventContoller.singleton.StartGame.Invoke();

        _lastPositionIndex = index;
        StartCoroutine(_gameController.DestroyCell(_lastPositionIndex));

        _cooldownMove = false;

    }

    private void Move(string side)
    {
        if (side == null) return;

        EventContoller.singleton.MoveHeadSnake.Invoke(_headTransform, side);

        RotateSnakeHead(side); // rotate
        StartCoroutine(MoveSnakeHead(side)); // move animation

    }

    private void RotateSnakeHead(string side)
    {
        Vector3 rotation = new();

        if (side == "t") rotation = new Vector3(0, 0, -180);

        if (side == "b") rotation = new Vector3(0, 0, 0);

        if (side == "l") rotation = new Vector3(0, 0, -90);

        if (side == "r") rotation = new Vector3(0, 0, 90);

        _headTransform.eulerAngles = rotation;
    }

    private IEnumerator MoveSnakeHead(string side)
    {
        bool action = true;
        float _currentTimeCurve = 0;

        AnimationCurve _animationCurve = new AnimationCurve();
        Transform target = _gameController.SelectTransform(_currentPositionIndex);

        if (side == "t" || side == "b")
        {
            _animationCurve.AddKey(0, _headTransform.position.y);
            _animationCurve.AddKey(_animationSpeed * 0.5f, Mathf.Lerp(_headTransform.position.y, target.transform.position.y, 0));
            _animationCurve.AddKey(_animationSpeed, target.transform.position.y);
        }

        if (side == "l" || side == "r")
        {
            _animationCurve.AddKey(0, _headTransform.position.x);
            _animationCurve.AddKey(_animationSpeed * 0.5f, Mathf.Lerp(_headTransform.position.x, target.transform.position.x, 0));
            _animationCurve.AddKey(_animationSpeed, target.transform.position.x);
        }

        float _totalTimeCurve = _animationCurve.keys[_animationCurve.keys.Length - 1].time;

        while (action)
        {
            if (side == "t" || side == "b") _headTransform.transform.position = new Vector2(_headTransform.position.x, _animationCurve.Evaluate(_currentTimeCurve));

            if (side == "l" || side == "r") _headTransform.transform.position = new Vector2(_animationCurve.Evaluate(_currentTimeCurve), _headTransform.position.y);

            _currentTimeCurve += Time.deltaTime;
            action = _totalTimeCurve >= _currentTimeCurve;

            yield return null;
        }

        if (CanExit()) EventContoller.singleton.OnGameOver.Invoke(); // Конец игры Победа

        yield break;
    }

    private IEnumerator MoveSnakeBody()
    {
        bool action = true;
        float _currentTimeCurve = 0;

        AnimationCurve _animationCurveBodyOneY = new AnimationCurve();
        AnimationCurve _animationCurveBodyOneX = new AnimationCurve();

        AnimationCurve _animationCurveBodyTwoY = new AnimationCurve();
        AnimationCurve _animationCurveBodyTwoX = new AnimationCurve();

        _animationCurveBodyOneY.AddKey(0, _bodyOneTransform.position.y);
        _animationCurveBodyOneY.AddKey(_animationSpeed * 0.5f, Mathf.Lerp(_bodyOneTransform.position.y, _headTransform.position.y, 0));
        _animationCurveBodyOneY.AddKey(_animationSpeed, _headTransform.position.y);

        _animationCurveBodyTwoY.AddKey(0, _bodyTwoTransform.position.y);
        _animationCurveBodyTwoY.AddKey(_animationSpeed * 0.5f, Mathf.Lerp(_bodyTwoTransform.position.y, _bodyOneTransform.position.y, 0));
        _animationCurveBodyTwoY.AddKey(_animationSpeed, _bodyOneTransform.position.y);



        _animationCurveBodyOneX.AddKey(0, _bodyOneTransform.position.x);
        _animationCurveBodyOneX.AddKey(_animationSpeed * 0.5f, Mathf.Lerp(_bodyOneTransform.position.x, _headTransform.position.x, 0));
        _animationCurveBodyOneX.AddKey(_animationSpeed, _headTransform.position.x);

        _animationCurveBodyTwoX.AddKey(0, _bodyTwoTransform.position.x);
        _animationCurveBodyTwoX.AddKey(_animationSpeed * 0.5f, Mathf.Lerp(_bodyTwoTransform.position.x, _bodyOneTransform.position.x, 0));
        _animationCurveBodyTwoX.AddKey(_animationSpeed, _bodyOneTransform.position.x);

        float _totalTimeCurve = _animationCurveBodyOneY.keys[_animationCurveBodyOneY.keys.Length - 1].time;

        while (action)
        {
            _bodyOneTransform.transform.position = new Vector2(_bodyOneTransform.position.x, _animationCurveBodyOneY.Evaluate(_currentTimeCurve));

            _bodyOneTransform.transform.position = new Vector2(_animationCurveBodyOneX.Evaluate(_currentTimeCurve), _bodyOneTransform.position.y);


            _bodyTwoTransform.transform.position = new Vector2(_bodyTwoTransform.position.x, _animationCurveBodyTwoY.Evaluate(_currentTimeCurve));

            _bodyTwoTransform.transform.position = new Vector2(_animationCurveBodyTwoX.Evaluate(_currentTimeCurve), _bodyTwoTransform.position.y);

            _currentTimeCurve += Time.deltaTime;
            action = _totalTimeCurve >= _currentTimeCurve;

            yield return null;
        }

        yield break;
    }

    private IEnumerator CooldownMove()
    {
        yield return new WaitForSeconds(_cooldown);

        _cooldownMove = true;

        yield break;
    }

    private bool CanExit()
    {
        Vector3 position = _headTransform.position;

        if (position.x > 1.8f && position.x < 2.2f && position.y < 2 && position.y > 1.5f) return true;

        return false;
    }
}
