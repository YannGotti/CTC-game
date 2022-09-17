using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private float _width = 3, _height = 2;
    [SerializeField] public List<GameObject> _cells;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] public List<Sprite> _colors;
    [SerializeField] private Sprite _blueCell;
    [SerializeField] private Sprite _redCell;
    [SerializeField] private Sprite _greenCell;
    [SerializeField] private Sprite _purpleCell;


    private void Start() => GenerateGrid();

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

    public List<GameObject> ReturnSprites() 
    {
        return _cells;
    }





}
