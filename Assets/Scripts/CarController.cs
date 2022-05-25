using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private bool asphaltMode;
    private bool groundMode;
    public bool isCarOnTheGround;
    public bool isCarOnTheAsphalt;
    public float fromSlipBreak = fromSlipBreakBase;
    public float fromSlipBreak2 = fromSlipBreakBase2;
    private bool isAsphaltMode = true;
    private bool currentMode;
    public string carMode = "Tryb asfaltowy";
    private Text carModeText;
    
    const float fromSlipBreakBase = 10;
    const float fromSlipBreakBase2 = 8;

    public float maxSteerAngle = 10;
    public float motorForce = 500;
    public float breakForce = 100;
    public Rigidbody carRB;
    public Vector3 center;
    public Vector3 startPosition;

    public WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider, rearRightWheelCollider;
    public Transform frontLeftWheelTransform, frontRightWheelTransform;
    public Transform rearLeftWheelTransform, rearRightWheelTransform;
    public LayerMask asphaltLayer;
    public LayerMask groundLayer;

    public void Start()
    {
        carRB = GetComponent<Rigidbody>();
        carRB.centerOfMass = center;
        startPosition = gameObject.transform.position;
        currentMode = isAsphaltMode;
    }

    private void FixedUpdate() 
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        Brake();
        WheelRaycast();
        ChangeSlipBasedOnSurface();
    }
    private void Update() 
    {
        ChangeCarMode();
    }

    public WheelFrictionCurve SetSlip(float extremumSlip, float extremumValue, float asymptoteSlip, float asymptoteValue, float stiffness)
    {
        WheelFrictionCurve wheelFrictionCurve = new WheelFrictionCurve();
        wheelFrictionCurve.extremumSlip = extremumSlip;
        wheelFrictionCurve.extremumValue = extremumValue;
        wheelFrictionCurve.asymptoteSlip = asymptoteSlip;
        wheelFrictionCurve.asymptoteValue = asymptoteValue;
        wheelFrictionCurve.stiffness = stiffness;

        return wheelFrictionCurve;
    }

    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    public void ChangeCarMode()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(currentMode != isAsphaltMode)
            {
                //Zmiana na Asfaltowy tryb
                carMode = "Tryb asfaltowy";
                isAsphaltMode = true;
            }
            else
            {
                //Zmiana na szutrowy tryb
                carMode = "Tryb szutrowy";
                isAsphaltMode = false;
            }
        }
    }

    public void ChangeSlipBasedOnSurface()
    {
        if(isCarOnTheAsphalt && isAsphaltMode)
        {
            //Debug.Log("Jeździsz po asfalcie w trybie asfaltowym");
        }
        else if(isCarOnTheAsphalt && !isAsphaltMode)
        {
            //Debug.Log("Jeździsz po asfalcie w trybie szutrowym");
            fromSlipBreak = 5;
            fromSlipBreak2 = 2.5f;
        }
        else if(!isCarOnTheAsphalt && isAsphaltMode)
        {
            //Debug.Log("Jeździsz po szutrze w trybie asfaltowym");
            fromSlipBreak = 5;
            fromSlipBreak2 = 2.5f;
        }
        else if(!isCarOnTheAsphalt && !isAsphaltMode)
        {
            //Debug.Log("Jeździsz po szutrze w trybie szutrowym");
        }
    }

    public void Steer()
    {
        horizontalInput *= maxSteerAngle;
        frontLeftWheelCollider.steerAngle = horizontalInput;
        frontRightWheelCollider.steerAngle = horizontalInput;
    }

    public void Accelerate()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;

    }

    public void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            fromSlipBreak = 5;
            fromSlipBreak = 4;

            rearLeftWheelCollider.sidewaysFriction = SetSlip(4, 1, 5, 1, 1);
            rearRightWheelCollider.sidewaysFriction = SetSlip(4, 1, 5, 1, 1);
            frontLeftWheelCollider.sidewaysFriction = SetSlip(4, 1, 5, 1, 1);
            frontRightWheelCollider.sidewaysFriction = SetSlip(4, 1, 5, 1, 1);

            frontLeftWheelCollider.brakeTorque += 0.0001f * breakForce * 100000; ;
            frontRightWheelCollider.brakeTorque += 0.0001f * breakForce * 100000; ;  
        }
        else
        {
            scaleSlipAfterBreak(fromSlipBreak, 1, fromSlipBreak2, 1);
            frontLeftWheelCollider.brakeTorque = 0;
            frontRightWheelCollider.brakeTorque = 0;
            rearLeftWheelCollider.brakeTorque = 0;
            rearRightWheelCollider.brakeTorque = 0;
        }
    }

    public void scaleSlipAfterBreak(float from_slip, float to_slip, float from_slip2, float to_slip2)
    {
        if (fromSlipBreak > to_slip) fromSlipBreak -= 5 * Time.deltaTime;
        if (fromSlipBreak2 > to_slip2) fromSlipBreak2 -= 5 * Time.deltaTime;

        rearLeftWheelCollider.sidewaysFriction = SetSlip(fromSlipBreak, 2, fromSlipBreak2, 1, 1);
        rearRightWheelCollider.sidewaysFriction = SetSlip(fromSlipBreak, 2, fromSlipBreak2, 1, 1);
        frontLeftWheelCollider.sidewaysFriction = SetSlip(fromSlipBreak, 2, fromSlipBreak2, 1, 1);
        frontRightWheelCollider.sidewaysFriction = SetSlip(fromSlipBreak, 2, fromSlipBreak2, 1, 1);
    }

    public void UpdateWheelPose(WheelCollider w, Transform t)
    {
        Vector3 position; Quaternion rotation;
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

    public void WheelRaycast()
    {
        RaycastHit hit;
        isCarOnTheGround = Physics.Raycast(transform.position, -transform.up, out hit, asphaltLayer);
        isCarOnTheAsphalt = Physics.Raycast(transform.position, -transform.up, out hit, groundLayer);
    }
}
