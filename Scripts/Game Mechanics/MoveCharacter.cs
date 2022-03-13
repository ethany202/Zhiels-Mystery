using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MoveCharacter : MonoBehaviour
{
    [Header("Character Meta Data")]
    public PhotonView PV;
    public CharacterController character;
    public TPSMouseLook viewObject;
    public Camera camObj;
    public Animator animator;

    // Transforms:
    [Header("Transform Data Types")]
    public Transform cam;
    public Transform body;
    //public Transform headTarget;
    public Transform grabPos;//, carryPos;
    public Transform cameraPos;

    [Header("GameObject Data Types")]
    public GameObject phoneObj; // Zhieltropolis
    public GameObject phoneDisplay;

    public GameObject scrollObj; // Zeliticus
    public GameObject headGearObj; // Cyberpunk

    // Data Values:
    private float knifeVal = 0f;
    private float crouchingVal = 0f;
    private float velocityNormalized = 0f;
    private float carryingVal = 0f;
    private float targetData = 0f;
    private float playerHealth = 100f;

    // Presets:
    [Header("Character Presets")]
    public float baseSpeed = 3f;
    public float sprintSpeed = 6f;
    public float jumpHeight = 0.3f;
    public float weight;
    public float strength;
    public float visionScore;

    /*[Range(0, 1)] public float rightDistanceToGround;
    [Range(0, 1)] public float leftDistanceToGround;
    [Range(0, 1)] private float headIKWeight = 0f;
    public LayerMask layerMask;*/

    // Animation:

    private float characterHeight = 0f;
    private float gravity;
    private float moveSpeed;

    private Vector3 moveDir;
    private bool holdingKnife;
    private bool holdingGun;
    private bool grabbingObject;

    private ObjectProperties handObject;
    private Transform objectBody;

    void Start()
    {
        targetData = (float)MapInfoController.multiplayerMapScene - 3f;

        animator.SetFloat(AnimationParameters.floats["targetData"], targetData);

        viewObject.SetNormalSensitivity(75f);
        viewObject.SetScopeSensitivity(50f);
        viewObject.SetVision(visionScore*200);

        moveSpeed = baseSpeed;
        characterHeight = character.height;

        if (!PV.IsMine)
        {
            camObj.enabled = false;
            camObj.GetComponent<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if (PV.IsMine)
        {
            NormalMovement();
            CheckCrouch();
            CheckJump();
            CallGravity();
            Aim();
            Attack();
            DropObject();
            CheckIdle();
            CheckTargetData();
            viewObject.Rotate();
            cam.position = cameraPos.position;
        }
    }

    public void NormalMovement()
    {
        if (animator.GetBool(AnimationParameters.parameters["checkingTargetData"]))
        {
            return;
        }

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.01f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool(AnimationParameters.parameters["isWalkingBackwards"], true);
                moveSpeed = baseSpeed / 1.15f;
            }
            else
            {
                if (!Input.GetKey(ControlsConstants.keys["crouch"]))
                {
                    moveSpeed = baseSpeed;
                    animator.SetBool(AnimationParameters.parameters["isMoving"], true);
                    CheckSprint();
                }
            }
            character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            moveDir = Vector3.zero;
        }
    }

    public void CheckCrouch()
    {
        if (Input.GetKey(ControlsConstants.keys["crouch"]))
        {
            ResetAnimations("isCrouching");
            moveSpeed = baseSpeed / 2f;
            if (characterHeight == character.height)
            {
                character.height *= 0.8f;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
            {
                if (crouchingVal < 1f)
                {
                    crouchingVal += 0.1f;
                }
                animator.SetFloat(AnimationParameters.floats["crouching"], crouchingVal);
                character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            }
            else
            {
                if (crouchingVal > 0f)
                {
                    crouchingVal -= 0.1f;
                }
                animator.SetFloat(AnimationParameters.floats["crouching"], crouchingVal);
            }
        }
        else
        {
            character.height = characterHeight;
        }
    }

    private void CheckSprint()
    {
        if (Input.GetKey(ControlsConstants.keys["sprint"]))
        {
            if (Input.GetKey(KeyCode.S))
                return;
            else
            {
                if (velocityNormalized < 1f)
                {
                    velocityNormalized += 0.1f;
                }
                moveSpeed = sprintSpeed;
                animator.SetFloat(AnimationParameters.floats["velocityNormalized"], 1f);
            }
        }
        if (!Input.GetKey(ControlsConstants.keys["sprint"]))
        {
            if (velocityNormalized > 0f)
            {
                velocityNormalized -= 0.1f;
            }
            moveSpeed = baseSpeed;
            animator.SetFloat(AnimationParameters.floats["velocityNormalized"], velocityNormalized);
        }
    }

    public void CheckJump()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["jump"]))
        {
            if (!Input.GetKey(KeyCode.S))
            {
                animator.SetTrigger(AnimationParameters.triggers["jump"]);
                moveSpeed = 0;
            }
        }
    }

    private void GrabObject()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["grab"]))
        {
            PV.RPC("UpdatePosition", RpcTarget.AllBuffered, null);   
        }
    }

    [PunRPC]
    private void UpdatePosition()
    {
        if (objectBody != null)
        {
            grabbingObject = true;
            objectBody.GetComponent<Rigidbody>().isKinematic = true;

            Transform prefabObj = null;
            int idIndex = objectBody.name.IndexOf("{");
            if (idIndex == -1)
            {
                prefabObj = grabPos.Find(objectBody.name);
            }
            else
            {
                prefabObj = grabPos.Find(objectBody.name.Substring(0, idIndex));
            }

            objectBody.position = prefabObj.position;
            objectBody.rotation = prefabObj.rotation;

            objectBody.parent = grabPos;

            if (prefabObj.gameObject.CompareTag("Knife"))
            {
                holdingKnife = true;
            }
            if (prefabObj.gameObject.CompareTag("Pistol"))
            {
                holdingGun = true;
                //gunBody = objectBody.gameObject;
            }
        }
        
    }

    public void ResetCharacterHeight()
    {
        character.height = characterHeight;
    }

    private void DropObject()
    {
        if (grabbingObject)
        {
            if (Input.GetKeyDown(ControlsConstants.keys["drop"]))
            {
                grabbingObject = false;
                holdingKnife = false;
                holdingGun = false;

                objectBody.SetParent(null);

                //objectBody.GetComponent<Rigidbody>().isKinematic = false;
                carryingVal = 0f;
                animator.SetFloat(AnimationParameters.floats["carrying"], carryingVal);

                PV.RPC("ReleaseObject", RpcTarget.AllBufferedViaServer, objectBody.name);

                objectBody = null;
                handObject = null;

            }
        }
    }

    [PunRPC]
    private void ReleaseObject(string objectName)
    {
        //Destroy(GameObject.Find(objectName));   
        GameObject obj = GameObject.Find(objectName);
        obj.transform.SetParent(null);
        obj.GetComponent<Rigidbody>().isKinematic = false;
        //obj.name = obj.name.Substring(LoadSceneLogic.playerID.Length + 1);
        //Instantiate(objectBody, objectBody.position, objectBody.rotation);
    }

    public void Attack()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["attack"]))
        {
            if (holdingGun)
            {
                Shoot();
            }
            else
            {
                Punch();
            }
        }
    }

    public void Punch()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetTrigger(AnimationParameters.triggers["punch"]);
        }
        else
        {
            animator.SetTrigger(AnimationParameters.triggers["idlePunch"]);
        }
    }

    public void Aim()
    {
        if (Input.GetKey(ControlsConstants.keys["scope"]) && holdingGun)
        {
            animator.SetBool(AnimationParameters.parameters["isAiming"], true);
        }
        else
        {
            animator.SetBool(AnimationParameters.parameters["isAiming"], false);
        }
    }

    private void Shoot(){animator.SetTrigger(AnimationParameters.triggers["shoot"]);}

    private void Bullet()
    {
        /*RaycastHit hit;
        if (Physics.Raycast(gunBody.transform.position, gunBody.transform.right, out hit, 100f))
        {
            Vector3 point = hit.point;
            point.x += 0.002f;
            point.y += 0.002f;
            point.z += 0.002f;
            GameObject newBulletDecal = Instantiate(bulletDecal, point, Quaternion.FromToRotation(Vector3.back, hit.normal));
        }*/
    }

    public void CheckIdle()
    {
        if (!Input.anyKey)
        {
            ResetTriggers();
            ResetAnimations();
            animator.SetBool(AnimationParameters.parameters["isIdle"], true);
            if (holdingKnife)
            {
                knifeVal = (knifeVal < 1f) ? knifeVal+0.1f : 1f;
                carryingVal = (carryingVal < 1f) ? carryingVal+0.1f : 1f;
                animator.SetFloat(AnimationParameters.floats["knife"], knifeVal);
                animator.SetFloat(AnimationParameters.floats["carrying"], carryingVal);
            }
            else
            {
                knifeVal = (knifeVal>0f) ? knifeVal-0.1f : 0f;
                //carryingVal = 0f;
                animator.SetFloat(AnimationParameters.floats["knife"], knifeVal);
                //animator.SetFloat(AnimationParameters.floats["carrying"], carryingVal);
            }
        }
        else
        {
            animator.SetBool(AnimationParameters.parameters["isIdle"], false);
            //
        }

    }

    private void CheckTargetData()
    {
        if (LoadSceneLogic.playerRole != 0)
        {
            return;
        }

        if (Input.GetKeyDown(ControlsConstants.keys["targetData"]))
        {
            if (MapInfoController.multiplayerMapScene == 3)
            {
                phoneDisplay.SetActive(true);
                phoneObj.SetActive(true);
                animator.SetBool(AnimationParameters.parameters["checkingTargetData"], !animator.GetBool(AnimationParameters.parameters["checkingTargetData"]));
            }
        }
        if(Input.GetKeyUp(ControlsConstants.keys["targetData"]))
        {
            if (MapInfoController.multiplayerMapScene == 3)
            {
                phoneDisplay.SetActive(false);
                phoneObj.SetActive(false);
            }
        }

    }

    public void CallGravity()
    {
        if (character.isGrounded)
        {
            return;
        }
        else if (!character.isGrounded)
        {
            character.Move(new Vector3(0f, gravity, 0f) * Time.deltaTime);
            gravity -= (0.5f);
        }

        if (character.isGrounded && gravity < 0)
        {
            gravity = -1f;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Object" && hit.transform.parent!=null)
        {
            playerHealth -= 10f;
            LoadSceneLogic.GetHealthText().GetComponent<TMP_Text>().text = playerHealth + "";
        }

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        Vector3 pushDir = new Vector3(moveSpeed * weight * hit.moveDirection.x * 0.5f, 9.8f * hit.moveDirection.y * 0.5f, moveSpeed * weight * hit.moveDirection.z * 0.5f);
        body.AddForceAtPosition(pushDir / Mathf.Sqrt(body.mass), hit.point);
    }

    public void ResetAnimations()
    {
        ResetAnimations("");
    }

    public void ResetAnimations(string exception)
    {
        foreach (KeyValuePair<string, int> val in AnimationParameters.parameters)
        {
            if (val.Key != exception)
            {
                animator.SetBool(val.Value, false);
            }
            else
            {
                animator.SetBool(val.Value, true);
            }
        }
    }

    public void ResetTriggers()
    {
        foreach (KeyValuePair<string, int> val in AnimationParameters.triggers)
        {
            animator.ResetTrigger(val.Value);
        }
    }

    /*void OnAnimatorIK()
    {
        if (animator)
        {
            //InitializeFootIKWeights();
            //FootIKPlacement();
            //HeadIKTilt();
        }
    }*/

    /*private void InitializeFootIKWeights()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.5f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.5f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.5f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.5f);
    }

    private void FootIKPlacement()
    {
        RaycastHit hit;
        Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, rightDistanceToGround + 1f, layerMask))
        {
            Vector3 footPos = hit.point;
            footPos.y += rightDistanceToGround;
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
        }

        ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, leftDistanceToGround + 1f, layerMask))
        {
            Vector3 footPos = hit.point;
            footPos.y += leftDistanceToGround;
            animator.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
        }
    }

    private void HeadIKTilt()
    {
        if (moveDir.x == 0 && moveDir.y == 0 && moveDir.z == 0)
        {
            if (headIKWeight < 0.7f)
                headIKWeight += 0.01f;
            animator.SetLookAtWeight(headIKWeight);
            animator.SetLookAtPosition(headTarget.position);
        }
        else
        {
            if (headIKWeight > 0f)
                headIKWeight -= 0.01f;
            animator.SetLookAtWeight(headIKWeight);
        }
    }*/

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Object")
        {
            if (!grabbingObject)
            {
                //grabInstructions.SetActive(true);
                handObject = other.GetComponent<ObjectProperties>();
                objectBody = other.GetComponent<Transform>();
                GrabObject();
            }
            else
            {
                //grabInstructions.SetActive(false);
            }
        }
        if (other.tag == "Region")
        {

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            //grabInstructions.SetActive(false);
        }
    }

}