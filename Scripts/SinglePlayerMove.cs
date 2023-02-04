using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayerMove : MonoBehaviour
{
    [Header("Character Meta Data")]
    public CharacterController character;
    public TPSMouseLook viewObject;
    public Animator animator, knifeAnim, pistolAnim;
    public GameObject armsKnife, armsPistol, phoneObj;
    public Camera camObj;
    private ExamHandler examHandler;

    // Transforms:
    [Header("Transform Data Types")]
    public Transform cam;
    public Transform body;
    public Transform cameraPos;
    public Transform grabPos;

    // Data Values:
    private float crouchingVal = 0f;
    private float velocityNormalized = 0f;
    private float playerHealth = 100f;

    // Presets:
    [Header("Character Presets")]
    public float baseSpeed = 3f;
    public float sprintSpeed = 6f;
    public float jumpHeight = 0.3f;
    public float weight;
    public float strength;
    public float visionScore;

    private float characterHeight = 0f;
    private float characterStep = 0f;
    private float gravity;
    private float moveSpeed;

    private Vector3 moveDir;
    private bool grabbingObject;

    private Transform objectBody;

    void Awake()
    {
        examHandler = GetComponent<ExamHandler>();
        
        // viewObject.SetNormalSensitivity(CustomizedData.normalSensitivity);
        viewObject.SetVision(visionScore * 200);

        moveSpeed = baseSpeed;
        characterHeight = character.height;
        characterStep = character.stepOffset;

        //LoadSceneLogic.player = this;
        //LoadSaveState();

        if (ControlsConstants.keys.Count == 0)
        {
            ControlsConstants.SetDefaultKeys();
        }
    }

    void Update()
    {
        NormalMovement();
        CheckCrouch();
        //CheckJump();
        CallGravity();
        Attack();
        HoldingObject();
        DropObject();
        CheckIdle();
        viewObject.Rotate();
        cam.position = cameraPos.position;
    }

    public void NormalMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude > 0f)
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
            animator.SetBool(AnimationParameters.parameters["isCrouching"], true);
            moveSpeed = baseSpeed / 2f;
            character.height = characterHeight / 2f;
            character.stepOffset = 0f;
            //animator.SetFloat(AnimationParameters.floats["crouching"], crouchingVal);

        }
        if(!Input.GetKey(ControlsConstants.keys["crouch"]))
        {
            animator.SetBool(AnimationParameters.parameters["isCrouching"], false);
            character.height = characterHeight;
            character.stepOffset = characterStep;
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
                moveSpeed = sprintSpeed;
                animator.SetFloat(AnimationParameters.floats["velocityNormalized"], 1f);
            }
        }
        if (!Input.GetKey(ControlsConstants.keys["sprint"]))
        {
            moveSpeed = baseSpeed;
            animator.SetFloat(AnimationParameters.floats["velocityNormalized"], 0f);
        }
    }

    public void CheckJump()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["jump"]))
        {
            character.Move(new Vector3(0f, 20f * Time.deltaTime, 0f));
        }
    }

    private void Attack()
    {
        if (armsKnife.activeInHierarchy)
        {
            if (Input.GetKeyDown(ControlsConstants.keys["attack"]))
            {
                knifeAnim.SetTrigger(Animator.StringToHash("attack"));
            }
        }
        if (armsPistol.activeInHierarchy)
        {
            if (Input.GetKeyDown(ControlsConstants.keys["attack"]))
            {
                pistolAnim.SetTrigger(Animator.StringToHash("shoot"));
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                pistolAnim.SetTrigger(Animator.StringToHash("reload"));
            }
        }
    }

    private void GrabObject()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["grab"]))
        {
            UpdatePosition();
            grabbingObject = true;
        }
    }


    private void UpdatePosition()
    {
        if (objectBody != null)
        {
            if (objectBody.gameObject.layer == LayerMask.NameToLayer("Knife"))
            {
                armsKnife.SetActive(true);
            }
            if(objectBody.gameObject.layer == LayerMask.NameToLayer("Pistol"))
            {
                armsPistol.SetActive(true);
                camObj.enabled = false;
            }
            if(objectBody.gameObject.layer==LayerMask.NameToLayer("Holdable Object"))
            {
                cam.GetComponentInChildren<PhysicalKeyProperties>(true).gameObject.SetActive(true);
            }
            objectBody.gameObject.SetActive(false);
            LoadSceneLogic.DisplayInstructions(false);
        }
    }

    private void DropObject()
    {
        if (grabbingObject)
        {
            if (Input.GetKeyDown(ControlsConstants.keys["drop"]))
            {
                grabbingObject = false;

                objectBody.position = grabPos.position;
                objectBody.gameObject.SetActive(true);

                objectBody = null;

                armsKnife.SetActive(false);
                armsPistol.SetActive(false);
                cam.GetComponentInChildren<PhysicalKeyProperties>(true).gameObject.SetActive(false);

                camObj.enabled = true;
            }
        }
    }

    private void HoldingObject()
    {
        if (grabbingObject)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {                
                if(objectBody.gameObject.layer == LayerMask.NameToLayer("Pistol"))
                {
                    armsPistol.SetActive(!armsPistol.activeInHierarchy);
                    camObj.enabled = !camObj.enabled;
                }
                if(objectBody.gameObject.layer == LayerMask.NameToLayer("Knife"))
                {
                    armsKnife.SetActive(!armsKnife.activeInHierarchy);
                }
                //if(objectBody.gameObject.layer==)

            }
        }

        /*if (Input.GetKeyDown(KeyCode.Alpha2) && SceneManager.sceneCount==1)
        {
            phoneObj.SetActive(!phoneObj.activeInHierarchy);
        }*/
    }

    public void CheckIdle()
    {
        if (!Input.anyKey)
        {
            ResetAnimations("isIdle");
        }
        else
        {
            animator.SetBool(AnimationParameters.parameters["isIdle"], false);
        }

    }

    public void CallGravity()
    {
        if (character.isGrounded)
        {
            gravity = 0f;

        }
        else
        {
            character.Move(new Vector3(0f, gravity, 0f));
            gravity -= (0.1f * Time.deltaTime);
        }
    }

    /*private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Breakable")
        {
            return;
        }

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        Vector3 pushDir = new Vector3(moveSpeed * weight * hit.moveDirection.x, 0f, moveSpeed * weight * hit.moveDirection.z);
        body.AddForceAtPosition(pushDir, hit.point);
    }*/

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

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Object")
        {
            if (!grabbingObject)
            {
                LoadSceneLogic.DisplayInstructions(true);
                LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["grab"].ToString());
                objectBody = other.GetComponent<Transform>();
                GrabObject();
            }
            else
            {
                LoadSceneLogic.DisplayInstructions(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            LoadSceneLogic.DisplayInstructions(false);
        }
    }

    public float GetHealth()
    {
        return playerHealth;
    }

    public void SetHealth(float newHealth)
    {
        playerHealth = newHealth;
        //LoadSceneLogic.GetHealthText().GetComponent<TMP_Text>().text = "" + (int)playerHealth;
    }

    public Transform GetObjectBody()
    {
        return objectBody;
    }

    public bool IsGrabbingObject()
    {
        return grabbingObject;
    }

    public void LoadSaveState()
    {
        if (!LoadSceneLogic.savedGame)
        {
            examHandler.SetExamState(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        else
        {
            SaveData data = SaveSystem.LoadPlayerState();
            Vector3 position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);

            body.position = position;
            this.playerHealth = data.playerHealth;
            
            if(data.holdingKnife)
            {
                grabbingObject = true;
                armsKnife.SetActive(true);
                objectBody = GameObject.Find(data.objectName).transform;
            }
            if (data.holdingPistol)
            {
                grabbingObject = true;
                armsPistol.SetActive(true);
                objectBody = GameObject.Find(data.objectName).transform;
            }
        }
    }

}