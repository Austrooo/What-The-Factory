using System;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public static MouseInput Instance { get; private set; }
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask placementLayerMask;

    private bool validPlacement;
    private Vector3 _lastMousePosition;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    public Vector3 GetSelectedMapPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, placementLayerMask))
        {
            validPlacement = true;
            return hit.point;
        }
        validPlacement = false;
        return  Vector3.zero;
    }

    public bool isValidPlacement() => validPlacement;
}
