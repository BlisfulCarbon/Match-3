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

    public void RefrashBoard() {

        
        GameObject[,] destroyedDots = allDots;
        BackgroundTile[,] destroyedTiles = allTiles;

        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; i++) {
                Destroy(destroyedDots[j, i]);
                destroyedDots[j, i] = null;

                Destroy(destroyedTiles[j, i]);
                destroyedTiles[j, i] = null;
            }
        }

     
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
                int maxIteerations = 0;
                
                while (MatchesAt(i, j, dots[dotToUse]) && maxIteerations < 100) {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIteerations++;
                }
                if (maxIteerations == 100) Debug.LogWarning("Board -> SetUp: generation board maxIteeratinons get max value" );
                maxIteerations = 0;
          
                GameObject dot = Instantiate(dots[dotToUse], columnPosition, Quaternion.identity);
                dot.transform.parent = backgroundTile.transform;
                dot.name = columnName;

                allDots[i, j] = dot;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece) {
        if(column > 1 && row > 1) {
            if(allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag){
                return true;
            }
            if(allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) {
                return true;
            }
        }else if(column <= 1 || row <= 1) {
            if(row > 1) {
                if(allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) {
                    return true;
                }
            }
            if (column > 1) {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag) {
                    return true;
                }
            }
        }
        return false;
    }

    private void DestroyMatchesAt(int column, int row) {
        if(allDots[column, row].GetComponent<Dot>().isMatched) {
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }
    
    public void DestroyMatches() {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) { 
                if(allDots[i, j] != null) {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo() {
        int nullCount = 0;
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i,j] == null) { 
                    nullCount++;
                } else if (nullCount > 0) {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard() {
       for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if(allDots[i, j] == null) {
                    Vector2 tempPosition = new Vector2(i, j);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                }
            }
        }
    }

    private bool MatchesOnBoard() {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if(allDots[i, j] != null) {
                    if(allDots[i, j].GetComponent<Dot>().isMatched) {
                        return true;
                    }
                }
            }
        }
        
        return false;
    }

    private IEnumerator FillBoardCo() {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard()) {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
    }
}
