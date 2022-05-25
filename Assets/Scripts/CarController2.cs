using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController2 : MonoBehaviour
{
    public float motorForce = 1500;
    public float maxSteerAngle = 30;
    public float horizontalInput;
    public float verticalInput;

    public WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider, rearRightWheelCollider;
    public Transform frontLeftWheelTransform, frontRightWheelTransform;
    public Transform rearLeftWheelTransform, rearRightWheelTransform;
    public Rigidbody carRB;
    public Vector3 center;

    private float steeringAngle;
    
    private void Start() 
    {
        carRB = GetComponent<Rigidbody>();
        carRB.centerOfMass = center;
    }

    private void FixedUpdate() 
    {
        Steering();
        Accelerate();
        UpdateWheelPoses();
    }

    private void Update()
    {
        GetInput();
    }

    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    public void Accelerate()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        //rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        //rearRightWheelCollider.motorTorque = verticalInput * motorForce;
    }

    public void Steering()
    {
        steeringAngle = Mathf.Clamp(horizontalInput, -1, 1) * maxSteerAngle;
        frontLeftWheelCollider.steerAngle = Mathf.Lerp(frontLeftWheelCollider.steerAngle, steeringAngle, 0.5f);
        frontRightWheelCollider.steerAngle = Mathf.Lerp(frontRightWheelCollider.steerAngle, steeringAngle, 0.5f);
    }

    public void UpdateWheelPose(WheelCollider w, Transform t)
    {
        Vector3 position = t.position; 
        Quaternion rotation = t.rotation;
        w.GetWorldPose(out position, out rotation);
        t.position = position;
        t.rotation = rotation;
    }

    public void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPose(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPose(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPose(rearRightWheelCollider, rearRightWheelTransform);
    }
}
