using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject self;
    public CharacterController player;
    public Transform cam;
    public Animator animator;

    string[] animations = { "isJumping", "isSprinting", "isWalking", "isRunningBackwards" };
    public float currentHealth = 100f;
    public float mass = 75f;
    public float baseSpeed = 1f;
    public float sprintSpeed = 2f;
    public float speed = 1f;
    public float gravity = -0.05f;
    public float currentJumpHeight = 1f;
    public float regJumpHeight = 1f;
    public float sprintJumpHeight = 2f;
    public float flexibility = 2f;
    float regOffset;

    // bool isSitting;

    public LayerMask groundMask;
    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.2f;


    void Start()
    {
        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        regOffset = player.stepOffset;
    }


    void Update()
    {
        CallGravity();
        Move();
        CallJump(currentJumpHeight);
        CallClimb();
    }

    public void Move()
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
                speed = baseSpeed / 1.5f;
            }
            else
            {
                ResetAnimations("isWalking");
                speed = baseSpeed;
            }
            CheckSprint();
            CheckGrounded();
            if (isGrounded) player.Move(moveDir.normalized * speed * Time.deltaTime);
            if (!isGrounded)
            {
                if (speed == baseSpeed)
                    player.Move(moveDir.normalized * (speed / 3f) * Time.deltaTime);
                if (speed == sprintSpeed)
                    player.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }
        else
            ResetAnimations();
    }

    private void CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.S))
                ResetAnimations("isRunningBackwards");
            else
            {
                animator.SetBool("isSprinting", true);
                while (speed < sprintSpeed)
                {
                    speed += 0.5f;
                }
                currentJumpHeight = sprintJumpHeight;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetBool("isSprinting", false);
            while (speed > baseSpeed)
            {
                speed -= 0.5f;
            }
            currentJumpHeight = regJumpHeight;
        }
    }

    public void CheckGrounded()
    {
        this.isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    public void CallGravity()
    {
        CheckGrounded();
        if (!isGrounded)
        {
            Vector3 fall = new Vector3(0f, gravity, 0f);
            player.Move(fall * Time.deltaTime);
            gravity -= (0.001f);
        }
        if (isGrounded && gravity < 0)
            gravity = -0.005f;
    }

    public void CallJump(float height)
    {
        CheckGrounded();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
            height += (i/5);
        }
    }

    public void Jump(float jumpHeight)
    {
        player.Move(new Vector3(0f, jumpHeight, 0f) * 0.01f);
    }

    public void Crouch(float flexibility)
    {
        // Will be implemented in later version
        // Must have a crouch animation to be coded; currently, no crouch animation is part of the model
    }

    public float GetHealth()
    {
        return this.currentHealth;
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
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        Vector3 pushDir = new Vector3(2 * speed * mass * hit.moveDirection.x, 9.8f * hit.moveDirection.y, 2 * speed * mass * hit.moveDirection.z);
        body.AddForceAtPosition(pushDir / Mathf.Sqrt(body.mass), hit.point);

    }

    private void CallClimb()
    {
    	if(Input.GetKey(KeyCode.C))
    	{
    		if(player.stepOffset != (flexibility / 1.5f) * regOffset)
    			player.stepOffset = (flexibility / 1.5f) * regOffset;
    	}
    	if(Input.GetKeyUp(KeyCode.C))
    		player.stepOffset = regOffset;
    }

    public void DisableScript()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }
    public void EnableScript()
    {
        GetComponent<PlayerMovement>().enabled = true;
    }
}

