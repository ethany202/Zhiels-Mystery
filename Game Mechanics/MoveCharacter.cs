using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveCharacter : MonoBehaviour
{

    public PhotonView PV;
    public CharacterController character;
    public TPSMouseLook viewObject;
    public Camera camObj;

    // Transforms:
    public Transform cam;
    public Transform body;
    public Transform headTarget;

    // Data Values:
    private float knifeVal = 0f;
    private float crouchingVal = 0f;
    private float velocityNormalized = 0f;

    // Data Values:
    private float baseSpeed = 3f;
    private float sprintSpeed = 4f;
    private float regJumpHeight = 1;
    private float sprintJumpHeight = 1;

    private float weight;
    private float strength;
    private float moveSpeed;
    private float currentJumpHeight;
    private float gravity;


    [Range(0, 1)] public float rightDistanceToGround;
    [Range(0, 1)] public float leftDistanceToGround;
    [Range(0, 1)] private float headIKWeight = 0;
    public LayerMask layerMask;

    // Animation:
    public Animator animator;

    private Vector3 moveDir;

    private bool isJumping;
    private bool holdingKnife;
    private bool holdingGun;
    private bool isCrouching;

    public GameObject defaultKnife;
    public GameObject bulletDecal;
    // public GameObject bulleteffect;

    void Start()
    {
        InitializePresets();

        moveSpeed = baseSpeed;
        currentJumpHeight = regJumpHeight;

        isJumping = false;
        holdingKnife = false;
        if (!PV.IsMine)
        {
            camObj.enabled = false;
        }
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
        if (PV.IsMine)
        {
            NormalMovement();         
            CheckCrouch();
            CheckJump();
            Punch();
            Kick();
            HoldKnife();
            CallGravity();
            Aim();
            Shoot();
            CheckIdle();
            viewObject.Rotate();
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
                if (!Input.GetKey(ControlsConstants.keys["crouch"]))
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

    public void CheckCrouch()
    {
        if (Input.GetKey(ControlsConstants.keys["crouch"]))
        {
            ResetAnimations("isCrouching");
            moveSpeed = baseSpeed / 2f;
            //character.center = new Vector3(0, 1.5f, 0);
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
                while (moveSpeed < sprintSpeed)
                {
                    moveSpeed += 0.2f;
                }
                currentJumpHeight = sprintJumpHeight;
                animator.SetFloat(AnimationParameters.floats["velocityNormalized"], 1f);
            }
        }
        if (!Input.GetKey(ControlsConstants.keys["sprint"]))
        {
            if (velocityNormalized > 0f)
            {
                velocityNormalized -= 0.1f;
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
        if (Input.GetKeyDown(ControlsConstants.keys["jump"]))
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
        if (!Input.GetKeyDown(ControlsConstants.keys["jump"]))
        {
            isJumping = false;
        }
    }

    public void Punch()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["attack"])) // change later
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
        if (Input.GetKeyDown(ControlsConstants.keys["kick"]))
        {
            animator.SetTrigger(AnimationParameters.triggers["kick"]);
            moveSpeed = 0;
        }
        if (!Input.GetKeyDown(ControlsConstants.keys["kick"]))
        {
            animator.ResetTrigger(AnimationParameters.triggers["kick"]);
            moveSpeed = baseSpeed;
        }
    }

    public void HoldKnife()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["knife"]))
        {
            holdingKnife = !holdingKnife;
            defaultKnife.SetActive(holdingKnife);
        }
    }

    public void Aim()
    {
        if (Input.GetKey(ControlsConstants.keys["scope"]) && holdingGun)
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
        if (Input.GetKeyDown(ControlsConstants.keys["attack"]) && holdingGun)
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
        /*if (Input.GetKeyDown(KeyCode.Mouse0) && holdingPistol)
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
                if (knifeVal < 1f)
                {
                    knifeVal += 0.1f;
                }
                animator.SetFloat(AnimationParameters.floats["knife"], knifeVal);
            }
            else
            {
                if (knifeVal > 0f)
                {
                    knifeVal -= 0.1f;
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        Vector3 pushDir = new Vector3(2 * moveSpeed * weight * hit.moveDirection.x, 9.8f * hit.moveDirection.y, 2 * moveSpeed * weight * hit.moveDirection.z);
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
            FootIKPlacement();
            HeadIKTilt();
            
        }
    }

    private void FootIKPlacement()
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
    }

    private void HeadIKTilt()
    {
        if (moveDir.x == 0 && moveDir.y == 0 && moveDir.z == 0 && animator.GetBool("isIdle"))
        {
            animator.SetLookAtWeight(0.75f);
            animator.SetLookAtPosition(headTarget.position);
        }
        else
        {
            animator.SetLookAtWeight(0f);
        }
    }

}