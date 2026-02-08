using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Manual
{
    public RuleSet[] ruleSets;
} 
public class RuleManager : MonoBehaviour
{
    public static RuleManager Instance;
    [SerializeField] private GameObject spawningPool;
    [SerializeField] private Manual[] manuals;
    [SerializeField] private int currentManualIndex;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        currentManualIndex = ManualManager.Instance.GetCurrentIndex();
    }

    public void CheckForTransformation(Vector3Int placedPos, Dictionary<Vector3Int, CubeData> currentGrid)
    {
        Manual activeManual = manuals[currentManualIndex];

        foreach (RuleSet rule in activeManual.ruleSets)
        {
            foreach (var patternPart in rule.pattern)
            {
                Vector3Int assumedOrigin = placedPos - patternPart.offset;
                // Debug.Log($"Testing Rule: {rule.ruleSetName} | Placed: {placedPos} | Assuming Origin: {assumedOrigin}");
                if (IsRuleMet(assumedOrigin, rule, currentGrid))
                {
                    ExecuteTransformation(assumedOrigin, rule, currentGrid);
                    return;
                }
            }
        }
    }

    private bool IsRuleMet(Vector3Int origin, RuleSet rule, Dictionary<Vector3Int, CubeData> grid)
    {
        foreach (var p in rule.pattern)
        {
            Vector3Int targetPos = origin + p.offset;

            if (!grid.TryGetValue(targetPos, out CubeData foundCube))
                return false;

            // 1. Check if the generic types match (e.g., both are Buildings)
            if (foundCube.type != p.requiredCube.type)
                return false;

            // 2. IMPORTANT: Check if the specific prefabs match
            // This ensures a 'House' isn't mistaken for a 'Factory'
            if (foundCube.cubePrefab != p.requiredCube.cubePrefab)
                return false;
            
            // 3. Optional: Ensure we aren't trying to combine a building with itself 
            // if it's a multi-tile building (Check unique buildingID)
        }
        return true;
    }

    private void ExecuteTransformation(Vector3Int origin, RuleSet rule, Dictionary<Vector3Int, CubeData> grid)
    {
        // 1. Clear ingredients
        foreach (var p in rule.pattern)
        {
            Vector3Int targetPos = origin + p.offset;
            GridManager.Instance.RemoveGridData(targetPos);
        }

        // 2. Setup the new building ID and Object
        int newID = GridManager.Instance.GetNextBuildingID();
        Vector3 spawnPos = GridManager.Instance.grid.CellToWorld(origin) + new Vector3(1f, 0, 1f);
        GameObject buildingObj = Instantiate(rule.buildingData.cubePrefab, spawnPos, Quaternion.identity, spawningPool.transform);
        buildingObj.name = newID.ToString();

        // 3. Register every cell the building occupies
        foreach (var p in rule.pattern)
        {
            Vector3Int buildingPartPos = origin + p.offset;
        
            // Create a unique data instance for this specific building
            CubeData instanceData = Instantiate(rule.buildingData); 
            instanceData.buildingID = newID;
            instanceData.objectInstance = buildingObj;

            GridManager.Instance.AddGridData(buildingPartPos, instanceData);
        }
    }
}
