using System;
using UnityEngine;
using TMPro;

public class ProductionManager : MonoBehaviour
{
    public static ProductionManager Instance { get; private set; }
    [SerializeField] private int energy, material, data;
    [SerializeField] private int controlHubCount;
    
    [Header("UI Elements")]
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text dataText;
    [SerializeField] private TMP_Text materialText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        energyText.text = energy.ToString();
        dataText.text = data.ToString();
        materialText.text = material.ToString();
    }

    public int GetControlHubCount()
    {
        return controlHubCount;
    }

    public void AddControlHub()
    {
        controlHubCount++;
    }

    public void RemoveControlHub()
    {
        controlHubCount--;
    }

    public int GetValue(string value)
    {
        switch (value)
        {
            case "Energy":
                return energy;
            case "Material":
                return material;
            case "Data":
                return data;
            default:
                return 0;
        }
    }
    
    public void AddEnergy(int energy)
    {
        this.energy += energy;
        energyText.text = this.energy.ToString();
    }

    public void AddMaterial(int material)
    {
        this.material += material;
        materialText.text = this.material.ToString();
    }

    public void AddData(int data)
    {
        this.data += data;
        dataText.text = this.data.ToString();
    }
}
