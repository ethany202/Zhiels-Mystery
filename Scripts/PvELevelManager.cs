using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvELevelManager : MonoBehaviour
{

    public int remainingNPCs;
    public MazeElevator physicalElevator;
    
    public void CheckLevelComplete()
    {
        if (remainingNPCs == 0)
        {
            physicalElevator.SetStageCompleted(true);
            Debug.Log("Level Completed");
        }
    }
}
