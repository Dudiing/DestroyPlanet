using UnityEngine;

public class OrbitCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform target; // El objeto alrededor del cual la cámara girará

    [Header("Orbit Parameters")]
    public float orbitSpeed = 45f; // Velocidad de rotación reducida
    public Vector2 orbitDamping = new Vector2(10f, 10f); // Desaceleración de la rotación aumentada

    [Header("Zoom Parameters")]
    public float zoomSpeed = 1.0f; // Velocidad de zoom mucho más reducida
    public float minZoom = 15f; // Zoom mínimo más restringido
    public float maxZoom = 25f; // Zoom máximo más restringido
    public float zoomDamping = 10f; // Desaceleración del zoom

    private float targetZoom;
    private Vector2 currentRotation, targetRotation;

    // Variables para la gestión de entradas táctiles
    private Vector2 previousTouchPosition1, previousTouchPosition2;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Error: No se ha asignado un target a la cámara orbit.");
            return;
        }

        targetZoom = Vector3.Distance(transform.position, target.position);
        currentRotation.y = Vector3.Angle(Vector3.right, transform.right);
    }

    private void Update()
    {
        HandleTouchInput();
        RotateCamera();
        HandleZoom();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            // Rotación con un dedo
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                targetRotation.x += touch.deltaPosition.y * orbitSpeed * Time.deltaTime;
                targetRotation.y += touch.deltaPosition.x * orbitSpeed * Time.deltaTime;
            }
        }
        else if (Input.touchCount == 2)
        {
            // Zoom con dos dedos (pellizcar)
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchPosition1 = touch1.position;
                Vector2 currentTouchPosition2 = touch2.position;

                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    previousTouchPosition1 = touch1.position;
                    previousTouchPosition2 = touch2.position;
                }
                else
                {
                    float prevTouchDeltaMag = (previousTouchPosition1 - previousTouchPosition2).magnitude;
                    float currentTouchDeltaMag = (currentTouchPosition1 - currentTouchPosition2).magnitude;

                    float deltaMagnitudeDiff = prevTouchDeltaMag - currentTouchDeltaMag;
                    targetZoom += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;

                    previousTouchPosition1 = currentTouchPosition1;
                    previousTouchPosition2 = currentTouchPosition2;
                }
            }
        }

        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    private void RotateCamera()
    {
        if (target == null) return;

        currentRotation.x = Mathf.Lerp(currentRotation.x, targetRotation.x, 1f / orbitDamping.x);
        currentRotation.y = Mathf.Lerp(currentRotation.y, targetRotation.y, 1f / orbitDamping.y);

        transform.eulerAngles = new Vector3(-currentRotation.x, currentRotation.y, 0);
        transform.position = target.position - transform.forward * targetZoom;
    }

    private void HandleZoom()
    {
        float currentDistance = Vector3.Distance(transform.position, target.position);
        float newZoom = Mathf.Lerp(currentDistance, targetZoom, Time.deltaTime * zoomDamping);

        transform.position = target.position - transform.forward * newZoom;
    }
}
