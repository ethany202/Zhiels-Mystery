using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject gunBody;
    public GameObject bulletDecal;
    //public Dictionary<string, ShootMethod> shootType = new Dictionary<string, ShootMethod>()
    //private delegate void ShootMethod();

    void Update()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["attack"]))
        {
            Shoot();
            Debug.Log("SHOT");
        }
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(gunBody.transform.position, gunBody.transform.forward, out hit, 100f))
        {
            
            GameObject newBulletDecal = Instantiate(bulletDecal, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            

            //Quaternion currentRot = newBulletDecal.GetComponent<Transform>().rotation;
            
            //currentRot.x = 90f;
            //Debug.Log(currentRot);
            //newBulletDecal.GetComponent<Transform>().rotation=currentRot;
        }
    }
}
