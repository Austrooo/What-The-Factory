using System;
using UnityEngine;
using UnityEngine.Events;

public enum HandCondition
{
    Empty,
    Holding
}

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance { get; private set; }
    
    [Header("Object Placement Reference")] 
    [SerializeField] private GameObject gridPlacementParent;
    [SerializeField] private GameObject placementIndicatorObj;
    [SerializeField] private CubeData currentObj;
    [SerializeField] private GameObject objHeld;

    [Header("Input System")] 
    [SerializeField] private Vector3 offset = new Vector3(0.5f, 0, 0.5f); // Centering offset
    [SerializeField] private Grid grid;

    [Header("Events")] 
    public UnityEvent OnCubePlaced;

    private HandCondition _currentCondition =  HandCondition.Empty;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Vector3 mousePos = MouseInput.Instance.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        HandleIndicator(gridPos);

        if (Input.GetMouseButtonDown(0) && _currentCondition == HandCondition.Holding) 
        {
            PlaceObject(gridPos);
        }
    }

    private void HandleIndicator(Vector3Int gridPos)
    {
        bool isValid = MouseInput.Instance.isValidPlacement();
        bool isGridAvailable = GridManager.Instance.IsGridAvailable(gridPos);
        Vector3 worldPos = grid.CellToWorld(gridPos) + offset;

        switch (_currentCondition)
        {
            case HandCondition.Empty:
                placementIndicatorObj.SetActive(isValid && isGridAvailable);
                placementIndicatorObj.transform.position = worldPos;
                break;
            case HandCondition.Holding:
                objHeld.SetActive(isValid && isGridAvailable);
                objHeld.transform.position = worldPos;
                break;
        }
    }

    private void PlaceObject(Vector3Int gridPos)
    {
        if (GridManager.Instance.IsGridAvailable(gridPos) && MouseInput.Instance.isValidPlacement())
        {
            // Use the currentObj instance we created in HoldObject, 
            // which already has the buildingID and objectInstance assigned.
            GridManager.Instance.AddGridData(gridPos, currentObj);

            objHeld.transform.SetParent(gridPlacementParent.transform);
            objHeld.transform.position = grid.CellToWorld(gridPos) + offset;

            // Check rules using the refreshed grid
            RuleManager.Instance.CheckForTransformation(gridPos, GridManager.Instance.gridData);
        
            // Null out so we don't destroy it in ClearHeldObject
            objHeld = null; 
            currentObj = null; // Also null this so we don't accidentally reuse it
        
            ManualManager.Instance.IncrementPlaceBuilding();
            OnCubePlaced.Invoke();
        }
        ClearHeldObject();
    }

    public void HoldObject(CubeData cube)
    {
        if(cube == null || currentObj != null) return;
        currentObj = Instantiate(cube);
        currentObj.buildingID = GridManager.Instance.GetNextBuildingID();
        // Instantiate as a "ghost" following the mouse
        objHeld = Instantiate(cube.cubePrefab);
        currentObj.objectInstance = objHeld;
        _currentCondition = HandCondition.Holding;
    }

    public void ClearHeldObject()
    {
        if (objHeld != null && _currentCondition == HandCondition.Holding)
        {
            Destroy(objHeld);
        }
        
        currentObj = null;
        objHeld = null;
        _currentCondition = HandCondition.Empty;
    }
}