using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    #region Поля
    private bool _dragging, _borderUp, _borderDown, _borderLeft, _borderRight;
    private float _maxMoving = 0.7f, _maxMovingIfNotBorder = 0.1f;
    private GameController _manager;
    [SerializeField] private GameObject _selectedCell;
    private int _indexInCellsArray;
    private Vector2 _lastPosition;
    private Transform _parent;
    private int _rotateSlide = -1;
    #endregion

    private void Start()
    {
        _manager = GameObject.Find("GameController").GetComponent<GameController>();
        _parent = transform.parent;
    }

    #region Работа мышью
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
        transform.SetParent(null);
        _lastPosition = transform.position;
        _indexInCellsArray = _manager.ReturnSprites().IndexOf(gameObject);
        GetComponent<SpriteRenderer>().sortingOrder = 2;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
        isBorder();
        _dragging = true;
    }

    private void OnMouseUp()
    {
        transform.position = _lastPosition;
        _borderDown = _borderUp = _borderLeft = _borderRight = false;
        transform.SetParent(_parent);
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
        _dragging = false;

        if (_selectedCell == null) return;

        _manager.OnSlideCell(_indexInCellsArray, _manager.ReturnSprites().IndexOf(_selectedCell), _lastPosition, _rotateSlide);
        _selectedCell = null;
        _rotateSlide = -1;

    }
    #endregion

    #region Проверки на возможность движения
    private void isBorder()
    {
        if (_indexInCellsArray >= 0 && _indexInCellsArray <= 8) _borderLeft = true;

        if (_indexInCellsArray >= 72 && _indexInCellsArray <= 80) _borderRight = true;

        for (int i = 0; i < 80; i += 9) if (_indexInCellsArray == i) _borderUp = true;

        for (int i = 8; i < 81; i += 9) if (_indexInCellsArray == i) _borderDown = true;
    }


    private void isMove()
    {
        if (transform.position.x <= _lastPosition.x - _maxMoving)
        {
            transform.position = new Vector2(_lastPosition.x - _maxMoving, _lastPosition.y);
            _rotateSlide = 0;
        }

        if (transform.position.x >= _lastPosition.x + _maxMoving)
        {
            transform.position = new Vector2(_lastPosition.x + _maxMoving, _lastPosition.y);
            _rotateSlide = 0;
        }

        if (transform.position.y >= _lastPosition.y + _maxMoving)
        {
            transform.position = new Vector2(_lastPosition.x, _lastPosition.y + _maxMoving);
            _rotateSlide = 1;
        }
            

        if (transform.position.y <= _lastPosition.y - _maxMoving)
        {
            transform.position = new Vector2(_lastPosition.x, _lastPosition.y - _maxMoving);
            _rotateSlide = 1;
        }

        
        if (_borderLeft && transform.position.x < _lastPosition.x - _maxMovingIfNotBorder)
            transform.position = new Vector2(_lastPosition.x - _maxMovingIfNotBorder, _lastPosition.y);
        
        if (_borderRight && transform.position.x > _lastPosition.x + _maxMovingIfNotBorder)
            transform.position = new Vector2(_lastPosition.x + _maxMovingIfNotBorder, _lastPosition.y);
        
        if (_borderUp && transform.position.y <= _lastPosition.y - _maxMovingIfNotBorder)
            transform.position = new Vector2(_lastPosition.x, _lastPosition.y - _maxMovingIfNotBorder);

        if (_borderDown && transform.position.y >= _lastPosition.y + _maxMovingIfNotBorder)
            transform.position = new Vector2(_lastPosition.x, _lastPosition.y + _maxMovingIfNotBorder);
    }

    #endregion

    #region Events

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cell") _selectedCell = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cell") _selectedCell = null;
    }

    #endregion

}
