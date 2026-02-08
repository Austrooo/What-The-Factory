using UnityEngine;

public class CubeController : MonoBehaviour
{
    public void DestroyCube()
    {
        Vector3Int cubePos = GridManager.Instance.grid.WorldToCell(transform.position);
        GridManager.Instance.RemoveGridData(cubePos);
        Destroy(gameObject);
    }
}
