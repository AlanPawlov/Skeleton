using UnityEngine;
using UnityEngine.EventSystems;

public class DragHelper : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public IDraggable Listener { get; set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        Listener.OnPointerDown(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Listener.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Listener.OnPointerUp(eventData);
    }
}
