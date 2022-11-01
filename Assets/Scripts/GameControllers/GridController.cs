using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    private readonly static float _width = 3;
    private readonly static float _height = 2;

    #region Grid
    public static void GenerateGrid(List<GameObject> Ñells, List<Sprite> Colors)
    {
        Transform parent = GameObject.Find("Cells").transform;
        int rand = Random.Range(-10, 10);

        var _prefabCell = Resources.Load<GameObject>("Prefabs/Cell");

        for (float x = -2.8f; x < _width; x += 0.7f)
        {
            for (float y = -3.8f; y < _height; y += 0.7f)
            {
                var spawendCell = Instantiate(_prefabCell, new Vector3(x, y), Quaternion.identity, parent);

                var colorCell = new GameObject();
                colorCell.name = $"{(int)x}:{(int)y}";
                colorCell.transform.SetParent(spawendCell.transform);
                colorCell.transform.position = new Vector2(spawendCell.transform.position.x + 0.005f, spawendCell.transform.position.y - 0.04f);
                var spriteRender = colorCell.AddComponent<SpriteRenderer>();

                spriteRender.sortingOrder = 2;
                Ñells.Add(spawendCell);
                RandomizeSprite(spriteRender, rand, Colors);
                spawendCell.name = $"Cell {Ñells.Count}";
            }
        }
    }

    private static void RandomizeSprite(SpriteRenderer spriteRender, int rand, List<Sprite> Colors)
    {
        var color = Randomize();


        if (Colors.Count == 30 + rand || Colors.Count == 70 + rand) color = Resources.Load<Sprite>("Sprites/Squares/Combo");

        while (Colors.Count > 9 && color == Colors[^9] || Colors.Count > 1 && color == Colors[^1])
            color = Randomize();


        Colors.Add(color);
        spriteRender.sprite = color;
    }

    private static Sprite Randomize()
    {
        int rand = Random.Range(0, 4);

        if (rand == 0) return Resources.Load<Sprite>("Sprites/Squares/Blue");
        if (rand == 1) return Resources.Load<Sprite>("Sprites/Squares/Green");
        if (rand == 2) return Resources.Load<Sprite>("Sprites/Squares/Red");
        if (rand == 3) return Resources.Load<Sprite>("Sprites/Squares/Purple");

        return null;
    }

    #endregion
}
