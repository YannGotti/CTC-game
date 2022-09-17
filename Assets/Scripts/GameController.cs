using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Поля
    private float _width = 3, _height = 2, _maxMoving = 0.6f;
    [SerializeField] public List<GameObject> _cells;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] public List<Sprite> _colors;
    [SerializeField] private Sprite _blueCell;
    [SerializeField] private Sprite _redCell;
    [SerializeField] private Sprite _greenCell;
    [SerializeField] private Sprite _purpleCell;

    [SerializeField] private float _animationSpeedSecond;

    #endregion

    private void Start() => GenerateGrid();

    #region Сетка

    void GenerateGrid()
    {
        Transform parent = GameObject.Find("Cells").transform;

        for (float x = -2.8f; x < _width; x += 0.7f)
        {
            for (float y = -3.8f; y < _height; y += 0.7f)
            {
                var spawendCell = Instantiate(_cellPrefab, new Vector3(x,y), Quaternion.identity, parent);
                
                var colorCell = new GameObject();
                colorCell.name = $"{(int)x}:{(int)y}";
                colorCell.transform.SetParent(spawendCell.transform);
                colorCell.transform.position = new Vector2(spawendCell.transform.position.x + 0.005f, spawendCell.transform.position.y - 0.04f);
                var spriteRender = colorCell.AddComponent<SpriteRenderer>();

                spriteRender.sortingOrder = 2;
                _cells.Add(spawendCell);
                RandomizeSprite(spriteRender);
                spawendCell.name = $"Cell {_cells.Count}";
            }
        }
    }


    void RandomizeSprite(SpriteRenderer spriteRender)
    {
        var color = Randomize();

        while (_colors.Count > 9 && color == _colors[_colors.Count - 9] || _colors.Count > 1 && color == _colors[_colors.Count - 1])
            color = Randomize();

        _colors.Add(color);
        spriteRender.sprite = color;
    }

    Sprite Randomize()
    {
        int rand = Random.Range(0, 4);

        if (rand == 0) return _blueCell;
        if (rand == 1) return _greenCell;
        if (rand == 2) return _redCell;
        if (rand == 3) return _purpleCell;

        return null;
    }

    #endregion

    #region Перемещение фишек
    public void OnSlideCell(int indexOwner, int indexTarget, Vector2 lastPosition, int rotateSlide)
    {
        var owner = _cells[indexOwner];
        var target = _cells[indexTarget];

        _cells[indexOwner] = target;
        _cells[indexTarget] = owner;

        StartCoroutine(SlideCell(owner, target, lastPosition, rotateSlide));
    }

    IEnumerator SlideCell(GameObject owner, GameObject target ,Vector2 lastPosition, int rotateSlide)
    {
        bool action = true;

        float _currentTimeCurve = 0;

        AnimationCurve _animationCurve = new AnimationCurve();
        AnimationCurve _animationCurveTarget = new AnimationCurve();

        if (rotateSlide == 0)
        {
            _animationCurve.AddKey(0, lastPosition.x);
            _animationCurve.AddKey(_animationSpeedSecond, target.transform.position.x);
            _animationCurveTarget.AddKey(0, target.transform.position.x);
            _animationCurveTarget.AddKey(_animationSpeedSecond, lastPosition.x);

        }
        else
        {
            _animationCurve.AddKey(0, lastPosition.y);
            _animationCurve.AddKey(_animationSpeedSecond, target.transform.position.y);
            _animationCurveTarget.AddKey(0, target.transform.position.y);
            _animationCurveTarget.AddKey(_animationSpeedSecond, lastPosition.y);
        }

        float _totalTimeCurve = _animationCurve.keys[_animationCurve.keys.Length - 1].time;

        while (action)
        {
            switch (rotateSlide)
            {
                case 0:
                    owner.transform.position = new Vector2(_animationCurve.Evaluate(_currentTimeCurve), owner.transform.position.y);
                    target.transform.position = new Vector2(_animationCurveTarget.Evaluate(_currentTimeCurve), owner.transform.position.y);
                    _currentTimeCurve += Time.deltaTime;
                    break;
                case 1:
                    owner.transform.position = new Vector2(owner.transform.position.x, _animationCurve.Evaluate(_currentTimeCurve));
                    target.transform.position = new Vector2(owner.transform.position.x, _animationCurveTarget.Evaluate(_currentTimeCurve));
                    _currentTimeCurve += Time.deltaTime;
                    break;
            }

            action = _totalTimeCurve >= _currentTimeCurve;

            yield return null;
        }

        _animationCurve = new AnimationCurve();
        _animationCurveTarget = new AnimationCurve();
        yield break;
    }
    


    #endregion

    public List<GameObject> ReturnSprites()
    {
        return _cells;
    }


}
