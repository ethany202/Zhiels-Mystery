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

    // Data Values:
    private float knifeVal = 0f;
    private float crouchingVal = 0f;
    private float velocityNormalized = 0f;

    private float baseSpeed = 3f;
    private float sprintSpeed = 4f;
    private float regJumpHeight = 1;
    private float sprintJumpHeight = 1;

    private float weight;
    private float strength;

    private float moveSpeed;
    private float currentJumpHeight;

    public float gravity;

    // Animation:
    public Animator animator;

    public int role; // 0 = hitmen, 1 = detective

    private Vector3 moveDir;

    private bool isJumping;
    private bool holdingKnife;
    private bool holdingGun = true;
    private bool isCrouching;

    private string[] inventoryItems;

    public GameObject defaultKnife, temporaryGun;

    [Range(0,1)]
    public float rightDistanceToGround;

    [Range(0, 1)]
    public float leftDistanceToGround;

    public LayerMask layerMask;
    // public GameObject bulleteffect;

    void Start()
    {
        //InitializePresets();
        viewObject.SetVision(1000);

        moveSpeed = baseSpeed;
        currentJumpHeight = regJumpHeight;

        isCrouching = false;
        isJumping = false;
        holdingKnife = false;
    }

    public void InitializePresets()
    {
        baseSpeed = CustomizedData.baseSpeed;
        sprintSpeed = baseSpeed * 2.5f;

        regJumpHeight = CustomizedData.regJumpHeight;
        sprintJumpHeight = CustomizedData.sprintJumpHeight;

        strength = CustomizedData.strength;
        weight = CustomizedData.weight;

        viewObject.SetVision(CustomizedData.vision);

        viewObject.SetSensitivity(CustomizedData.normalSensitivity);
    }

    void Update()
    {

        NormalMovement();
        CheckSprint();
        CheckCrouch();
        CheckJump();
        Punch();
        Kick();
        HoldKnife();
        CallGravity();
        Shoot();
        Aim();
        CheckIdle();
        viewObject.Rotate();
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
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    animator.SetBool(AnimationParameters.parameters["isMoving"], true);
                }
            }
            character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    public void CheckCrouch()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ResetAnimations("isCrouching");
            moveSpeed = baseSpeed / 2f;
            character.height = 2.35f;
            character.center = new Vector3(0, 1.5f, 0);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
            {
                if(crouchingVal < 1f)
                {
                    crouchingVal += 0.25f;
                }
                animator.SetFloat(AnimationParameters.floats["crouching"], crouchingVal);
                character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            }
            else
            {
                if(crouchingVal > 0f)
                {
                    crouchingVal -= 0.25f;
                }
                animator.SetFloat(AnimationParameters.floats["crouching"], crouchingVal);
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool(AnimationParameters.parameters["isCrouching"], false);
            moveSpeed = baseSpeed;
            character.height = 3.4f;
            character.center = new Vector3(0, 2f, 0);
        }
    }

    /*public void CheckCrouch()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ResetAnimations("isCrouching");
            moveSpeed = baseSpeed / 2f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
            {
                animator.SetBool(AnimationParameters.parameters["isCrouchWalking"], true);
                character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool(AnimationParameters.parameters["isCrouchWalking"], false);
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool(AnimationParameters.parameters["isCrouchWalking"], false);
            animator.SetBool(AnimationParameters.parameters["isCrouching"], false);
        }
    }*/

    private void CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.S))
                return;
            else
            {
                if(velocityNormalized < 1f)
                {
                    velocityNormalized += 0.25f;
                }
                while (moveSpeed < sprintSpeed)
                {
                    moveSpeed += 0.25f;
                }
                currentJumpHeight = sprintJumpHeight;
                animator.SetFloat(AnimationParameters.floats["velocityNormalized"], 1f);
            }
        }
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            if(velocityNormalized > 0f)
            {
                velocityNormalized -= 0.25f;
            }
            while (moveSpeed > baseSpeed)
            {
                moveSpeed -= 0.5f;
            }
            currentJumpHeight = regJumpHeight;
            animator.SetFloat(AnimationParameters.floats["velocityNormalized"], velocityNormalized);
        }
    }

    public void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                if (!Input.GetKey(KeyCode.S))
                {
                    animator.SetTrigger(AnimationParameters.triggers["jump"]);
                    //character.Move(new Vector3(0f, currentJumpHeight, 0f));
                    moveSpeed = 0;
                }
            }
        }
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = false;
        }
    }


    private void Slide()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.V))
            {
                character.height = 2.25f;
                character.center = new Vector3(0f, 3f, 0f);
                animator.SetTrigger(AnimationParameters.triggers["slide"]);
            }
            if (!Input.GetKey(KeyCode.V))
            {
                character.height = 3.4f;
                animator.ResetTrigger(AnimationParameters.triggers["slide"]);
                character.center = new Vector3(0f, 2f, 0f);
            }
        }

    }

    public void Punch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
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

    public void Kick()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetTrigger(AnimationParameters.triggers["kick"]);
            moveSpeed = 0;
        }
        if (!Input.GetKeyDown(KeyCode.C))
        {
            animator.ResetTrigger(AnimationParameters.triggers["kick"]);
            moveSpeed = baseSpeed;
        }
    }

    public void HoldKnife()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            holdingKnife = !holdingKnife;
            //animator.SetBool(AnimationParameters.parameters["holdingKnife"], true);
            defaultKnife.SetActive(holdingKnife);
        }
    }

    public void Aim()
    {
        if (Input.GetKey(KeyCode.Mouse1) && holdingGun)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool(AnimationParameters.parameters["halfAiming"], true);
            }
            else
            {
                animator.SetBool(AnimationParameters.parameters["halfAiming"], false);
                animator.SetBool(AnimationParameters.parameters["isAiming"], true);
            }
        }
        else
        {
            animator.SetBool(AnimationParameters.parameters["halfAiming"], false);
            animator.SetBool(AnimationParameters.parameters["isAiming"], false);
        }
    }

    public void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            holdingGun = !holdingGun;
            temporaryGun.SetActive(holdingGun);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && holdingGun)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetTrigger(AnimationParameters.triggers["halfShoot"]);
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
                if(knifeVal < 1f)
                {
                    knifeVal += 0.25f;
                }
                animator.SetFloat(AnimationParameters.floats["knife"], knifeVal);
            }
            else
            {
                if(knifeVal > 0f)
                {
                    knifeVal -= 0.25f;
                }
                animator.SetFloat(AnimationParameters.floats["knife"], knifeVal);
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        Vector3 pushDir = new Vector3(2 * moveSpeed * weight * hit.moveDirection.x, 9.8f * hit.moveDirection.y, 2 * moveSpeed * weight * hit.moveDirection.z);
        body.AddForceAtPosition(pushDir / Mathf.Sqrt(body.mass), hit.point);

    }

    private void PlayAnimationClip(string name)
    {
        if (!animator.GetBool(AnimationParameters.parameters[name]))
        {
            animator.SetBool(AnimationParameters.parameters[name], true);
        }
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

    public Transform rightObj;
    public Transform lookOBJ;

    void OnAnimatorIK()
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

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

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

            animator.SetIKPosition(AvatarIKGoal.RightHand, rightObj.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightObj.rotation);

            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(lookOBJ.position);


        }
    }
}
/*using System.Collections;
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

    // Data Values:
    private float baseSpeed = 3f;
    private float sprintSpeed = 4f;
    private float regJumpHeight = 1;
    private float sprintJumpHeight = 1;

    private float flexibility;
    private float weight;
    private float strength;

    private float moveSpeed;
    private float currentJumpHeight;

    public float gravity;

    // Animation:
    public Animator animator;

    public int role; // 0 = hitmen, 1 = detective

    private Vector3 moveDir;

    private bool isJumping;
    private bool holdingKnife;
    private bool holdingGun = false;
    private bool isCrouching;

    private string[] inventoryItems;

    public GameObject defaultKnife;

    public GameObject bulletDecal;
    // public GameObject bulleteffect;

    void Start()
    {
        //InitializePresets();
        viewObject.SetVision(1000);

        moveSpeed = baseSpeed;
        currentJumpHeight = regJumpHeight;

        isCrouching = false;
        isJumping = false;
        holdingKnife = false;
    }

    public void InitializePresets()
    {
        baseSpeed = CustomizedData.baseSpeed;
        sprintSpeed = baseSpeed * 2.5f;

        regJumpHeight = CustomizedData.regJumpHeight;
        sprintJumpHeight = CustomizedData.sprintJumpHeight;

        flexibility = CustomizedData.flexibility;
        strength = CustomizedData.strength;
        weight = CustomizedData.weight;

        viewObject.SetVision(CustomizedData.vision);

        viewObject.SetSensitivity(CustomizedData.normalSensitivity);
    }

    void Update()
    {

            NormalMovement();
            CheckSprint();
            CheckCrouch();
            CheckJump();
            Punch();
            Kick();
            HoldKnife();
            CallGravity();
            //Slide();
            // Shoot and aim method()
            Aim();
            Shoot();
            //CheckTriggerPlaying();
            CheckAnimationPlaying();
        CheckIdle();
        viewObject.Rotate();
    }

    public bool CheckAnimationPlaying()
    {
        for(int i = 0; i < AnimationParameters.triggerClips.Length; i++)
        {
            if(animator.GetCurrentAnimatorStateInfo(1).IsName(AnimationParameters.triggerClips[i]) && animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1f)
            {
                return true;
                Debug.Log(AnimationParameters.triggerClips[i]);
            }
        }
        return false;
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
                //PlayAnimationClip("isWalkingBackwards");
                animator.SetBool(AnimationParameters.parameters["isWalkingBackwards"], true);
                moveSpeed = baseSpeed / 1.15f;
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    //PlayAnimationClip("isWalking");
                    //animator.SetTrigger("walk");
                    animator.SetBool(AnimationParameters.parameters["isWalking"], true);
                }
                //  animator.SetBool(AnimationParameters.parameters["isWalking"], true);
            }
            character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

     public void CheckCrouch()
     {
         if (Input.GetKey(KeyCode.LeftShift))
         {
             ResetAnimations("isCrouching");
             //PlayAnimationClip("isCrouching");
             moveSpeed = baseSpeed / 2f;
             character.height = 2.35f;
             character.center = new Vector3(0, 1.5f, 0);
             if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
             {
                 //animator.SetBool(AnimationParameters.parameters["isCrouchWalking"], true);
                 //PlayAnimationClip("isCrouchWalking");
                 ResetAnimations("isCrouchWalking");
                 character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
             }
             else
             {
                 animator.SetBool(AnimationParameters.parameters["isCrouchWalking"], false);
             }
         }
         if (!Input.GetKey(KeyCode.LeftShift))
         {
             animator.SetBool(AnimationParameters.parameters["isCrouching"], false);
             moveSpeed = baseSpeed;
             character.height = 3.49f;
             character.center = new Vector3(0, 2f, 0);
         }
     }

    public void CheckCrouch()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ResetAnimations("isCrouching");
            moveSpeed = baseSpeed / 2f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
            {
                animator.SetBool(AnimationParameters.parameters["isCrouchWalking"], true);
                character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool(AnimationParameters.parameters["isCrouchWalking"], false);
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool(AnimationParameters.parameters["isCrouchWalking"], false);
            animator.SetBool(AnimationParameters.parameters["isCrouching"], false);
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
                animator.SetBool(AnimationParameters.parameters["isRunning"], true);
                while (moveSpeed < sprintSpeed)
                {
                    moveSpeed += 0.25f;
                }
                currentJumpHeight = sprintJumpHeight;
            }
        }
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetBool(AnimationParameters.parameters["isRunning"], false);
            while (moveSpeed > baseSpeed)
            {
                moveSpeed -= 0.5f;
            }
            currentJumpHeight = regJumpHeight;
        }
    }

    public void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                if (!Input.GetKey(KeyCode.S))
                {
                    animator.SetTrigger(AnimationParameters.triggers["jump"]);
                    //character.Move(new Vector3(0f, currentJumpHeight, 0f));
                    moveSpeed = 0;
                }
            }
        }
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = false;
        }
    }


    private void Slide()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.V))
            {
                character.height = 2.25f;
                character.center = new Vector3(0f, 3f, 0f);
                animator.SetTrigger(AnimationParameters.triggers["slide"]);
            }
            if (!Input.GetKey(KeyCode.V))
            {
                character.height = 3.49f;
                animator.ResetTrigger(AnimationParameters.triggers["slide"]);
                character.center = new Vector3(0f, 2f, 0f);
            }
        }
            
    }

    public void Punch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
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
        if (!Input.GetKeyDown(KeyCode.Q))
        {
            animator.ResetTrigger(AnimationParameters.triggers["idlePunch"]);
            animator.ResetTrigger(AnimationParameters.triggers["punch"]);
        }
    }

    public void Kick()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetTrigger(AnimationParameters.triggers["kick"]);
            moveSpeed = 0;
        }
        if (!Input.GetKeyDown(KeyCode.C))
        {
            animator.ResetTrigger(AnimationParameters.triggers["kick"]);
            moveSpeed = baseSpeed;
        }
    }

    public void HoldKnife()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            holdingKnife = !holdingKnife;
            //ResetAnimations("holdingKnife");
            animator.SetBool(AnimationParameters.parameters["holdingKnife"], true);
            defaultKnife.SetActive(holdingKnife);
        }
    }

    public void Aim()
    {
        if(Input.GetKey(KeyCode.Mouse1) && holdingGun)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                animator.SetBool(AnimationParameters.parameters["halfAiming"], false);
                animator.SetBool(AnimationParameters.parameters["isAiming"], false);
            }
            else
            {
                animator.SetBool(AnimationParameters.parameters["halfAiming"], false);
                animator.SetBool(AnimationParameters.parameters["isAiming"], true);
                //ResetAnimations("isAiming");
            }
        }
        else
        {
            animator.SetBool(AnimationParameters.parameters["halfAiming"], false);
            animator.SetBool(AnimationParameters.parameters["isAiming"], false);
        }
    }

    public void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && holdingGun)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                animator.SetTrigger(AnimationParameters.triggers["halfShoot"]);
            }
            else
            {
                //animator.SetBool("isShooting", true);
                animator.SetTrigger(AnimationParameters.triggers["shoot"]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && holdingPistol)
        {
            bool hasProperGun = false;
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i] == gunId)
                {
                    hasProperGun = true;
                    break;
                }
            }
            if (hasProperGun)
            {
                animator.SetTrigger(AnimationParameters.parameters[gunId]);
                /*RaycastHit hit;
                 * if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hit, range)
                 * {
                 * Debug.Log(hit.transform.name);
                 * Instantiate(bulletEffect, hit.point, Quaternion.LookRotation(hit.normal));
                 *Instantiate(bulletDecal, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                 *
                 *
                 * }
                 
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && holdingGun)
        {
            animator.ResetTrigger(AnimationParameters.parameters[gunId]);
        }
    }

    public void CheckIdle()
    {
        
        if (!Input.anyKey)
        {
            ResetTriggers();
            ResetAnimations();
            if (holdingKnife)
            {
                animator.SetBool(AnimationParameters.parameters["holdingKnife"], true);
                //PlayAnimationClip("holdingKnife");
            }
            else
            {
                animator.SetBool(AnimationParameters.parameters["isIdle"], true);
                //PlayAnimationClip("isIdle");
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        Vector3 pushDir = new Vector3(2 * moveSpeed * weight * hit.moveDirection.x, 9.8f * hit.moveDirection.y, 2 * moveSpeed * weight * hit.moveDirection.z);
        body.AddForceAtPosition(pushDir / Mathf.Sqrt(body.mass), hit.point);

    }

    private void PlayAnimationClip(string name)
    {
        if (!animator.GetBool(AnimationParameters.parameters[name]))
        {
            animator.SetBool(AnimationParameters.parameters[name], true);
        }
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

    
}*/

/*using System.Collections;
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

    // Data Values:
    private float baseSpeed = 3f;
    private float sprintSpeed = 4f;
    private float regJumpHeight = 1;
    private float sprintJumpHeight = 1;


    private float flexibility;
    private float weight;
    private float strength;

    private float moveSpeed;
    private float currentJumpHeight;

    public float gravity;

    // Animation:
    public Animator animator;

    public int role; // 0 = hitmen, 1 = detective

    private Vector3 moveDir;

    private bool isJumping;
    private bool holdingKnife;
    private bool holdingGun;
    private bool isCrouching;

    private string[] inventoryItems;

    public GameObject defaultKnife;

    public GameObject bulletDecal;
    // public GameObject bulleteffect;

    void Start()
    {
        //InitializePresets();
        viewObject.SetVision(1000);

        moveSpeed = baseSpeed;
        currentJumpHeight = regJumpHeight;


        isJumping = false;
        holdingKnife = false;
    }

    public void InitializePresets()
    {
        baseSpeed = CustomizedData.baseSpeed;
        sprintSpeed = baseSpeed * 2.5f;

        regJumpHeight = CustomizedData.regJumpHeight;
        sprintJumpHeight = CustomizedData.sprintJumpHeight;

        flexibility = CustomizedData.flexibility;
        strength = CustomizedData.strength;
        weight = CustomizedData.weight;

        viewObject.SetVision(CustomizedData.vision);

        viewObject.SetSensitivity(CustomizedData.normalSensitivity);
    }

    void Update()
    {

            viewObject.Rotate();
            BasicMovement();
            CheckSprint();
            CallGravity();
            CallJump();
            Punch();
            Kick();
            CheckIdle();
            Crouch();
            HoldKnife();
    }



    public void BasicMovement()
    {

            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                if (vertical < 0f)
                {
                    animator.SetBool(AnimationParameters.parameters["isWalkingBackwards"], true);
                    moveSpeed = baseSpeed / 1.15f;
                }
                else
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        moveSpeed = baseSpeed;
                    }
                    animator.SetBool(AnimationParameters.parameters["isWalking"], true);
                }
                if (character.isGrounded)
                {
                    character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
                }
                if (!character.isGrounded)
                {
                    if (moveSpeed == baseSpeed)
                        character.Move(moveDir.normalized * (moveSpeed / 3f) * Time.deltaTime);
                    if (moveSpeed == sprintSpeed)
                        character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                ResetAnimations("isIdle");
            }
        
    }

    private void CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.S))
            {
                return;
            }
            else
            {
                animator.SetBool(AnimationParameters.parameters["isRunning"], true);
                while (moveSpeed < sprintSpeed)
                {
                    moveSpeed += 0.25f;
                }
                currentJumpHeight = sprintJumpHeight;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetBool(AnimationParameters.parameters["isRunning"], false);
            while (moveSpeed > baseSpeed)
            {
                moveSpeed -= 0.5f;
            }
            currentJumpHeight = regJumpHeight;
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

    public void CallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                if (!Input.GetKey(KeyCode.S))
                {
                    animator.SetTrigger(AnimationParameters.triggers["jump"]);
                    //character.Move(new Vector3(0f, currentJumpHeight, 0f));
                    moveSpeed = 0;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            animator.ResetTrigger(AnimationParameters.triggers["jump"]);
        }
    }

    public void Punch()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Debug.Log("Not punching");
            animator.ResetTrigger(AnimationParameters.triggers["idlePunch"]);
            animator.ResetTrigger(AnimationParameters.triggers["punch"]);
            ResetTriggers();
        }
    }

    public void Kick()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetAnimations();
            animator.SetTrigger(AnimationParameters.triggers["kick"]);
            moveSpeed = 0;

        }
    }

    public void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            character.height = 2.35f;
            character.center = new Vector3(0, 1.5f, 0);
            moveSpeed = baseSpeed / 2f;
            animator.SetBool(AnimationParameters.parameters["isCrouching"], true);
            isCrouching = true;
            //ResetAnimations("isCrouching");
            if (Input.GetKey(KeyCode.W))
            {
                //animator.SetBool(AnimationParameters.parameters["isCrouchWalking"], true);
                ResetAnimations("isCrouchWalking");
            }
            else if (Input.GetKey(KeyCode.A))
            {
                //animator.SetBool(AnimationParameters.parameters["leftCrouchWalking"], true);
                ResetAnimations("leftCrouchWalking");
            }
            else if (Input.GetKey(KeyCode.S))
            {
                //animator.SetBool(AnimationParameters.parameters["backCrouchWalking"], true);
                ResetAnimations("backCrouchWalking");
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //animator.SetBool(AnimationParameters.parameters["rightCrouchWalking"], true);
                ResetAnimations("rightCrouchWalking");
            }
            else
            {
                animator.SetBool(AnimationParameters.parameters["leftCrouchWalking"], false);
                animator.SetBool(AnimationParameters.parameters["backCrouchWalking"], false);
                animator.SetBool(AnimationParameters.parameters["rightCrouchWalking"], false);
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool(AnimationParameters.parameters["isCrouching"], false);
            //ResetAnimations();
            moveSpeed = baseSpeed;
            character.height = 3.516f;
            character.center = new Vector3(0, 2f, 0);
            isCrouching = false;
        }
    }

    public void HoldKnife()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            holdingKnife = !holdingKnife;
            animator.SetBool(AnimationParameters.parameters["holdingKnife"], holdingKnife);
            defaultKnife.SetActive(holdingKnife);
        }
    }

    public void Shoot(string gunId)
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && holdingGun)
        {
            bool hasProperGun = false;
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i] == gunId)
                {
                    hasProperGun = true;
                    break;
                }
            }
            if (hasProperGun)
            {
                animator.SetTrigger(AnimationParameters.parameters[gunId]);
                //RaycastHit hit;
                 //if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hit, range)
                 //{
                 //Debug.Log(hit.transform.name);
                 //Instantiate(bulletEffect, hit.point, Quaternion.LookRotation(hit.normal));
                 //Instantiate(bulletDecal, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                
                 
                 //}
                 
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && holdingGun)
        {
            animator.ResetTrigger(AnimationParameters.parameters[gunId]);
        }
    }

    public void CheckIdle()
    {
        if (!Input.anyKey)
        {
            ResetTriggers();
            ResetAnimations("isIdle");
            if (holdingKnife)
            {
                animator.SetBool(AnimationParameters.parameters["holdingKnife"], true);
            }
        }
        else
        {
            animator.SetBool(AnimationParameters.parameters["isIdle"], false);
        }
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        Vector3 pushDir = new Vector3(2 * moveSpeed * weight * hit.moveDirection.x, 9.8f * hit.moveDirection.y, 2 * moveSpeed * weight * hit.moveDirection.z);
        body.AddForceAtPosition(pushDir / Mathf.Sqrt(body.mass), hit.point);

    }

}*/
