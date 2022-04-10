using UnityEngine;

public class MoonRotate : MonoBehaviour
{
    public GameObject Moon;
    private Camera _camera;

    private Vector2 firstTouchPrevPos, secondTouchPrevPos;
    private float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

    [SerializeField]
    private float zoomModifierSpeed = 0.1f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            float modifier = 25;

            Moon.transform.Rotate(new Vector3(
                0f,
                -touch.deltaPosition.x / modifier,
                0f
                ));

        }
        else if (Input.touchCount == 2)
        {
            Debug.Log("sHOULD ROTATR");
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
                _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, Mathf.Clamp(_camera.transform.position.z - zoomModifier, -1000, -500));
            if (touchesPrevPosDifference < touchesCurPosDifference)
                _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, Mathf.Clamp(_camera.transform.position.z + zoomModifier, -1000, -500));

        }
    }
}
