using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{

    public Transform pickupObj;
    public Transform handLocation;
    public Transform positionPos;
    public Animator animator;


    public void Grab()
    {
        animator.SetTrigger("grab");
        //pickupObj.position = handLocation.position;
        //Debug.Log(handLocation.position);
        //pickupObj.SetParent(handLocation);
    }

    public void OnTriggerEnter(Collider other)
    {
        pickupObj = other.transform;
        //Debug.Log("Entered");
        //pickupObj.localRotation = Quaternion.Euler(0, 90, 0);

        pickupObj.SetParent(handLocation);
        //pickupObj.position = positionPos.position;
        pickupObj.position = positionPos.position;

        
        //pickupObj.localPosition = Vector3.zero;
    }

    public void OnTriggerExit(Collider other)
    {
        pickupObj = null;
    }

    /*private bool isHolding = false;
    public float maxVolume = 7f;
    public float range = 10f;
    public Camera tpCam;
    public Transform destination;
    private ObjectProperties obj;
    public Transform player;
    public GameObject instructions;
    private Rigidbody material;*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Grab();
            //Debug.Log()
        }
        /*if (!isHolding)
        {
            material = Grab();
            if (material != null)
            {         
                if (material.transform.name != player.name)
                {
                    obj = material.GetComponent<ObjectProperties>();
                    if (obj != null && obj.GetVolume() <= maxVolume)
                        instructions.SetActive(true);
                    else
                        instructions.SetActive(false);
                }      
                if (Input.GetKeyDown(KeyCode.G))
                {
                    isHolding = true;
                    HoldObject(material, destination);
                }
            }
            else
                instructions.SetActive(false);
        }
        else
        {
            instructions.SetActive(false);
            HoldObject(material, destination);
        }
        /*if (isHolding && Input.GetKeyDown(KeyCode.G))
        {

        }*/
    }
    /*private Rigidbody Grab()
    {
        RaycastHit hit;
        if (Physics.Raycast(tpCam.transform.position, tpCam.transform.forward, out hit, range))
        {
            return hit.rigidbody;
        }
        return null;
    }

    private void Release()
    {

    }
    private void HoldObject(Rigidbody material, Transform destination)
    {
        material.transform.position = destination.position;
        material.useGravity = false;
        material.freezeRotation = true;
    }*/



}
