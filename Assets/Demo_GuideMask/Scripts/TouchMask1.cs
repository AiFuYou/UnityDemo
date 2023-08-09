using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchMask1 : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }
    
    private void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        var current = data.pointerCurrentRaycast.gameObject;
        Debug.Log($"射线检测穿透\t{current.name}");
        for (int i = 0; i < results.Count; i++)
        {
            //判断穿透对象是否是需要要点击的对象
            if (current != results[i].gameObject)
            {
                if (results[i].gameObject.GetComponent<Button>() != null)
                {
                    Debug.Log($"找到目标\t{results[i].gameObject.name}");
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                    return;
                }
            }
        }
    }
}
