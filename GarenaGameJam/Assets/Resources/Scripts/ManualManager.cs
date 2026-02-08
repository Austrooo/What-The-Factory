using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ManualManager : MonoBehaviour
{
    public static  ManualManager Instance;
    private int _currentManualIndex = 0;
    private Animator _animator;
    private bool isOpen = false;
    private int targetForChange;
    private int placedBuilding;
    [SerializeField] private Sprite[] manualImages;
    [SerializeField] private Image manualUI;

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetTargetForChangingManual();
    }

    private void FixedUpdate()
    {
        if (targetForChange == placedBuilding)
        {
            _currentManualIndex = Random.Range(0, manualImages.Length);
            SetTargetForChangingManual();
            placedBuilding = 0;
        }
    }

    private void SetTargetForChangingManual()
    {
        targetForChange = Random.Range(5, 9);
    }

    private void Update()
    {
        manualUI.sprite = manualImages[_currentManualIndex];
    }

    public void ToggleManual()
    {
        if (isOpen)
        {
            _animator.Play("CloseManual");
        }
        else
        {
            _animator.Play("OpenManual");
        }
        isOpen = !isOpen;
    }
    public int GetCurrentIndex() => _currentManualIndex;

    public void IncrementPlaceBuilding()
    {
        placedBuilding++;
    }
    
}
