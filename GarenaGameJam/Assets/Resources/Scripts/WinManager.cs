using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinManager : MonoBehaviour
{
    [SerializeField] private int dataTarget;
    [SerializeField] private int materialTarget;
    [SerializeField] private int energyTarget;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text minutesText;
    [SerializeField] private TMP_Text secondsText;
    [SerializeField] private Sprite[] extraImages;
    [SerializeField] private Image imageUI;
    
    private bool isTargetReached;

    private void Start()
    {
        isTargetReached = false;
        DefineTarget();
    }

    private void Update()
    {
        TargetReached();
    }

    private void TargetReached()
    {
        if (isTargetReached) return;
        Debug.Log(ProductionManager.Instance.GetValue("data"));
        if (ProductionManager.Instance.GetValue("Data") < dataTarget) return;
        if (ProductionManager.Instance.GetValue("Material") < materialTarget) return;
        if (ProductionManager.Instance.GetValue("Energy") < energyTarget) return;
        isTargetReached = true;
        ToggleWinScreen();
    }

    public void ToggleWinScreen()
    {
        winPanel.SetActive(!winPanel.activeSelf);
        minutesText.text = Mathf.Floor(Timer.Instance.GetElapsedTime()/60f).ToString("00");
        secondsText.text = Mathf.Floor(Timer.Instance.GetElapsedTime()%60f).ToString("00");
    }

    private void DefineTarget()
    {
        int rnd = Random.Range(0, 3);
        switch (rnd)
        {
            case 0:
                dataTarget = 2000;
                materialTarget = 1000;
                energyTarget = 1000;
                imageUI.sprite = extraImages[0];
                break;
            case 1:
                dataTarget = 1000;
                materialTarget = 2000;
                energyTarget = 1000;
                imageUI.sprite = extraImages[1];
                break;
            case 2:
                dataTarget = 1000;
                materialTarget = 1000;
                energyTarget = 2000;
                imageUI.sprite = extraImages[2];
                break;
        }
        imageUI.gameObject.SetActive(true);
    }
}
