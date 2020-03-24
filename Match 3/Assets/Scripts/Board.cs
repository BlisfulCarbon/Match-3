using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        SetUp();
    }

    private void SetUp(){
        int countLoop = 1;

        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                Debug.Log(countLoop);
                countLoop++;
                GameObject backgroundTile = Instantiate(tilePrefab, new Vector2(i, j), Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + ", " + j + ")";
            }
        }
    }
}
