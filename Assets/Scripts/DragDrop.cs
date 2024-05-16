using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class DragDrop : NetworkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PlayerManager playerManager;
    private CardManager handManager;
    private GameObject canvas;
    private bool isOverDropZone = false;
    private bool isDraggable = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("DropZone"))
        {
            isOverDropZone = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("DropZone"))
        {
            isOverDropZone = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) { return; }

        if (transform.parent.CompareTag("DropZone"))
        {
            return;
        }

        handManager.RemoveCard(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent.CompareTag("DropZone"))
        {
            return;
        }

        transform.position = eventData.position;
        transform.SetParent(canvas.transform, true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) { return; }

        if (transform.parent.CompareTag("DropZone"))
        {
            return;
        }

        if (isOverDropZone)
        {
            isDraggable = false;
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
            playerManager.PlayCard(gameObject);
        }
        else
        {
            handManager.AddCard(gameObject, eventData.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Main Canvas");
        handManager = GameObject.Find("PlayerArea").GetComponent<CardManager>();

        if (isOwned)
        {
            isDraggable = true;
        }
    }
}
