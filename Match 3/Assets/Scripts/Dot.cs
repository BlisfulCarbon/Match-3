using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    public float swiperAngle = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseDown() {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp() {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle() {
        swiperAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - finalTouchPosition.x) * 180 / Mathf.PI;
        Debug.Log(swiperAngle);
    }
}
