using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyPress : MonoBehaviour
{

    private Animator key;
    public static GameObject previousKey;

    private Transform playerCamera;

    public Material defaultWhiteMat;
    public Material defaultBlackMat;
    public Material hoverMat;

    public GameObject physicalKey, keyPrefab;
    private GameObject previousPhysicalKey;

    private void Update()
    {
        if (playerCamera == null)
        {
            try
            {
                //playerCamera = SpawnPlayer.playerBody.GetComponent<Camera>().transform;
                var playerBody = SpawnPlayer.playerBody.GetComponent<CharacterManager>();
                playerCamera = playerBody.cam;
            }
            catch(Exception e)
            {

            }
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 15f))
        {
            if (hit.transform.tag != "Key")
            {
                //LoadSceneLogic.DisplayInstructions(false);
                ResetPreviousKey();
                //previousKey = null;
                return;
            }

            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["open"].ToString());

            GameObject currentKey = hit.transform.gameObject;
            if (previousKey == null)
            {
                currentKey.GetComponent<MeshRenderer>().material = hoverMat;
            }
            else if (currentKey != previousKey)
            {
                ResetPreviousKey();
                currentKey.GetComponent<MeshRenderer>().material = hoverMat;
            }
            previousKey = currentKey;
            key = hit.transform.GetComponent<Animator>();
        }

        DetectInput();
    }

    public void DetectInput()
    {
        if (key != null)
        {
            if (Input.GetKeyDown(ControlsConstants.keys["open"]))
            {
                key.SetTrigger("PressKey");
                DropPhysicalKey();
            }
        }
    }

    private void ResetPreviousKey()
    {
        if (previousKey == null)
        {
            return;
        }
        if (previousKey.layer == LayerMask.NameToLayer("Black Key"))
        {
            previousKey.GetComponent<MeshRenderer>().material = defaultBlackMat;
        }
        else
        {
            previousKey.GetComponent<MeshRenderer>().material = defaultWhiteMat;
        }
    }

    private void DropPhysicalKey()
    {
        GameObject currentKey = Instantiate(keyPrefab, physicalKey.transform.position, Quaternion.identity);
        if (previousPhysicalKey != null)
        {
            Destroy(previousPhysicalKey);
        }
        previousPhysicalKey = currentKey;
        previousPhysicalKey.GetComponent<PhysicalKeyProperties>().SetKeyID(key.name);
    }
}
