using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerMove : MonoBehaviour
{

    public CharacterController character;
    public TPSMouseLook viewObject;
    public Camera camObj;

    // Transforms:
    public Transform cam;
    public Transform body;
    public Transform headTarget;
    public Transform grabPos, carryPos;
    public Transform camPos;

    public GameObject phoneObj;
    public GameObject phoneDisplay;
    // Data Values:
    private float knifeVal = 0f;
    private float crouchingVal = 0f;
    private float velocityNormalized = 0f;
    private float carryingVal = 0f;

    // Presets:
    private float baseSpeed = 3f;
    private float sprintSpeed = 4f;
    private float jumpHeight = 0.3f;
    private float characterHeight = 0f;

    private float weight;
    private float strength;
    private float moveSpeed;
    private float gravity;

    [Range(0, 1)] public float rightDistanceToGround;
    [Range(0, 1)] public float leftDistanceToGround;
    [Range(0, 1)] private float headIKWeight = 0f;
    public LayerMask layerMask;

    // Animation:
    public Animator animator;

    private Vector3 moveDir;

    private bool holdingKnife;
    private bool holdingGun = true;
    private bool grabbingObject=false;

    public GameObject defaultKnife, bulletDecal;
    //public GameObject grabInstructions, dropInstructions;

    private ObjectProperties handObject, bulletSmoke;
    private Transform objectBody;



    void Start()
    {

        moveSpeed = baseSpeed;

        characterHeight = character.height;

        holdingKnife = false;
    }

    void Update()
    {
        NormalMovement();
        CheckCrouch();
        CheckJump();
        Punch();
        //HoldKnife();
        CheckTargetData();
        CallGravity();
        DropObject();
        CheckIdle();
        Aim();
        cam.position = camPos.position;
        viewObject.Rotate();
    }

    private void CheckTargetData()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            phoneDisplay.SetActive(true);
            phoneObj.SetActive(true);
            animator.SetBool(AnimationParameters.parameters["checkingTargetData"], !animator.GetBool(AnimationParameters.parameters["checkingTargetData"]));
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            phoneDisplay.SetActive(false);
            phoneObj.SetActive(false);
        }
    }

    public void NormalMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
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
                if (!Input.GetKey(KeyCode.LeftShift))
                {
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

    private void Bullet() { }

    public void CheckCrouch()
    {
        if (Input.GetKey(KeyCode.LeftShift))
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
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.S))
                return;
            else
            {
                if (velocityNormalized < 1f)
                {
                    velocityNormalized += 0.1f;
                }
                if (moveSpeed < sprintSpeed)
                {
                    moveSpeed += 0.2f;
                }
                animator.SetFloat(AnimationParameters.floats["velocityNormalized"], 1f);
            }
        }
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            if (velocityNormalized > 0f)
            {
                velocityNormalized -= 0.1f;
            }
            if (moveSpeed > baseSpeed)
            {
                moveSpeed -= 0.5f;
            }
            animator.SetFloat(AnimationParameters.floats["velocityNormalized"], velocityNormalized);
        }
    }

    public void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!Input.GetKey(KeyCode.S))
            {
                animator.SetTrigger(AnimationParameters.triggers["jump"]);
                moveSpeed = 0;
            }
        }
    }

    private void MoveUp()
    {
        character.Move(new Vector3(0f, jumpHeight, 0f) * 0.5f);
    }

    private void GrabObject()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetObjectGrab();
        }
    }

    private void SetObjectGrab()
    {
        grabbingObject = true;
        objectBody.GetComponent<Rigidbody>().isKinematic = true;
        if (handObject.GetVolume() <= 5f)
        {

            if (handObject.GetObjectType()==2)
            {
                objectBody.gameObject.SetActive(false);
                Transform prefabObj = grabPos.Find(objectBody.name);
                prefabObj.gameObject.SetActive(true);
                if (prefabObj.gameObject.CompareTag("Knife"))
                {
                    holdingKnife = true;
                }
            }
            else
            {
                objectBody.position = grabPos.position;
                objectBody.SetParent(grabPos);
            }
        }
    }

    private void DropObject()
    {
        if (grabbingObject)
        {
            //dropInstructions.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Z))
            {

                grabbingObject = false;
                holdingKnife = false;
                
                objectBody.SetParent(null);

                if (handObject.GetVolume() <= 5f && handObject.GetObjectType() == 2)
                {
                    objectBody.position = grabPos.position;
                    objectBody.gameObject.SetActive(true);

                    grabPos.Find(objectBody.name).gameObject.SetActive(false);
                }


                objectBody.GetComponent<Rigidbody>().isKinematic = false;
                handObject = null;
                objectBody = null;

                carryingVal = 0f;

                animator.SetFloat(AnimationParameters.floats["carrying"], carryingVal);
                
            }
        }
        else
        {
            //dropInstructions.SetActive(false);
        }
    }

    public void Punch()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // change later
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
    }

    public void HoldKnife()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)&&!grabbingObject)
        {
            holdingKnife = !holdingKnife;
            grabbingObject = !grabbingObject;
            defaultKnife.SetActive(holdingKnife);
        }
    }

    public void Aim()
    {
        if (Input.GetKey(KeyCode.Mouse1) && holdingGun)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftShift))
            {
               // animator.SetBool(AnimationParameters.parameters["halfAiming"], true);
            }
            else
            {
               // animator.SetBool(AnimationParameters.parameters["halfAiming"], false);
                animator.SetBool(AnimationParameters.parameters["isAiming"], true);
            }
        }
        else
        {
            //animator.SetBool(AnimationParameters.parameters["halfAiming"], false);
            animator.SetBool(AnimationParameters.parameters["isAiming"], false);
        }
    }

    public void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && holdingGun)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftShift))
            {
                //animator.SetTrigger(AnimationParameters.triggers["halfShoot"]);
            }
            else
            {
                animator.SetTrigger(AnimationParameters.triggers["shoot"]);
            }
        }
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
                if (knifeVal < 1f)
                {
                    knifeVal += 0.2f;
                }
                if (carryingVal < 1f)
                {
                    carryingVal += 0.25f;
                }
                animator.SetFloat(AnimationParameters.floats["knife"], knifeVal);
                animator.SetFloat(AnimationParameters.floats["carrying"], carryingVal);
            }
            else if(handObject==null)
            {
                if (knifeVal > 0f)
                {
                    knifeVal -= 0.2f;
                }
                if (carryingVal > 0f)
                {
                    carryingVal -= 0.25f;
                }
                animator.SetFloat(AnimationParameters.floats["knife"], knifeVal);
                animator.SetFloat(AnimationParameters.floats["carrying"], carryingVal);
            }
        }
        else
        {
            animator.SetBool(AnimationParameters.parameters["isIdle"], false);
        }

    }

    public void CallGravity()
    {
        if (!character.isGrounded)
        {
            Vector3 fall = new Vector3(0f, gravity, 0f);
            character.Move(fall * Time.deltaTime);
            gravity -= (0.5f);
        }
        if (character.isGrounded && gravity < 0)
            gravity = -1f;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
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

    void OnAnimatorIK()
    {
        if (animator)
        {
            InitializeFootIKWeights();
            FootIKPlacement();
            //HeadIKTilt();
            //ArmPlacement();
        }
    }

    private void InitializeFootIKWeights()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
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
        if (moveDir.x == 0 && moveDir.y == 0 && moveDir.z == 0 && animator.GetBool("isIdle"))
        {
            if (headIKWeight < 0.7f)
                headIKWeight += 0.1f;
            animator.SetLookAtWeight(headIKWeight);
            animator.SetLookAtPosition(headTarget.position);
        }
        else
        {
            if (headIKWeight > 0f)
                headIKWeight -= 0.1f;
            animator.SetLookAtWeight(headIKWeight);
        }
    }

    private void ArmPlacement()
    {
        if (handObject != null && handObject.GetVolume() > 5)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, objectBody.position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, objectBody.position);
        }
    }

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
               // grabInstructions.SetActive(false);
                //DropObject();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            //grabInstructions.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
    }

}