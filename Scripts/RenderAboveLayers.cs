using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderAboveLayers : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
    }
}
