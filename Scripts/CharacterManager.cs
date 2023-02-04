using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{

    [Header("Character Meta Data")]
    public CharacterController character;
    public Transform cam;
    public TPSMouseLook viewObject;
    public Animator anim;
    public Transform body;
    public Transform grabPos;

    [Header("Player-Specific UI")]
    public Image bloodFrame;
    public Image bloodSplatter;
    public Image deathFade;
    public GameObject blackScreen;

    // Speed Types
    [Header("Primitive Attributes")]
    public float baseSpeed = 1f;
    public float sprintSpeed = 1.75f;
    public float crouchSpeed = 0.65f;
    public float slideSpeed = 2.65f;
    public float currentSpeed;
    private Vector3 moveDir;

    private float characterHeight;
    private float stepHeight;
    private float gravity = 0f;

    private bool isSliding = false;
    private bool isCrouching = false;
    public bool isMoving = false;

    private float health = 100f;
    public float Health { get => health; set => DisplayPlayerHealth(value); }


    // -50

    private void Awake()
    {
        LoadSceneLogic.player = this;
        LoadSaveState();       
    }

    private void Start()
    {
        characterHeight = character.height;
        stepHeight = character.stepOffset;

        currentSpeed = baseSpeed;

        if (ControlsConstants.keys.Count == 0)
        {
            ControlsConstants.SetDefaultKeys();
        }
        InvokeRepeating("RegenerateHealth", 0f, 2f);
    }

    private void Update()
    {
        Walk();
        Crouch();

        viewObject.Rotate();
        AddGravity();
    }

    private void Walk()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.25f)
        {
            isMoving = true;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            InitiateWalkSpeed();
            Sprint();
            Crouch();

            if (isSliding)
            {
                currentSpeed = slideSpeed;
            }
            else if (isCrouching)
            {
                currentSpeed = crouchSpeed;
            }
            else
            {

            }
            character.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            ResetAnimations();
        }
    }

    private void InitiateWalkSpeed()
    {
        currentSpeed = baseSpeed;
    }

    private void Sprint()
    {
        if (Input.GetKey(ControlsConstants.keys["sprint"]))
        {
            anim.SetBool("isSprinting", true);
            currentSpeed = sprintSpeed;
            Slide();
        }
        else
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isSprinting", false);
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["crouch"]))
        {
            character.height = characterHeight / 3;
            character.stepOffset = 0;

            isCrouching = true;
            anim.SetBool("isCrouching", true);
        }
        if (Input.GetKeyUp(ControlsConstants.keys["crouch"]))
        {
            character.height = characterHeight;
            character.stepOffset = stepHeight;

            isCrouching = false;
            anim.SetBool("isCrouching", false);
        }
    }

    private void Slide()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["slide"]))
        {
            StartCoroutine(CallSliding());
        }
    }

    private IEnumerator CallSliding()
    {
        isSliding = true;
        anim.SetBool("isSliding", true);

        character.height = characterHeight / 3;

        yield return new WaitForSecondsRealtime(0.6f);
        character.height = characterHeight;

        isSliding = false;
        anim.SetBool("isSliding", false);
    }

    private void AddGravity()
    {
        if (character.isGrounded)
        {
            gravity = 0f;
        }
        else
        {
            character.Move(new Vector3(0f, gravity, 0f));
            gravity -= (0.2f * Time.deltaTime);
        }

        // Check if player fell thru map
        if(transform.position.y <= -200f)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ResetAnimations()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isSprinting", false);
        anim.SetBool("isCrouching", false);
    }


    public void LoadSaveState()
    {
        if(LoadSceneLogic.savedGame && SceneManager.GetActiveScene().buildIndex == LoadSceneLogic.examPhase)
        {
            SaveData data = SaveSystem.LoadPlayerState();
            Vector3 position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);

            transform.position = position;
            Health = data.playerHealth;
        }
    }

    private void DisplayPlayerHealth(float healthInput)
    {
        if (healthInput <= 0f)
        {
            healthInput = 0f;
            blackScreen.SetActive(true);

            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        this.health = healthInput;
        DamageFadeIn(this.health);
    }

    private void DamageFadeIn(float currentHealth)
    {
        float alphaCode = 1f - (currentHealth / 100f);

        Color splatterAlpha = bloodSplatter.color;
        splatterAlpha.a = alphaCode / 10f;
        bloodSplatter.color = splatterAlpha;

        Color frameAlpha = bloodFrame.color;
        frameAlpha.a = alphaCode / 3f;
        bloodFrame.color = frameAlpha;

        Color deathAlpha = deathFade.color;
        deathAlpha.a = alphaCode;
        deathFade.color = deathAlpha;
    }

    private void RegenerateHealth()
    {
        if (Health < 100f)
        {
            Health = Mathf.Min(Health + 2f, 100f);
        }
    }
}
