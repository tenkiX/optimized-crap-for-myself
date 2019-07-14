using UnityEngine;
using UnityEngine.UI;

public class SwipeLogger : MonoBehaviour
{
    [SerializeField] Text debugText;

    private void Awake()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }

    private void SwipeDetector_OnSwipe(SwipeDirection data)
    {
        debugText.text="Swipe in Direction: " + data + " y" + Random.Range(1,10);
    }
}