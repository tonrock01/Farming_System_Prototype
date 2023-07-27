using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;
    public float panSpeed = 20f;
    public float zoomSpeed = 20f;
    public float zoomSensitivity = 2f;

    public float maxZoom = 5f;
    public float minZoom = 20f;

    public float maxPanDistance = 10f;
    public float minPanDistance = 2f;

    private Vector3 lastPanPosition;
    private int panFingerId;

    public bool canPan = true;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (canPan)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastPanPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - lastPanPosition;
                PanCamera(delta);
                lastPanPosition = Input.mousePosition;
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll);

        if (Input.touchSupported && canPan)
        {
            HandleTouch();
        }
    }

    void PanCamera(Vector3 delta)
    {
        Vector3 panDelta =
            new Vector3(-delta.x / Screen.width, -delta.y / Screen.height, 0f) * panSpeed;
        Vector3 newPosition = transform.position + panDelta;

        newPosition.x = Mathf.Clamp(newPosition.x, -maxPanDistance, maxPanDistance);
        newPosition.y = Mathf.Clamp(newPosition.y, -maxPanDistance, maxPanDistance);

        transform.position = newPosition;
    }

    void ZoomCamera(float scroll)
    {
        float zoomDelta = scroll * zoomSpeed * zoomSensitivity;
        float currentZoom = Camera.main.orthographicSize;
        float newZoom = currentZoom - zoomDelta;

        newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

        Camera.main.orthographicSize = newZoom;
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {
            case 1:
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.phase == TouchPhase.Moved && touch.fingerId == panFingerId)
                {
                    Vector3 touchPos = new Vector3(touch.position.x, touch.position.y, 0f);
                    Vector3 lastPos = new Vector3(lastPanPosition.x, lastPanPosition.y, 0f);
                    Vector3 delta = touchPos - lastPos;
                    PanCamera(delta);
                    lastPanPosition = touch.position;
                }
                break;

            case 2:
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                ZoomCamera(deltaMagnitudeDiff * 0.01f * zoomSpeed * zoomSensitivity);
                break;
        }
    }
}
