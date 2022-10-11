using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    #region Fields

    private readonly float _width = 3;
    private readonly float _height = 2;
    [Header("Settings cells")]
    public List<GameObject> Сells;
    public List<Sprite> Colors;
    [SerializeField] private GameObject _cellPrefab;

    [Header("Colors cells")]
    [SerializeField] private Sprite _blueCell;
    [SerializeField] private Sprite _redCell;
    [SerializeField] private Sprite _greenCell;
    [SerializeField] private Sprite _purpleCell;
    [SerializeField] private Sprite _comboCell;

    [SerializeField] private float _animationSpeedSecond;

    [Header("Settings game")]
    [SerializeField] private Text _textComponent;
    [SerializeField] private int _steps;
    [SerializeField] private int _stepsSnake;
    private int _indexSelectedCell;

    [Header("Settings sounds")]
    [SerializeField] private AudioSource _audioSource;

    //[Header("Settings particles")]
    //[SerializeField] private GameObject _particleDestroyCell;
    #endregion

    private void Start()
    {
        GenerateGrid();
        FindPath(null);
        _textComponent.text = $"{_steps}";

        EventContoller.singleton.OnSlideCell.AddListener(LostStep);
        EventContoller.singleton.OnSlideCell.AddListener(FindPath);
        EventContoller.singleton.OnGameOver.AddListener(GameOver);
        EventContoller.singleton.AnimationSlideCell.AddListener(AnimationSlideCell);
    }

    #region Grid
    void GenerateGrid()
    {
        Transform parent = GameObject.Find("Cells").transform;
        int rand = Random.Range(-10, 10);


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
                Сells.Add(spawendCell);
                RandomizeSprite(spriteRender, rand);
                spawendCell.name = $"Cell {Сells.Count}";
            }
        }
    }

    void RandomizeSprite(SpriteRenderer spriteRender, int rand)
    {
        var color = Randomize();


        if (Colors.Count == 30 + rand || Colors.Count == 70 + rand) color = _comboCell;

        while (Colors.Count > 9 && color == Colors[Colors.Count - 9] || Colors.Count > 1 && color == Colors[Colors.Count - 1])
            color = Randomize();
            

        Colors.Add(color);
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

    #region Cells
    public void AnimationSlideCell(int indexOwner, int indexTarget, Vector2 lastPosition, int rotateSlide)
    {
        var owner = Сells[indexOwner];
        var target = Сells[indexTarget];

        if (target.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == _comboCell) return;

        Сells[indexOwner] = target;
        Сells[indexTarget] = owner;

        Сells[indexOwner].GetComponent<Cell>().IndexInCellsArray = indexTarget;
        Сells[indexTarget].GetComponent<Cell>().IndexInCellsArray = indexOwner;

        Colors[indexOwner] = target.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Colors[indexTarget] = owner.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        StartCoroutine(SlideCell(owner, target, lastPosition, rotateSlide));
    }

    IEnumerator SlideCell(GameObject owner, GameObject target ,Vector2 lastPosition, int rotateSlide)
    {
        bool action = true;

        float _currentTimeCurve = 0;

        Vector3 ownerPosition = lastPosition;
        Vector3 targetPosition = target.transform.position;

        AnimationCurve _animationCurve = new AnimationCurve();
        AnimationCurve _animationCurveTarget = new AnimationCurve();

        if (rotateSlide == 0)
        {
            _animationCurve.AddKey(0, ownerPosition.x);
            _animationCurve.AddKey(_animationSpeedSecond, targetPosition.x);

            _animationCurveTarget.AddKey(0, targetPosition.x);
            _animationCurveTarget.AddKey(_animationSpeedSecond, lastPosition.x);

        }
        if (rotateSlide == 1)
        {
            _animationCurve.AddKey(0, ownerPosition.y);
            _animationCurve.AddKey(_animationSpeedSecond, targetPosition.y);

            _animationCurveTarget.AddKey(0, targetPosition.y);
            _animationCurveTarget.AddKey(_animationSpeedSecond, lastPosition.y);
        }

        float _totalTimeCurve = _animationCurve.keys[_animationCurve.keys.Length - 1].time;

        while (action)
        {

            if (rotateSlide == 0)
            {
                owner.transform.position = new Vector2(_animationCurve.Evaluate(_currentTimeCurve), owner.transform.position.y);
                target.transform.position = new Vector2(_animationCurveTarget.Evaluate(_currentTimeCurve), owner.transform.position.y);
                
            }

            if (rotateSlide == 1)
            {
                owner.transform.position = new Vector2(owner.transform.position.x, _animationCurve.Evaluate(_currentTimeCurve));
                target.transform.position = new Vector2(owner.transform.position.x, _animationCurveTarget.Evaluate(_currentTimeCurve));
            }

            _currentTimeCurve += Time.deltaTime;
            action = _totalTimeCurve >= _currentTimeCurve;

            yield return null;
        }

        owner.transform.position = targetPosition;
        target.transform.position = ownerPosition;

        _audioSource.Play();
        EventContoller.singleton.OnSlideCell.Invoke(owner);
        yield break;
    }
    public List<GameObject> ReturnSprites() { return Сells; }

    #endregion

    #region AlhorytmFindPath

    private void FindPath(GameObject obj)
    {

    }

    #endregion

    #region SnakeController
    public bool IsMovePlayer(int index, Sprite color)
    {
        _indexSelectedCell = index;

        if (_stepsSnake == 0 && _indexSelectedCell == 9)
        {
            _stepsSnake++;
            return true;
        }

        if (color == null) return false;

        if (color == Colors[index] || color == _comboCell)
        {
            _stepsSnake++;
            return true;
        }

        if (Colors[_indexSelectedCell] == _comboCell)
        {
            EventContoller.singleton.OnComboCell.Invoke();

            _stepsSnake++;
            return true;
        }

        return false;
    }
    
    public Sprite SelectSprite(int index) { return Colors[index]; }

    public Transform SelectTransform(int index) { return Сells[index].transform; }

    public bool IsBorderCell(int lastPosition, int currentPosition)
    {
        if (lastPosition == -1) return true;

        if (lastPosition + 9 == currentPosition || lastPosition - 9 == currentPosition ||
            lastPosition + 1 == currentPosition || lastPosition - 1 == currentPosition) return true;

        return false;
    }

    public string SelectSide(int lastPosition, int currentPosition)
    {
        if (lastPosition == -1) return "t";

        if (lastPosition + 9 == currentPosition) return "r"; //right

        if (lastPosition - 9 == currentPosition) return "l"; //left

        if (lastPosition + 1 == currentPosition) return "t"; //top

        if (lastPosition - 1 == currentPosition) return "b"; //bot

        return null;
    }

    public IEnumerator DestroyCell(int indexCell)
    {
        var cell = Сells[indexCell];

        //Instantiate(_particleDestroyCell, new Vector3(cell.transform.position.x, cell.transform.position.y, -3), Quaternion.identity);

        yield return new WaitForSeconds(1);
        Destroy(cell);
        yield break;
    }

    #endregion

    #region GameOver
    private void LostStep(GameObject obj)
    {
        _textComponent.text = $"{_steps -= 1}";

        if (_steps == 0) EventContoller.singleton.OnGameOver.Invoke();
    }

    private void GameOver() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    #endregion
}