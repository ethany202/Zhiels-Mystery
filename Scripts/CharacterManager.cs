using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{

    private const float WALK_TIME_INTERVAL = 1.5f;
    private const float SPRINT_TIME_INTERVAL = 1f;

    [Header("Character Meta Data")]
    public CharacterController character;
    public Transform cam;
    public TPSMouseLook viewObject;
    public Animator anim;
    public Transform body;
    public Transform bodyTarget;
    public Transform grabPos;

    [Header("FPS Objects")]
    public GameObject rifleArms;
    public GameObject pistolArms;
    public GameObject syringeArms;

    // Speed Types
    private float baseSpeed = 1f;
    private float sprintSpeed = 2f;
    private float crouchSpeed = 0.65f;
    public float currentSpeed;
    private Vector3 moveDir;

    private float characterHeight;
    private float stepHeight;
    private float gravity = 0f;

    [SerializeField] private float slideSpeed = 2.5f;
    [SerializeField] private bool isSliding = false;
    private bool isCrouching = false;

    private float health=100f;
    public float Health { get => health; set => DisplayPlayerHealth(value); }
    public int syringeCount=0;

    private bool grabbingObject;
    private Transform objectBody;

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
    }

    private void Update()
    {
        Walk();
        Crouch();

        viewObject.Rotate();
        AddGravity();

        UseSyringe();
        DropObject();
    }

    private void Walk()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.25f)
        {
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
                //currentSpeed = baseSpeed;
            }
            character.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            //character.Move(moveDir.normalized * -(currentSpeed/2) * Time.deltaTime);
            //moveDir = Vector3.zero;
            ResetAnimations();
        }
    }

    private void InitiateWalkSpeed()
    {
        currentSpeed = baseSpeed;
    }

    private void InitiateCrouchSpeed()
    {
        currentSpeed = baseSpeed / 2f;
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            character.height = characterHeight / 3;
            character.stepOffset = 0;

            //InitiateCrouchSpeed();
            isCrouching = true;
            anim.SetBool("isCrouching", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            character.height = characterHeight;
            character.stepOffset = stepHeight;

            //InitiateWalkSpeed();
            isCrouching = false;
            anim.SetBool("isCrouching", false);
        }
    }

    private void Jump()
    {

    }

    private void Slide()
    {
        if (Input.GetKeyDown(KeyCode.F))
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

    private void UseSyringe()
    {
        if (syringeCount == 0) { return; }

        if (Input.GetKeyDown(KeyCode.Alpha4)) 
        {
            syringeArms.SetActive(true);
        }
    }

    public void SyringeInjected()
    {
        Health += 40f;
        if (Health > 100f)
        {
            Health = 100f;
        }

        syringeCount--;
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
    }

    private void ResetAnimations()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isSprinting", false);
        anim.SetBool("isCrouching", false);
    }

    //private void CheckIdle()
    //{
    //    if (!Input.anyKey)
    //    {
    //        ResetAnimations();
    //    }
    //}

    private void GrabObject()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["grab"]))
        {
            if (objectBody.gameObject.layer == LayerMask.NameToLayer("Syringe"))
            {
                LoadSceneLogic.DisplayInstructions(false);

                objectBody.gameObject.SetActive(false);
                syringeCount++;
                return;
            }

            UpdatePosition();
            grabbingObject = true;
        }
    }


    private void UpdatePosition()
    {
        if (objectBody != null)
        {
            if (objectBody.gameObject.layer == LayerMask.NameToLayer("Holdable Object"))
            {
                PhysicalKeyProperties playerKeyCopy = cam.GetComponentInChildren<PhysicalKeyProperties>(true);
                playerKeyCopy.gameObject.SetActive(true);
                playerKeyCopy.SetKeyID(objectBody.GetComponent<PhysicalKeyProperties>().GetKeyID());
            }
            if(objectBody.gameObject.layer== LayerMask.NameToLayer("Rifle"))
            {
                rifleArms.SetActive(true);
            }
            if (objectBody.gameObject.layer == LayerMask.NameToLayer("Pistol"))
            {
                pistolArms.SetActive(true);
            }
            objectBody.gameObject.SetActive(false);
            LoadSceneLogic.DisplayInstructions(false);
        }
    }

    public void LoadSaveState()
    {
        if(LoadSceneLogic.savedGame)
        {
            SaveData data = SaveSystem.LoadPlayerState();
            Vector3 position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);

            transform.position = position;
            Health = data.playerHealth;
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

                rifleArms.SetActive(false);
                //meleeArms.SetActive(false);
                pistolArms.SetActive(false);
                
                cam.GetComponentInChildren<PhysicalKeyProperties>(true).gameObject.SetActive(false);
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

    private void DisplayPlayerHealth(float healthInput)
    {
        if (healthInput <= 0f)
        {
            // Call Restart level method;
            healthInput = 0f;
        }

        this.health = healthInput;
        LoadSceneLogic.GetHealthText().GetComponent<TMP_Text>().text = this.health + "";
    }
}
