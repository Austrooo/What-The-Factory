using System;
using System.Collections;
using UnityEngine;

public enum FactoryType
{
    Generator,
    Assembler,
    DataCenter,
    Refinary,
    ControlHub
}
public class FactoryManager : MonoBehaviour
{
    [SerializeField] private FactoryType type;
    [SerializeField] private float productionRate;

    private void Start()
    {
        StartCoroutine(StartProduction());
    }

    private IEnumerator StartProduction()
    {
        if (type == FactoryType.ControlHub)
        {
            ProductionManager.Instance.AddControlHub();
            yield return null;
        }
        
        while (true)
        {
            switch (type)
            {
                case FactoryType.Assembler:
                    ProductionManager.Instance.AddMaterial(1 * (1 + ProductionManager.Instance.GetControlHubCount()));
                    break;
                case FactoryType.Generator:
                    ProductionManager.Instance.AddEnergy(1 * (1 + ProductionManager.Instance.GetControlHubCount()));
                    break;
                case FactoryType.DataCenter:
                    ProductionManager.Instance.AddData(1 * (1 + ProductionManager.Instance.GetControlHubCount()));
                    break;
                case FactoryType.Refinary:
                    ProductionManager.Instance.AddEnergy(2);
                    ProductionManager.Instance.AddData(2);
                    break;
            }
            yield return new WaitForSeconds(productionRate);
        }
    }

    private void OnDestroy()
    {
        ProductionManager.Instance.RemoveControlHub();
    }
}
