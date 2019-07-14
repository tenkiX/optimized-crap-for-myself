using System;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    [SerializeField] private float _minDistanceForSwipe = 80f;
    private List<Finger> _touchingFingers = new List<Finger>();

    private class Finger
    {
        public int fingerId;
        public Vector2 fingerDownPosition;
        public Vector2 fingerUpPosition;
    }

    public static event Action<SwipeDirection> OnSwipe = delegate { };

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Finger newTouch = new Finger
                {
                    fingerId = touch.fingerId,
                    fingerDownPosition = touch.position
                };
                _touchingFingers.Add(newTouch);
            }
            Finger current = _touchingFingers.Find(x => x.fingerId == touch.fingerId);
            if (current != null) current.fingerUpPosition = touch.position;
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended)
            {
                DetectSwipe();
                Finger current = _touchingFingers.Find(x => x.fingerId == touch.fingerId);
                _touchingFingers.Remove(current);
            }
        }
    }

    private void DetectSwipe()
    {
        if (_touchingFingers.Count != 3) return;
        int swipesToLeft = 0, swipesToRight = 0;

        foreach (Finger finger in _touchingFingers)
        {
            if (IsSwipeDistancePassedMinDistance(finger) && !IsVerticalSwipe(finger))
            {
                SwipeDirection swipeDirection = finger.fingerUpPosition.x - finger.fingerDownPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;

                if (swipeDirection == SwipeDirection.Right) { swipesToRight++; } else { swipesToLeft++; };

                if (swipesToRight == 3) OnSwipe(SwipeDirection.Right);
                if (swipesToLeft == 3) OnSwipe(SwipeDirection.Left);
            }
        }
    }

    private bool IsVerticalSwipe(Finger finger)
    {
        return Mathf.Abs(finger.fingerDownPosition.y - finger.fingerUpPosition.y) > Mathf.Abs(finger.fingerDownPosition.x - finger.fingerUpPosition.x);
    }

    private bool IsSwipeDistancePassedMinDistance(Finger finger)
    {
        return Mathf.Abs(finger.fingerDownPosition.x - finger.fingerUpPosition.x) > _minDistanceForSwipe;
    }
}

public enum SwipeDirection
{
    Left,
    Right
}