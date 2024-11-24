using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float startAngleX = 35f;
    public float startAngleY = 45f;
    public float startZoom = 6f;
    public float zoomSpeed = 1f; // Скорость приближения/отдаления
    public float rotateSpeed = 1f; // Скорость вращения
    public float minSize = 3f; // Минимальный размер камеры
    public float maxSize = 10f; // Максимальный размер камеры
    
    public static bool CanInput { get; set; }
    
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        CanInput = true;
        transform.rotation = Quaternion.Euler(startAngleX, startAngleY, 0f);
        mainCamera.orthographicSize = startZoom;
    }

    void Update()
    {
        if (CanInput)
        {
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                ZoomCamera(deltaMagnitudeDiff * zoomSpeed);
            }
            else
            {
                // Если нет жеста масштабирования, проверяем вращение
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Moved)
                    {
                        RotateCamera(touch.deltaPosition.x * rotateSpeed);
                    }
                }
                else
                {
                    // Если не используется тачскрин или симулятор телефона, поддерживаем управление с помощью мыши
                    if (Input.GetMouseButton(0))
                    {
                        RotateCamera(Input.GetAxis("Mouse X") * rotateSpeed);
                    }
                }
            }
        }
    }

    void ZoomCamera(float deltaMagnitudeDiff)
    {
        mainCamera.orthographicSize += deltaMagnitudeDiff * Time.deltaTime;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minSize, maxSize);
    }

    void RotateCamera(float deltaRotation)
    {
        transform.Rotate(Vector3.up, deltaRotation * Time.deltaTime, Space.World);
    }
}
