using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    private float _maxMoving = 0.7f;
    private float _maxMovingIfNotBorder = 0.1f;
    private GameObject _selectedCell;
    private int _indexInCellsArray;

    [SerializeField] private GridManager _manager;

    private bool _dragging;

    private Vector2 _lastPosition;


    private void Start() => _manager = GameObject.Find("GridManager").GetComponent<GridManager>();

    private void OnMouseDrag()
    {
        if (!_dragging) return;

        var _inputMouse = Input.mousePosition;
        _inputMouse.z = 9;
        var _mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(_inputMouse);

        transform.position = _mousePosition;

        isMove();
    }

    private void OnMouseDown()
    {
        transform.SetAsLastSibling();
        _lastPosition = transform.position;
        _indexInCellsArray = _manager.ReturnSprites().IndexOf(gameObject);
        _dragging = true;

    } 

    private void OnMouseUp()
    {
        transform.position = _lastPosition;
        _dragging = false;
    }

    private void isMove()
    {
        if (transform.position.x <= _lastPosition.x - _maxMoving)
                transform.position = new Vector2(_lastPosition.x - _maxMoving, _lastPosition.y);

        if (transform.position.x >= _lastPosition.x + _maxMoving)
            transform.position = new Vector2(_lastPosition.x + _maxMoving, _lastPosition.y);

        if (transform.position.y >= _lastPosition.y + _maxMoving)
            transform.position = new Vector2(_lastPosition.x, _lastPosition.y + _maxMoving);

        if (transform.position.y <= _lastPosition.y - _maxMoving)
            transform.position = new Vector2(_lastPosition.x, _lastPosition.y - _maxMoving);



        if (_indexInCellsArray >= 0 && _indexInCellsArray <= 8)
            if (transform.position.x < _lastPosition.x - _maxMovingIfNotBorder)
                transform.position = new Vector2(_lastPosition.x - _maxMovingIfNotBorder, _lastPosition.y);

        if (_indexInCellsArray >= 72 && _indexInCellsArray <= 80)
            if (transform.position.x > _lastPosition.x + _maxMovingIfNotBorder)
                transform.position = new Vector2(_lastPosition.x + _maxMovingIfNotBorder, _lastPosition.y);

        for (int i = 0; i < 80; i += 9)
            if (_indexInCellsArray == i)
                if (transform.position.y <= _lastPosition.y - _maxMovingIfNotBorder)
                    transform.position = new Vector2(_lastPosition.x, _lastPosition.y - _maxMovingIfNotBorder);

        for (int i = 8; i < 81; i += 9)
            if (_indexInCellsArray == i)
                if (transform.position.y >= _lastPosition.y + _maxMovingIfNotBorder)
                    transform.position = new Vector2(_lastPosition.x, _lastPosition.y + _maxMovingIfNotBorder);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Cell") return;
        _selectedCell = collision.gameObject;
    }


}
