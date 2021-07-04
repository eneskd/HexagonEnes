using System;
using System.Collections;
using System.Collections.Generic;
using HexagonalGrid;
using TMPro;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void OnScreenClickHandler(Vector3 position);

    public static event OnScreenClickHandler OnScreenClick;

    public delegate void OnRotateCommandHandler(RotationDirection direction);

    public static event OnRotateCommandHandler OnRotateCommand;

    public static PlayerInput I => _i;
    private static PlayerInput _i;

    private void Awake()
    {
        if (_i != null && _i != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _i = this;
        }

        TouchDetector.OnSwipe += SwipeAction;
        TouchDetector.OnTouch += TouchAction;
    }

    private void Update()
    {
        
#if UNITY_STANDALONE_WIN
        
        if (GameLoop.CurrentState != GameState.WaitingInput) return;
        if (Input.GetButtonDown("Fire1"))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnScreenClick?.Invoke(mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnRotateCommand?.Invoke(RotationDirection.CW);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnRotateCommand?.Invoke(RotationDirection.CCW);
        }
#endif
        
        
    }

    private void TouchAction(Vector2 touchPosition)
    {
        if(GameLoop.CurrentState != GameState.WaitingInput) return;
        var selectPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        OnScreenClick?.Invoke(selectPosition);
    }

    private void SwipeAction(Vector2 swipeDirection, Vector2 swipeStartPosition)
    {
        var startPosition = Camera.main.ScreenToWorldPoint(swipeStartPosition);
        if (SelectionManager.isSelectionActive)
        {
            var selectionPosition = SelectionManager.Selection.transform.position;
            var difference = startPosition - selectionPosition;
            //if (difference.x < 0)
            //{
            //    difference = new Vector2(difference.x, -difference.y);
            //}
//



            var directionAngle = Mathf.Atan2(swipeDirection.y, swipeDirection.x);
            var differenceAngle =  Mathf.Atan2(difference.y, difference.x);
            var angelBetween = directionAngle - differenceAngle;
            Debug.Log($"difference angle {differenceAngle}");
            Debug.Log(startPosition);

            OnRotateCommand?.Invoke(Mathf.Sin(angelBetween) >= 0
                ? RotationDirection.CCW
                : RotationDirection.CW);
        }
    }




}