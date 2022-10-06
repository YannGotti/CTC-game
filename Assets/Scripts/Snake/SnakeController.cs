using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SnakeController : MonoBehaviour
{
    [Header("Настройки змейки")]
    [SerializeField] private GameController _gameController;
    [SerializeField] private Sprite _colorSnake;
    [SerializeField] private int _lastPositionIndex = -1;
    [SerializeField] public int _currentPositionIndex;

    [Header("Настройки анимации")]
    [SerializeField] private float _animationSpeed;


    private Transform _head;
    private Transform _body_0;
    private Transform _body_1;

    private void Start()
    {
        EventContoller.singleton.OnDownCell.AddListener(OnDownCell);
        _head = transform.GetChild(0);
        _body_0 = transform.GetChild(1);
        _body_1 = transform.GetChild(2);
    }

    private void OnDownCell(GameObject obj, int index)
    {
        _currentPositionIndex = index;

        if (!_gameController.IsBorderCell(_lastPositionIndex, index)) return;

        if (!_gameController.IsMovePlayer(index, _colorSnake)) return;

        _colorSnake = _gameController.SelectSprite(index);

        Move(_gameController.SelectSide(_lastPositionIndex, index));

        _lastPositionIndex = index;
    }

    private void Move(string side)
    {
        if (side == null) return;


        RotateSnake(side); // rotate
        StartCoroutine(MoveSnake(side)); // move animation
    }

    private void RotateSnake(string side)
    {
        Vector3 rotation = new();

        if (side == "t") rotation = new Vector3(0, 0, -180);

        if (side == "b") rotation = new Vector3(0, 0, 0);

        if (side == "l") rotation = new Vector3(0, 0, -90);

        if (side == "r") rotation = new Vector3(0, 0, 90);

        _head.eulerAngles = rotation;
    }

    IEnumerator MoveSnake(string side)
    {
        bool action = true;
        float _currentTimeCurve = 0;

        AnimationCurve _animationCurve = new AnimationCurve();
        Transform target = _gameController.SelectTransform(_currentPositionIndex);

        if (side == "t" || side == "b")
        {
            _animationCurve.AddKey(0, _head.position.y);
            _animationCurve.AddKey(_animationSpeed * 0.5f, Mathf.Lerp(_head.position.y, target.transform.position.y, 0));
            _animationCurve.AddKey(_animationSpeed, target.transform.position.y);
        }

        if (side == "l" || side == "r")
        {
            _animationCurve.AddKey(0, _head.position.x);
            _animationCurve.AddKey(_animationSpeed * 0.5f, Mathf.Lerp(_head.position.x, target.transform.position.x, 0));
            _animationCurve.AddKey(_animationSpeed, target.transform.position.x);
        }

        float _totalTimeCurve = _animationCurve.keys[_animationCurve.keys.Length - 1].time;

        while (action)
        {
            if (side == "t" || side == "b") _head.transform.position = new Vector2(_head.position.x, _animationCurve.Evaluate(_currentTimeCurve));

            if (side == "l" || side == "r") _head.transform.position = new Vector2(_animationCurve.Evaluate(_currentTimeCurve), _head.position.y);

            _currentTimeCurve += Time.deltaTime;
            action = _totalTimeCurve >= _currentTimeCurve;

            yield return null;
        }

        yield break;
    }


}
