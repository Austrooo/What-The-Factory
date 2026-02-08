using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct GridDebugEntry
{
    public Vector3Int position;
    public CubeData data;
}
public class GridManager : MonoBehaviour, ISerializationCallbackReceiver
{
    public static GridManager Instance { get; private set; }
    public Dictionary<Vector3Int, CubeData> gridData = new Dictionary<Vector3Int, CubeData>();
    public Grid grid;
    public GameObject poolObject;
    [Header("Debug View")]
    [SerializeField] private List<GridDebugEntry> inspectorGridData = new List<GridDebugEntry>();

    private void Awake()
    {
        Instance = this;
    }

    public void OnBeforeSerialize()
    {
        inspectorGridData.Clear();
        foreach (var pair in gridData)
        {
            inspectorGridData.Add(new GridDebugEntry { position = pair.Key, data = pair.Value });
        }
    }

    public void OnAfterDeserialize()
    {
        gridData.Clear();
        foreach (var entry in inspectorGridData)
        {
            if(!gridData.ContainsKey(entry.position))
                gridData.Add(entry.position, entry.data);
        }
    }

    public CubeData GetGridData(Vector3Int cubePosition)
    {
        gridData.TryGetValue(cubePosition, out CubeData cubeData);
        return cubeData;
    }

    public void AddGridData(Vector3Int cubePosition, CubeData cubeData)
    {
        gridData.TryAdd(cubePosition, cubeData);
    }

    public void RemoveGridData(Vector3Int cubePosition)
    {
        if (!gridData.TryGetValue(cubePosition, out CubeData data)) return;

        if (data.type == CubeType.Building)
        {
            // 1. Get the Unique ID and the Physical Object
            int idToMatch = data.buildingID;
            GameObject instanceToDestroy = data.objectInstance;

            // 2. Find ALL tiles in the grid that share this Building ID
            // We collect them in a list first to avoid errors while modifying the dictionary
            List<Vector3Int> cellsToRemove = new List<Vector3Int>();
            foreach (var pair in gridData)
            {
                if (pair.Value.buildingID == idToMatch)
                {
                    cellsToRemove.Add(pair.Key);
                }
            }

            // 3. Wipe all those tiles from the dictionary
            foreach (var pos in cellsToRemove)
            {
                gridData.Remove(pos);
            }

            // 4. Physically destroy the building from the scene
            if (instanceToDestroy != null)
            {
                Destroy(instanceToDestroy);
                Debug.Log($"Destroyed building instance with ID: {idToMatch}");
            }
        }
        else
        {
            // Logic for single blocks (like basic ground cubes)
            RemoveSingleCube(cubePosition);
        }
    }
    
    public int GetNextBuildingID()
    {
        if (gridData.Count == 0) return 1;

        // Find the highest buildingID currently in the grid
        int maxId = gridData.Values.Max(d => d.buildingID);
        return maxId + 1;
    }

    private GameObject GetBuildingRoot(Transform hitTransform)
    {
        Transform current = hitTransform;
        while (current.parent != null)
        {
            if (current.parent.gameObject.name == "Placed Object")
            {
                return current.gameObject;
            }
            current = current.parent;
        }
        return null;
    }

    private void ClearBuildingDataFromGrid(Transform buildingRoot)
    {
        foreach (Transform part in buildingRoot)
        {
            Vector3Int partPos = grid.WorldToCell(part.position);
            gridData.Remove(partPos);
        }
        Vector3Int rootPos = grid.WorldToCell(buildingRoot.position);
        gridData.Remove(rootPos);
    }
    
    private void RemoveSingleCube(Vector3Int cubePosition)
    {
        gridData.Remove(cubePosition);
        foreach (Transform t in grid.transform.GetChild(0).transform)
        {
            Vector3Int childGridPos = grid.WorldToCell(t.position);

            if (childGridPos == cubePosition)
            {
                Destroy(t.gameObject);
                Debug.Log($"Successfully removed single cube at {cubePosition}");
                return;
            }
        }
    }
    
    public bool IsGridAvailable(Vector3Int cubePosition)
    {
        return !gridData.ContainsKey(cubePosition);
    }
}