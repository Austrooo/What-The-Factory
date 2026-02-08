using UnityEngine;

[System.Serializable]
public struct RelativePosition
{
    public Vector3Int offset;
    public CubeData requiredCube;
}

[CreateAssetMenu(fileName = "RuleSet", menuName = "Scriptable Objects/RuleSet")]
public class RuleSet : ScriptableObject
{
    public string ruleSetName;
    public Sprite ruleSetImage;
    public CubeData buildingData; // The "Factory" or "Result" of the pattern
    public RelativePosition[] pattern;
}
