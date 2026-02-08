using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    
    [Header("UI Elements")] 
    [SerializeField] private TMP_Text timerText;

    [Header("Settings")] 
    [SerializeField] private int betrayalLimit = 360;
    [SerializeField] private float elapsedTime = 0;

    [Header("Events")] 
    public UnityEvent OnBetrayalStart;
    
    public float GetElapsedTime() => elapsedTime;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Mathf.Round(elapsedTime) == betrayalLimit)
        {
            OnBetrayalStart.Invoke();
        }
        elapsedTime += Time.deltaTime;
        string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
        string seconds = Mathf.Floor(elapsedTime % 60).ToString("00");
        timerText.text = minutes + ":" + seconds;
    }
}
