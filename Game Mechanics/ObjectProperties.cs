using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{

    public float volume;
    public int objectId;    // Determines the type of object(i.e., gun, sword, knife, hammer, box)

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public float GetVolume()
    {
        return this.volume;
    }
}
