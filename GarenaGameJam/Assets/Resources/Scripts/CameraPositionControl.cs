using System;
using UnityEngine;
using Unity.Cinemachine;

public class CameraPositionControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCameraBase[] cameras;

    private int currentCamIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GoToLeft();
        }
    }

    public void GoToLeft()
    {
        // 1. Reset the current camera's priority to a base level
        cameras[currentCamIndex].Priority = 10;

        // 2. Increment the index and use Modulo (%) to wrap around to 0 automatically
        currentCamIndex = (currentCamIndex + 1) % cameras.Length;

        // 3. Set the new camera to a higher priority so Cinemachine transitions to it
        cameras[currentCamIndex].Priority = 20;
    }
}
