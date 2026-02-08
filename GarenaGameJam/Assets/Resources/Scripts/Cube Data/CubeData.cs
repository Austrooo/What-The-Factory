using UnityEngine;

public enum CubeType
{
    Cube,
    Building
}

[CreateAssetMenu(fileName = "CubeData", menuName = "Scriptable Objects/CubeData")]
public class CubeData : ScriptableObject
{
    public GameObject cubePrefab;
    public Sprite cubeSprite;
    public string cubeID;
    public CubeType type;
    public int buildingID;
    public GameObject objectInstance;
}
