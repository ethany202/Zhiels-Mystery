using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    
    private float horizontalInput;
    private float verticalInput;
    private float currentbrakeForce;
    private float currentsteerAngle;
    private bool isBreaking;

    private float initialVelocity = 0.0f;
    private float finalVelocity = 0.65f;
    private float currentVelocity = 0.0f;
    private float accelerationRate = 0.15f;
    private float decelerationRate = 0.3f;

    [SerializeField] private float motorForce = 1000;
    [SerializeField] private float maxSteeringAngle = 20;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    //public ParticleSystem spark;



    void FixedUpdate()
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
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;


        currentbrakeForce = isBreaking ? 4050f : 0f;
        frontLeftWheelCollider.brakeTorque = currentbrakeForce;
        frontRightWheelCollider.brakeTorque = currentbrakeForce;
        rearLeftWheelCollider.brakeTorque = currentbrakeForce;
        rearRightWheelCollider.brakeTorque = currentbrakeForce;
        accelerateCar();
    }


    private void accelerateCar()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.W))
        {
            currentVelocity += (accelerationRate * Time.deltaTime);
        }
        else
        {
            currentVelocity -= (decelerationRate * Time.deltaTime);
        }
        currentVelocity = Mathf.Clamp(currentVelocity, initialVelocity, finalVelocity);
        transform.Translate(0, 0, currentVelocity);
    }

    private void HandleSteering()
    {
        currentsteerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentsteerAngle;
        frontRightWheelCollider.steerAngle = currentsteerAngle;
    }


    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    /*private void OnCollisionEnter(Collision other)
    {
        if (other.transform != transform && other.contacts.Length != 0)
        {
            for (int i = 0; i < other.contacts.Length; i++)
            {
                Instantiate(spark, other.contacts[i].point, Quaternion.identity);
            }
            
        }
        
    }*/
} 