using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTargetData : MonoBehaviour
{
    [Header("Physical Phone Data")]
    public GameObject[] targetData;

    [Header("Display Phone Data")]
    public GameObject[] displayData;

    public int currentIndex;

    void Update()
    {
        if (MapInfoController.multiplayerMapScene == 3)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentIndex++;
            }
            if (currentIndex >= targetData.Length)
            {
                currentIndex = 0;
            }  
            UpdateScreen();
        }
        
    }

    private void UpdateScreen()
    {
        for (int i = 0; i < targetData.Length; i++)
        {
            if (currentIndex == i)
            {
                targetData[currentIndex].SetActive(true);
                displayData[currentIndex].SetActive(true);
            }
            else
            {
                targetData[i].SetActive(false);
                displayData[i].SetActive(false);
            }
        }
    }
}
