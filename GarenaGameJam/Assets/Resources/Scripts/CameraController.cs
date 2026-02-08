using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Panning Limits (Relative to Start)")]
    public float leftLimit = -10f;  // How far left from start
    public float rightLimit = 10f; // How far right from start
    public float downLimit = -5f;  // How far down from start
    public float upLimit = 5f;    // How far up from start

    [Header("Settings")]
    public float dragSpeed = 20f;
    public float zoomSpeed = 5f;
    public float minSize = 5f;
    public float maxSize = 20f;

    private Vector3 startPosition;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        startPosition = cam.transform.position;
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    void HandlePan()
    {
        if (Input.GetMouseButton(0))
        {
            // 1. Get mouse input
            float moveX = Input.GetAxisRaw("Mouse X") * dragSpeed * Time.deltaTime;
            float moveY = Input.GetAxisRaw("Mouse Y") * dragSpeed * Time.deltaTime;

            // 2. Define "Flat" vectors for a 45-degree isometric view
            // These vectors move perfectly left/right and up/down on your screen
            Vector3 screenHorizontal = new Vector3(1, 0, 1).normalized; // "Flat" horizontal
            Vector3 screenVertical = new Vector3(-1, 0, 1).normalized;   // "Flat" vertical

            // 3. Combine movement (Inverted as requested)
            Vector3 moveDir = (screenHorizontal * -moveY) + (screenVertical * moveX);

            Vector3 targetPos = cam.transform.position + moveDir;

            // 4. Clamp based on your start position
            // We clamp X and Z because in isometric, "Up/Down" is Z movement
            targetPos.x = Mathf.Clamp(targetPos.x, startPosition.x + leftLimit, startPosition.x + rightLimit);
            targetPos.z = Mathf.Clamp(targetPos.z, startPosition.z + downLimit, startPosition.z + upLimit);

            cam.transform.position = targetPos;
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (scroll * zoomSpeed), minSize, maxSize);
        }
    }
}