using UnityEngine;
using UnityEngine.Events;

public class DestroyManager : MonoBehaviour
{
    [SerializeField] private bool isDestroying = false;
    [SerializeField] private GameObject poolParent;

    [SerializeField] private GameObject destroyButton;
    [SerializeField] private GameObject cancelButton;

    [Header("Events")] 
    public UnityEvent OnCubeDestroyed;
    private void Update()
    {
        if (!isDestroying) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = MouseInput.Instance.GetSelectedMapPosition();
            Vector3Int gridPos = GridManager.Instance.grid.WorldToCell(mousePos);
            CubeData gridData = GridManager.Instance.GetGridData(gridPos);
            Debug.Log("Grid Data : " + gridData);
            if (gridData != null)
            {
                GridManager.Instance.RemoveGridData(gridPos);
                OnCubeDestroyed.Invoke();
            }
            ToggleDestroyMode();
        }
    }

    public void ToggleDestroyMode()
    {
        isDestroying = !isDestroying;
        destroyButton.SetActive(!isDestroying);
        cancelButton.SetActive(isDestroying);
    }
}
