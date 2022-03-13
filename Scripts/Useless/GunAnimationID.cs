using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimationID : MonoBehaviour
{
    public static Dictionary<string, float> gunTypesParam = new Dictionary<string, float>()
    {
        {"Pistol",0f },
        {"Rifle", 1.0f },
        {"Shotgun", 2.0f },
        {"Sniper", 3.0f }
    };

}
