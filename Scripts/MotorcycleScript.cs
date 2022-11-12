using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleScript : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentbrakeForce;
    private float currentsteerAngle;
    private bool isBreaking;

    private float initialVelocity = 0.0f;
    private float finalVelocity = 0.7f;
    private float currentVelocity = 0.0f;
    private float accelerationRate = 0.21f;
    private float decelerationRate = 0.2f;

    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteeringAngle;

    [SerializeField] public WheelCollider frontCollider;
    [SerializeField] public WheelCollider backCollider;


    [SerializeField] private Transform frontTransform;
    [SerializeField] private Transform backTransform;


    /*
    public ParticleSystem spark;
    public ParticleSystem blood;
    public bool IsCollidingWithHuman;
    public bool IsCollidingWithObstacle;
    */


    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }


    private void HandleMotor()
    {

        frontCollider.motorTorque = verticalInput * motorForce;


        currentbrakeForce = isBreaking ? 4000f : 0f;
        frontCollider.brakeTorque = currentbrakeForce;
        backCollider.brakeTorque = currentbrakeForce;

        accelerateCar();
    }


    private void accelerateCar()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            currentVelocity = currentVelocity + (accelerationRate * Time.deltaTime);
        }
        else
        {
            currentVelocity = currentVelocity - (decelerationRate * Time.deltaTime);
        }
        currentVelocity = Mathf.Clamp(currentVelocity, initialVelocity, finalVelocity);
        transform.Translate(0, 0, currentVelocity);
    }

    private void HandleSteering()
    {
        currentsteerAngle = maxSteeringAngle * horizontalInput;
        frontCollider.steerAngle = currentsteerAngle;
        
    }


    private void UpdateWheels()
    {
        UpdateSingleWheel(frontCollider, frontTransform);
        UpdateSingleWheel(backCollider, backTransform);
        
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    /*
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Obstacle" && other.contacts.Length != 0)
        {
            for (int i = 0; i < other.contacts.Length; i++)
            {
                GameObject impactSpark = Instantiate(spark.gameObject, other.contacts[i].point, Quaternion.identity);
                Destroy(impactSpark, 2f);
            }
            IsCollidingWithObstacle = true;
        }
        if (other.collider.tag == "Human" && other.contacts.Length != 0)
        {
            for (int j = 0; j < other.contacts.Length; j++)
            {
                GameObject impactGO = Instantiate(blood.gameObject, other.contacts[j].point, Quaternion.identity);
                Destroy(impactGO, 2f);
            }
            IsCollidingWithHuman = true;
        }
    */
    }
