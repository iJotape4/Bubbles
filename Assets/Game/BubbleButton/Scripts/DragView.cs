using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityAction onBeginDrag, onDrag, onEndDrag;
    Vector3 offset;
    [HideInInspector] public bool canDrag = true;
    public bool wasMergedInThisMovement =false;
    private void Awake()
    {
        EventManager.AddListener(LevelFlowEvents.LevelEnded, DisableDrag);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener(LevelFlowEvents.LevelEnded, DisableDrag);
    }

    public void DisableDrag()
    {
        canDrag = false;
        onEndDrag?.Invoke();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        onBeginDrag?.Invoke();
        offset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
        offset.z = Camera.main.nearClipPlane; // Ensure there's no change in the Z position
    }
    public void OnDrag(PointerEventData eventData)
    {
        //isDragging = true;
        if (!canDrag) return;
        onDrag?.Invoke();
        transform.position = Camera.main.ScreenToWorldPoint(eventData.position) + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canDrag = true;
        onEndDrag?.Invoke();
        // Optional: Code to execute when dragging ends
        //isDragging = false;
        if (!wasMergedInThisMovement)
        {
            EventManager.Dispatch(InGameEvents.MovementFinished);
            EventManager.Dispatch(SongsEvents.PlaySFX, songs.MoveBubble);
        }
        wasMergedInThisMovement = false;
    }
}
