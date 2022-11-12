using UnityEngine;
using TMPro;

public class StatsSinglePlayer : MonoBehaviour
{

    public GameObject fpsUI;
    public TMP_Text fpsText;

    void Start()
    {
        fpsUI.SetActive(CustomizedData.showFPS);
    }

    void Awake()
    {
        if (CustomizedData.showFPS)
        {
            InvokeRepeating("ShowFPS", 0f, 1f);
        }
    }

    void ShowFPS()
    {
        fpsText.text = "fps: " + (int)(1 / Time.unscaledDeltaTime);
    }
}
