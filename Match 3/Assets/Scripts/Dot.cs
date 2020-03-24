using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {
    public float swiperAngle = 0;

    public int column;
    public int row;
    public int targetX;
    public int targetY;

    public bool isMatched;

    private GameObject otherDot;

    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    private Vector2 temPosition;

    void Start() {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
    }

    void Update() {
        FindMatches();
        if (isMatched) {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color(0.2f, 0.2f, 0.2f);
        }

        targetX = column;
        targetY = row;

        if(Mathf.Abs(targetX - transform.position.x) > .1) {
            // Move towards the target
            temPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, temPosition, .4f);
        }
        else {
            // Directly set the position
            temPosition = new Vector2(targetX, transform.position.y);
            transform.position = temPosition;
            board.allDots[column, row] = this.gameObject;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1) {
            // Move towards the target
            temPosition = new Vector2(transform.position.x, targetY );
            transform.position = Vector2.Lerp(transform.position, temPosition, .4f);
        }
        else {
            // Directly set the position
            temPosition = new Vector2(transform.position.x, targetY);
            transform.position = temPosition;
            board.allDots[column, row] = this.gameObject;
        }
    }

    private void OnMouseDown() {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp() {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle() {
        swiperAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;   
        MovePieces();
    }

    void MovePieces() {
        Debug.Log(swiperAngle);
        if((swiperAngle > -45 && swiperAngle <= 45) && column < board.width) {
            //Right swipe
            Debug.Log("Right swipe");
            otherDot = board.allDots[column + 1, row];
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        } else if ((swiperAngle > 45 && swiperAngle <= 135) && row < board.height) {
            //Up swipe
            Debug.Log("Up swipe");
            otherDot = board.allDots[column, row + 1];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        } else if ((swiperAngle > 135 || swiperAngle <= -135) && column > 0) {
            //Left swipe
            Debug.Log("Left swipe");
            otherDot = board.allDots[column - 1, row];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        } else if ((swiperAngle < -45 && swiperAngle >= -135) && row > 0) {
            //Down swipe
            Debug.Log("Down swipe");
            otherDot = board.allDots[column, row - 1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
    }

    void FindMatches() {
        if(column > 0 && column < board.width - 1) {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if(leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag) {
                leftDot1.GetComponent<Dot>().isMatched = true;
                rightDot1.GetComponent<Dot>().isMatched = true;
                isMatched = true;
            }
        }
        if (row > 0 && row < board.height - 1) {
            GameObject upDot1 = board.allDots[column, row - 1];
            GameObject downDot1 = board.allDots[column, row + 1];
            if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag) {
                upDot1.GetComponent<Dot>().isMatched = true;
                downDot1.GetComponent<Dot>().isMatched = true;
                isMatched = true;
            }
        }
    }
}
