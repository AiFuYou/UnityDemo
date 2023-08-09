using UnityEngine;

public class TouchMask2 : MonoBehaviour, ICanvasRaycastFilter
{
    public RectTransform areaTouch;
    
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (areaTouch != null && RectTransformUtility.RectangleContainsScreenPoint(areaTouch, sp, eventCamera))
        {
            return false;
        }
        return true;
    }
}
