using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static bool playerLeft = false;

    public static void SetPlayerLeft(bool val)
    {
        playerLeft = val;
    }
}
