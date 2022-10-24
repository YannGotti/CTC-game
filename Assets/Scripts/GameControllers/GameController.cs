using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Fields
    
    [Header("Settings cells")]
    public List<GameObject> Cells;
    public List<Sprite> Colors;
    private static float _animationSpeedSecond = 0.2f;


    [Header("Settings game")]
    [SerializeField] private Text _textComponent;
    [SerializeField] private int _steps;
    [SerializeField] private int _stepsSnake;
    [SerializeField] private bool _gameStarted;

    [Header("Settings sounds")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Settings score")]
    [SerializeField] private float _maxMoneyDrop;
    [SerializeField] private float _maxScoreDrop;
    [SerializeField] private float _maxStep;
    [SerializeField] private float _maxTime;

    [Header("Settings stats panel")]
    [SerializeField] private GameObject _statsPanel;
    [SerializeField] private TMPro.TMP_Text _moneyStatText;
    [SerializeField] private TMPro.TMP_Text _stepsStatText;
    [SerializeField] private TMPro.TMP_Text _timeStatText;


    private readonly float _width = 3;
    private readonly float _height = 2;

    private int _indexSelectedCell;


    private MySqlConnector _mySqlConnector;
    private float _timeGame;
    private float _stepCount;
    private int _moneyDrop;


    //[Header("Settings particles")]
    //[SerializeField] private GameObject _particleDestroyCell;
    #endregion

    private void Start()
    {
        GridController.GenerateGrid(_width, _height, Cells, Colors);
        FindPath(null);
        _textComponent.text = $"{_steps}";

        _mySqlConnector = GetComponent<MySqlConnector>();
        EventContoller.singleton.OnSlideCell.AddListener(LostStep);
        EventContoller.singleton.StartGame.AddListener(StartGame);
        EventContoller.singleton.OnSlideCell.AddListener(FindPath);
        EventContoller.singleton.OnGameOver.AddListener(MathMoneyAndScore);
        EventContoller.singleton.OnGameOver.AddListener(GameOver);

        EventContoller.singleton.AnimationSlideCell.AddListener(AnimationSlideCell);
    }

    private void FixedUpdate()
    {
        if (_gameStarted) _timeGame += Time.fixedDeltaTime;
    }

    #region Cells

    public void AnimationSlideCell(int indexOwner, int indexTarget, Vector2 lastPosition, int rotateSlide)
    {
        var owner = Cells[indexOwner];
        var target = Cells[indexTarget];

        if (target.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("Sprites/Squares/Combo")) return;

        Cells[indexOwner] = target;
        Cells[indexTarget] = owner;

        Cells[indexOwner].GetComponent<Cell>().IndexInCellsArray = indexTarget;
        Cells[indexTarget].GetComponent<Cell>().IndexInCellsArray = indexOwner;

        Colors[indexOwner] = target.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Colors[indexTarget] = owner.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        EventContoller.singleton.StartGame.Invoke();

        StartCoroutine(SlideCell(owner, target, lastPosition, rotateSlide));
    }

    private IEnumerator SlideCell(GameObject owner, GameObject target, Vector2 lastPosition, int rotateSlide)
    {
        bool action = true;

        float _currentTimeCurve = 0;

        Vector3 ownerPosition = lastPosition;
        Vector3 targetPosition = target.transform.position;

        AnimationCurve _animationCurve = new();
        AnimationCurve _animationCurveTarget = new();

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

        float _totalTimeCurve = _animationCurve.keys[^1].time;

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

    public List<GameObject> ReturnSprites() { return Cells; }

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

        if (_stepsSnake == 0 && _indexSelectedCell == 0)
        {
            _stepsSnake++;
            return true;
        }

        if (color == null) return false;

        if (color == Colors[index] || color == Resources.Load<Sprite>("Sprites/Squares/Combo"))
        {
            _stepsSnake++;
            return true;
        }

        if (Colors[_indexSelectedCell] == Resources.Load<Sprite>("Sprites/Squares/Combo"))
        {
            EventContoller.singleton.OnComboCell.Invoke();

            _stepsSnake++;
            return true;
        }

        return false;
    }
    
    public Sprite SelectSprite(int index) { return Colors[index]; }

    public Transform SelectTransform(int index) { return Cells[index].transform; }

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
        var cell = Cells[indexCell];

        //Instantiate(_particleDestroyCell, new Vector3(cell.transform.position.x, cell.transform.position.y, -3), Quaternion.identity);

        yield return new WaitForSeconds(1);
        Destroy(cell);
        yield break;
    }

    public bool IsLastCell(int currentPosition)
    {
        if (currentPosition + 1 == Cells.Count) return true;

        return false;
    }

    #endregion

    #region GameOver
    private void LostStep(GameObject obj)
    {
        _stepCount++;
        _textComponent.text = $"{_steps -= 1}";

        if (_steps == 0) EventContoller.singleton.OnGameOver.Invoke();
    }

    public void StatsEnable()
    {
        _moneyStatText.text = $"Получено {(int)_moneyDrop} монет!";
        _stepsStatText.text = $"Пройдено за  {(int)_stepCount} шагов!";
        _timeStatText.text = $"Пройдено за  {(int)_timeGame} секунд!";
        _statsPanel.SetActive(true);
    } 
    public void StatsDisable() => _statsPanel.SetActive(false);

    private void MathMoneyAndScore()
    {
        float percentSteps = (_stepCount / _maxStep) * 50;
        float percentTime = (_timeGame / _maxTime) * 50;

        MathMoney(percentSteps, percentTime);
        MathScore(percentSteps, percentTime);
        _mySqlConnector.UpdateStepsAndTimeUser((int)_stepCount, (int)_timeGame);
    }

    private void MathMoney(float percentSteps, float percentTime)
    {
        float moneyDrop =  (((50 - percentSteps) + (50 - percentTime)) * _maxMoneyDrop) / 100;

        if (moneyDrop <= 0) moneyDrop = 0;

        _mySqlConnector.UpdateMoneyUser(_moneyDrop = (int)moneyDrop);
    }

    private void MathScore(float percentSteps, float percentTime)
    {
        float score =  (((50 - percentSteps) + (50 - percentTime)) * _maxScoreDrop) / 100;

        if (score <= 0) score = 0;


        _mySqlConnector.UpdateScoreUser((int)score);
    }

    private void StartGame()
    {
        if (_gameStarted) return;

        _gameStarted = true;
    }

    private void GameOver()
    {
        _gameStarted = false;
        StatsEnable();
    }

    public void GameReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuOpen() => SceneManager.LoadScene(0);
    #endregion
}
