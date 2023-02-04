using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvELevelManager : MonoBehaviour
{

    public bool checkProceedStage;

    public int remainingNPCs;
    public MazeElevator physicalElevator;

    public ElevatorMovement elevatorMvt;

    public void CheckLevelComplete()
    {
        if (remainingNPCs == 0)
        {
            physicalElevator.SetStageCompleted(true);

            if (checkProceedStage)
            {
                elevatorMvt.levelCleared = true;
                physicalElevator.ChangeOperate(true);
            }
        }
    }
}
