using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private float Treshold = 150;
    private bool hasMoved = false;

    public Vector3 startPos, currentPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Handles movement by checking if current Touched Position is greater/smaller than the
        // start Touched Position +/- treshhold on all 4 axis

        if (hasMoved) return;

        currentPos = eventData.position;


        if (currentPos.y > startPos.y + Treshold) { MovePlayerUp(); hasMoved = true; }

        if (currentPos.y < startPos.y - Treshold) { MovePlayerDown(); hasMoved = true; }

        if (currentPos.x > startPos.x + Treshold) { MovePlayerRight(); hasMoved = true; }

        if (currentPos.x < startPos.x - Treshold) { MovePlayerLeft(); hasMoved = true; }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        hasMoved = false;
    }



    public static void MovePlayerUp()
    {
        Debug.Log("Move Up");
        OnMoveUp();
    }
    public static void MovePlayerDown()
    {
        Debug.Log("Move Down");
        OnMoveDown();
    }
    public static void MovePlayerLeft()
    {
        Debug.Log("Move Left");
        OnMoveLeft();
    }
    public static void MovePlayerRight()
    {
        Debug.Log("Move Right");
        OnMoveRight();
    }


    //Events for each Move

    public delegate void MoveUp();
    public static event MoveUp OnMoveUp;

    public delegate void MoveDown();
    public static event MoveDown OnMoveDown;

    public delegate void MoveRight();
    public static event MoveRight OnMoveRight;

    public delegate void MoveLeft();
    public static event MoveLeft OnMoveLeft;
}
