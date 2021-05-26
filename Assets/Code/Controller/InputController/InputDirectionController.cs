using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDirectionController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event System.Action<Vector2> playerDirectionController;

    private IEnumerator Start()
    {
        yield return null;
        playerDirectionController?.Invoke(PlayerController.Instance.transform.forward);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta;
        playerDirectionController?.Invoke(delta);
        //Debug.Log($"Mouse Drag: {delta}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
