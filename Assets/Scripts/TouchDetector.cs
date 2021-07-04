using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchDetector : MonoBehaviour
{
    private Vector2 _fingerUp;
    private Vector2 _fingerDown;
    private Vector2 _swipeDirection = Vector2.zero;
    
    [SerializeField] private bool detectSwipeOnlyAfterRelease = true;
    [SerializeField] private float swipeThreshold = 20f;

    public delegate void OnSwipeHandler(Vector2 swipeDirection, Vector2 swipeStartPosition);
    public static event OnSwipeHandler OnSwipe;
    
    public delegate void OnTouchHandler(Vector2 touchPosition);
    public static event OnTouchHandler OnTouch;
    
    private void Update()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _fingerDown = touch.position;
                _fingerUp = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    _fingerUp = touch.position;
                    CheckSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                _fingerUp = touch.position;
                CheckSwipe();
            }
        }
    }

    void CheckSwipe()
    {
        
        if (CheckDistance() > swipeThreshold)
        {
            _swipeDirection = _fingerUp - _fingerDown;
            OnSwipe?.Invoke(_swipeDirection, _fingerDown);
        }
        else
        {
            OnTouch?.Invoke(_fingerDown);
        }
        
    }

    private float CheckDistance()
    {
        return (_fingerDown - _fingerUp).magnitude;
    }
}