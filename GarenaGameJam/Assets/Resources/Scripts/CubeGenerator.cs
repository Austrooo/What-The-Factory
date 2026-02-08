using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private CubeData[] allCubeDatas;
    [SerializeField] private Button buildButton;
    [SerializeField] private CubeData generatedCube1;
    [SerializeField] private CubeData generatedCube2;
    [SerializeField] private float remainingTime;
    [SerializeField] private float generateTime;

    [Header("UI Elements")] 
    [SerializeField] private Image generatedCube1Image;
    [SerializeField] private Image generatedCube2Image;

    private void Start()
    {
        generatedCube1Image.sprite = null;
        generatedCube2Image.sprite = null;
        StartCoroutine(GeneratingCycle());
    }

    private void Update()
    {
        if (generatedCube1 == null && generatedCube2 != null)
        {
            generatedCube1 = generatedCube2;
            generatedCube1Image.sprite = generatedCube1.cubeSprite;
            generatedCube2 = null;
            generatedCube2Image.sprite = null;
        }
    }

    private IEnumerator GeneratingCycle()
    {
        remainingTime = generateTime;
        while (true)
        {
            if (generatedCube2 == null && remainingTime <= 0)
            {
                generatedCube2 = allCubeDatas[Random.Range(0, allCubeDatas.Length)];
                generatedCube2Image.sprite = generatedCube2.cubeSprite;
                buildButton.onClick.AddListener(() =>
                {
                    if (generatedCube1 == null) return;
                    PlacementSystem.Instance.HoldObject(generatedCube1);
                    generatedCube1 = null;
                    generatedCube1Image.sprite = null;
                });
            }
            if(remainingTime <= 0) remainingTime = generateTime;
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }
    }
}
