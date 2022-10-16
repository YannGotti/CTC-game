using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject _selectedCell;
    public int IndexInCellsArray;
    private bool _dragging, _borderUp, _borderDown, _borderLeft, _borderRight, _isMove;
    private readonly float _maxMoving = 0.7f;
    private readonly float _maxMovingIfNotBorder = 0.1f;
    private GameController _gameController;
    private Vector2 _lastPosition;
    private Transform _parent;
    private int _rotateSlide = -1;


    private void Start() { _gameController = GameObject.Find("GameController").GetComponent<GameController>(); _parent = transform.parent; }

    private void OnMouseDrag()
    {
        if (!_dragging) return;

        var _inputMouse = Input.mousePosition;
        _inputMouse.z = 9;
        var _mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(_inputMouse);

        transform.position = _mousePosition;

        MoveMouseCell();
        _isMove = IsMove();
    }

    private void OnMouseDown()
    {

        if (transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("Sprites/Squares/Combo"))
            return;

        Cursor.visible = false;
        transform.SetParent(null);
        _lastPosition = transform.position;
        IndexInCellsArray = _gameController.ReturnSprites().IndexOf(gameObject);
        GetComponent<SpriteRenderer>().sortingOrder = 2;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
        IsBorder();
        _dragging = true;
    }

    private void OnMouseUp()
    {
        if (transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("Sprites/Squares/Combo"))
            return;

        Cursor.visible = true;
        transform.position = _lastPosition;
        _borderDown = _borderUp = _borderLeft = _borderRight = false;
        transform.SetParent(_parent);
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
        _dragging = false;

        if (_selectedCell == null) return;
        if (!_isMove) return;

        EventContoller.singleton.AnimationSlideCell.Invoke(IndexInCellsArray, _gameController.ReturnSprites().IndexOf(_selectedCell), _lastPosition, _rotateSlide);

        _selectedCell = null;
        _rotateSlide = -1;
    }

    private void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        EventContoller.singleton.OnDownCell.Invoke(gameObject, _gameController.ReturnSprites().IndexOf(gameObject));
    }


    private void IsBorder()
    {
        if (IndexInCellsArray >= 0 && IndexInCellsArray <= 8) _borderLeft = true;

        if (IndexInCellsArray >= 72 && IndexInCellsArray <= 80) _borderRight = true;

        for (int i = 0; i < 80; i += 9) if (IndexInCellsArray == i) _borderUp = true;

        for (int i = 8; i < 81; i += 9) if (IndexInCellsArray == i) _borderDown = true;
    }


    private void MoveMouseCell()
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

    private bool IsMove()
    {
        if (transform.position == new Vector3(_lastPosition.x - _maxMoving, _lastPosition.y))
            return true;

        if (transform.position == new Vector3(_lastPosition.x + _maxMoving, _lastPosition.y))
            return true;

        if (transform.position == new Vector3(_lastPosition.x, _lastPosition.y + _maxMoving))
            return true;

        if (transform.position == new Vector3(_lastPosition.x, _lastPosition.y - _maxMoving))
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision) { if (collision.gameObject.tag == "Cell") _selectedCell = collision.gameObject; }

    private void OnTriggerExit2D(Collider2D collision) { if (collision.gameObject.tag == "Cell") _selectedCell = null; }


}
