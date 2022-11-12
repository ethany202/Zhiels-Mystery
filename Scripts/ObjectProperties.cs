using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{

    public float volume;

    [Range(1,2)]
    public int objectType;    // 1 = Unity Object; 2 = Prefab Object

    private float gravity;

    private bool onGround, isHeld;

    public int GetObjectType()
    {
        return objectType;
    }

    public void SetIsHeld(bool held)
    {
        isHeld = held;
    }

    public float GetVolume()
    {
        return this.volume;
    }
}
