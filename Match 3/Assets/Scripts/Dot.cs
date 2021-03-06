﻿using System.Collections;
using UnityEngine;

public class Dot : MonoBehaviour {

    [Header("Board variables")]
    public int column;
    public int row;
    public int targetX;
    public int targetY;

    public int previousColumn;
    public int previousRow;
    public float swiperAngle = 0;
    public bool isMatched;
    public float swipeResist = 1f;

    private FindMatches findMatches;
    private GameObject otherDot;
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 temPosition;

    void Start() {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
    }

    void Update() {
        if (isMatched) {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color(0.2f, 0.2f, 0.2f);
        }

        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .6f) {
            // Move towards the target
            temPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, temPosition, .1f);

            if (board.allDots[column, row] != this.gameObject) {
                board.allDots[column, row] = this.gameObject;
            }

            findMatches.FindAllMatches();
        }
        else {
            // Directly set the position
            temPosition = new Vector2(targetX, transform.position.y);
            transform.position = temPosition;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .6f) {
            // Move towards the target
            temPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, temPosition, .1f);

            if (board.allDots[column, row] != this.gameObject) {
                board.allDots[column, row] = this.gameObject;
            }

           findMatches.FindAllMatches();
        }
        else {
            // Directly set the position
            temPosition = new Vector2(transform.position.x, targetY);
            transform.position = temPosition;
            board.allDots[column, row] = this.gameObject;
        }
    }

    public IEnumerator CheckMoveCo() {
        yield return new WaitForSeconds(.2f);
        if (otherDot != null) {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched) {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;

                yield return new WaitForSeconds(.5f);
                board.currentState = GameState.move;
            }
            else {
                board.DestroyMatches();
            }
            otherDot = null;
        }
    }

    private void OnMouseDown() {
        if(board.currentState == GameState.move) { 
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp() {
        if (board.currentState == GameState.move) {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle() {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist) {
            swiperAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            board.currentState = GameState.wait;
        } else {
            board.currentState = GameState.move;
        }
    }

    void MovePieces() {
        if ((swiperAngle > -45 && swiperAngle <= 45) && column < board.width - 1) {
            //Right swipe
            Debug.Log("Right swipe");
            otherDot = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if ((swiperAngle > 45 && swiperAngle <= 135) && row < board.height - 1) {
            //Up swipe
            Debug.Log("Up swipe");
            otherDot = board.allDots[column, row + 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if ((swiperAngle > 135 || swiperAngle <= -135) && column > 0) {
            //Left swipe
            Debug.Log("Left swipe column: " + column + "   " + this.name);
            otherDot = board.allDots[column - 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else if ((swiperAngle < -45 && swiperAngle >= -135) && row > 0) {
            //Down swipe
            Debug.Log("Down swipe");
            otherDot = board.allDots[column, row - 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }
}
