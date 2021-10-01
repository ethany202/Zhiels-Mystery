using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorProperties : MonoBehaviour
{
    // Start is called before the first frame update

    private bool isOpen = false;
    public Transform door;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckOpen();
    }
    private void CheckOpen()
    {
        if (door.rotation.y == 0)
        {
            isOpen = false;
        }
        else
        {
            isOpen = true;
        }
    }
    public bool GetIsOpen()
    {
        return this.isOpen;
    }
}
