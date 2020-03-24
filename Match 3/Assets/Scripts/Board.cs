using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;

    public GameObject[] dots;
    private BackgroundTile[,] allTiles;

    public GameObject[,] allDots;

    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    private void SetUp(){
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                Vector2 columnPosition = new Vector2(i, j);
                string columnName = "(" + i + ", " + j + ")";

                GameObject backgroundTile = Instantiate(tilePrefab, columnPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = columnName;

                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], columnPosition, Quaternion.identity);
                dot.transform.parent = backgroundTile.transform;
                dot.name = columnName;

                allDots[i, j] = dot;
            }
        }
    }
}
