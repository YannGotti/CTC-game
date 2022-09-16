using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private float _width = 3, _height = 2;

    [SerializeField] private List<Sprite> _cells;

    [SerializeField] private Cell _cellPrefab;

    [SerializeField] private Sprite _blueCell;
    [SerializeField] private Sprite _redCell;
    [SerializeField] private Sprite _greenCell;
    [SerializeField] private Sprite _purpleCell;



    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        Transform parent = GameObject.Find("Cells").transform;

        for (float x = -2.8f; x < _width; x += 0.7f)
        {
            for (float y = -3.8f; y < _height; y += 0.7f)
            {
                var spawendCell = Instantiate(_cellPrefab, new Vector3(x,y), Quaternion.identity, parent);
                spawendCell.name = $"Cell";
                var colorCell = new GameObject();
                colorCell.name = $"{(int)x}:{(int)y}";
                colorCell.transform.SetParent(spawendCell.transform);
                colorCell.transform.position = new Vector2(spawendCell.transform.position.x + 0.005f, spawendCell.transform.position.y - 0.04f);
                var spriteRender = colorCell.AddComponent<SpriteRenderer>();

                RandomizeSprite(spriteRender);
                spriteRender.sortingOrder = 2;

                while (_cells.Count > 0 && _cells[_cells.Count - 1] == spriteRender.sprite)
                    RandomizeSprite(spriteRender);

                while (_cells.Count >= 9 && _cells[_cells.Count - 9] == spriteRender.sprite)
                    RandomizeSprite(spriteRender);

                _cells.Add(spriteRender.sprite);
            }
        }
    }

    void RandomizeSprite(SpriteRenderer spriteRender)
    {
        int rand = Random.Range(0, 4);


        if (rand == 0) spriteRender.sprite = _blueCell;
        if (rand == 1) spriteRender.sprite = _greenCell;
        if (rand == 2) spriteRender.sprite = _redCell;
        if (rand == 3) spriteRender.sprite = _purpleCell;
    }

}
