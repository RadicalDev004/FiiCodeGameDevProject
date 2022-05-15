using UnityEngine;
using UnityEngine.EventSystems;

public class MenuBackgroundScroll : MonoBehaviour, IPointerDownHandler
{
    public RectTransform Background;
    public Vector2 startPos, currentPos;
    public float left, right, up, down;

    public float PosModifier = 5, zoomSpeed = 0.1f;

    private Vector2 firstTouchPrevPos, secondTouchPrevPos;
    private float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

    public static int PauseScroll;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        PauseScroll = 0;
        if (!PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetInt("Level", 1);
        }

        left = Background.sizeDelta.x / 2 - 960;
        right = -left;

        down = Background.sizeDelta.y / 2 - 540;
        up = -down;
    }

    private void Update()
    {
        if (PauseScroll == 1) return;
        Debug.Log(PauseScroll);

        left = Background.sizeDelta.x / 2 - 960;
        right = -left;

        down = Background.sizeDelta.y / 2 - 540;
        up = -down;


        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            Background.anchoredPosition += touch.deltaPosition/PosModifier;

            Background.anchoredPosition = new Vector2(Mathf.Clamp(Background.anchoredPosition.x, right, left), Mathf.Clamp(Background.anchoredPosition.y, up , down)); 
        }
        if(Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;


            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude / zoomSpeed;


            if (touchesPrevPosDifference > touchesCurPosDifference) //smaller
            {
                Background.sizeDelta -= new Vector2(zoomModifier,zoomModifier);
            }
            if (touchesPrevPosDifference < touchesCurPosDifference) //bigger
            {
                Background.sizeDelta += new Vector2(zoomModifier, zoomModifier);
            }

            Background.sizeDelta = new Vector2(Mathf.Clamp(Background.sizeDelta.x, 2000, 5000), Mathf.Clamp(Background.sizeDelta.y, 2000, 5000));
        }
    }

    private bool CheckBorder()
    {
        if (Background.anchoredPosition.x <= right && currentPos.x < startPos.x)
        {
            Debugg(Background.anchoredPosition.x, right, currentPos.x, startPos.x);
            return false;
        }
        if (Background.anchoredPosition.x >= left && currentPos.x > startPos.x)
        {
            Debugg(Background.anchoredPosition.x, left, currentPos.x, startPos.x);
            return false;
        }


        if (Background.anchoredPosition.y <= up && currentPos.y < startPos.y)
        {
            Debugg(Background.anchoredPosition.y, up, currentPos.y, startPos.y);
            return false;
        }
        if (Background.anchoredPosition.y >= down && currentPos.y > startPos.y)
        {
            Debugg(Background.anchoredPosition.y, down, currentPos.y, startPos.y);
            return false;
        }

        return true;
    }
    private void Debugg(object o1, object o2, object o3, object o4)
    {
        Debug.Log(o1 + " " + o2 + " " + o3 + " " + o4);
    }

    private void SnapToPos(float posX, float posY)
    {

    }


    public void CancelMapScroll()
    {
        PauseScroll = 1;
    }
    public void EnableMapScroll()
    {
        PauseScroll = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EnableMapScroll();
    }
}
