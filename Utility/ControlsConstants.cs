using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsConstants : MonoBehaviour
{

    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public static Dictionary<string, KeyCode> GetKeys()
    {
        return keys;
    }
}
