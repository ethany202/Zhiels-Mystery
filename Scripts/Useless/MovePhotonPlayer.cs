using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovePhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    private CharacterController character;
    public Transform cam;
    public TPSMouseLook viewObject;
    public Camera camObj;

    public float baseSpeed;
    public float moveSpeed;
    public float sprintSpeed;
    public float gravity;
    public float currentJumpHeight;
    public float regJumpHeight;
    public float sprintJumpHeight;

    public Animator animator;
    private string[] animations = { "isSprinting", "isWalking", "isJumping", "isRunningBackwards" };

    public int role; // 0 = hitmen, 1 = detective

    void Start()
    {
        PV = GetComponent<PhotonView>();
        character = GetComponent<CharacterController>();
        // viewObject = GetComponentInChildren<TPSMouseLook>();
        if (!PV.IsMine)
        {
            camObj.enabled = false;
        }
    }

    void Update()
    {
        if (PV.IsMine)
        {
            CallGravity();
            BasicMovement();
            CallJump(currentJumpHeight);
            viewObject.Rotate();
        }
    }

    public void BasicMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (vertical < 0f)
            {
                ResetAnimations("isRunningBackwards");
                moveSpeed = baseSpeed / 1.5f;
            }
            else
            {
                ResetAnimations("isWalking");
                moveSpeed = baseSpeed;
            }
            CheckSprint();
            CallJump(currentJumpHeight);
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
            character.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            ResetAnimations();
        }
    }

    private void CheckSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.S))
                ResetAnimations("isRunningBackwards");
            else
            {
                animator.SetBool("isSprinting", true);
                while (moveSpeed < sprintSpeed)
                {
                    moveSpeed += 0.5f;
                }
                currentJumpHeight = sprintJumpHeight;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isSprinting", false);
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
            gravity -= (0.0005f);
        }
        if (character.isGrounded && gravity < 0)
            gravity = 0;
    }

    public void CallJump(float height)
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.isGrounded)
        {
            animator.SetBool("isJumping", true);
            CheckJump(height);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    public void CheckJump(float height)
    {
        for (int i = 0; i < 15; i++)
        {
            Jump(height);
            height += (i / 5);
        }
    }

    public void Jump(float jumpHeight)
    {
        character.Move(new Vector3(0f, jumpHeight, 0f) * 0.01f);
    }

    private void ResetAnimations(string exception)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i] != exception) animator.SetBool(animations[i], false);
            else animator.SetBool(animations[i], true);
        }
    }
    private void ResetAnimations()
    {
        for (int i = 0; i < animations.Length; i++)
        {
            animator.SetBool(animations[i], false);
        }
    }
}
