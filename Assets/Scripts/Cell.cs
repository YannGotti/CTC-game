using UnityEngine;

public class Cell : MonoBehaviour
{

    private float _maxMoving = 0.7f;
    private bool _dragging, _onlyX, _onlyY;

    private Vector2 _lastPosition;

    private void OnMouseDrag()
    {
        if (!_dragging) return;

        var _inputMouse = Input.mousePosition;
        _inputMouse.z = 9;
        var _mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(_inputMouse);

        if(_onlyX)
            transform.position = new Vector2(_mousePosition.x, transform.position.y);

        if (_onlyY)
            transform.position = new Vector2(transform.position.x, _mousePosition.y);

        isMove();
    }

    private void OnMouseDown()
    {
        transform.SetAsLastSibling();
        _lastPosition = transform.position;
        _dragging = true;
        
    } 

    private void OnMouseUp()
    {
        transform.position = _lastPosition;
        _dragging = false;
    }
    
    private void isMove()
    {
        if (transform.position.x >= _lastPosition.x + _maxMoving)
            transform.position = new Vector2(_lastPosition.x + _maxMoving, _lastPosition.y);

        if (transform.position.x <= _lastPosition.x - _maxMoving)
            transform.position = new Vector2(_lastPosition.x - _maxMoving, _lastPosition.y);


    }



}
