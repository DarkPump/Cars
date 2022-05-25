using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController3 : MonoBehaviour
{
    [Header("Inputs")]
    // Input names to read using GetAxis
    [SerializeField] string throttleInput = "Throttle";
    [SerializeField] string brakeInput = "Brake";
    [SerializeField] string turnInput = "Horizontal";

    [SerializeField] AnimationCurve turnInputCurve = AnimationCurve.Linear(-1.0f, -1.0f, 1.0f, 1.0f);
    [SerializeField] AnimationCurve alternateTurnInputCurve = AnimationCurve.Linear(-1.0f, -1.0f, 1.0f, 1.0f);

    [Header("Wheels")]
    [SerializeField] WheelCollider[] driveWheel;
    public WheelCollider[] DriveWheel { get { return driveWheel; } }
    [SerializeField] WheelCollider[] turnWheel;
    public WheelCollider[] TurnWheel { get { return turnWheel; } }
    [SerializeField] Transform[] wheelMeshes;
    public Transform[] WheelMeshes { get { return wheelMeshes; } }

    [Header("Behaviour")]
    [SerializeField] AnimationCurve motorTorque = new AnimationCurve(new Keyframe(0, 200), new Keyframe(50, 300), new Keyframe(200, 0));
    [SerializeField] AnimationCurve alternateMotorTorque = new AnimationCurve(new Keyframe(0, 200), new Keyframe(50, 300), new Keyframe(200, 0));

    // Differential
    [Range(2, 16)]
    [SerializeField] float diffGearing = 4.0f;
    public float DiffGearing { get { return diffGearing; } set { diffGearing = value; } }

    // Brake force
    [SerializeField] float brakeForce = 1500.0f;
    public float BrakeForce { get { return brakeForce; } set { brakeForce = value; } }

    // Max steering angle
    [Range(0f, 50.0f)]
    [SerializeField] float steerAngle = 30.0f;
    public float SteerAngle { get { return steerAngle; } set { steerAngle = Mathf.Clamp(value, 0.0f, 50.0f); } }

    // Steering Lerp
    [Range(0.001f, 1.0f)]
    [SerializeField] float steerSpeed = 0.2f;
    public float SteerSpeed { get { return steerSpeed; } set { steerSpeed = Mathf.Clamp(value, 0.001f, 1.0f); } }


    // Reset Values
    Vector3 spawnPosition;
    Quaternion spawnRotation;

    [SerializeField] Transform centerOfMass;

    // Downforce
    [Range(0.5f, 10f)]
    [SerializeField] float downforce = 1.0f;
    public float Downforce { get{ return downforce; } set{ downforce = Mathf.Clamp(value, 0, 5); } }     

    // Steering
    float steering;
    public float Steering { get{ return steering; } set{ steering = Mathf.Clamp(value, -1f, 1f); } } 

    // Throttle
    public float throttle;
    public float Throttle { get{ return throttle; } set{ throttle = Mathf.Clamp(value, -1f, 1f); } } 

    // Handbrake
    [SerializeField] bool handbrake;
    public bool Handbrake { get{ return handbrake; } set{ handbrake = value; } }       

    // Speedometer
    [SerializeField] public float speed = 0.0f;
    public float Speed { get{ return speed; } }

    public Rigidbody _rb;
    [SerializeField] public CarFunctionsScript carFunctionsScript;
    private WheelCollider[] wheels;

    void Start() {
        _rb = GetComponent<Rigidbody>();
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        if (_rb != null && centerOfMass != null)
        {
            _rb.centerOfMass = centerOfMass.localPosition;
        }

        wheels = GetComponentsInChildren<WheelCollider>();

        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = 0.0001f;
        }
    }
    
    void FixedUpdate() 
    {
        SpeedMeasure();
        Turning();
        Direction();
        HandBrake();
        
        _rb.AddForce(-transform.up * speed * downforce);
        UpdateWheelPose(wheels, wheelMeshes);
    }

    public float GetInput(string input) 
    {
        return Input.GetAxis(input);
    }

    private void UpdateWheelPose(WheelCollider[] w, Transform[] t)
    {
        for(int i = 0; i < t.Length; i++)
        {
            Vector3 _pos = t[i].position;
            Quaternion _rot = t[i].rotation;
            w[i].GetWorldPose(out _pos, out _rot);
            t[i].position = _pos;
            t[i].rotation = _rot;
        }
    }

    public void SpeedMeasure()
    {
        speed = transform.InverseTransformDirection(_rb.velocity).z * 3.6f;
        if (throttleInput != "" && throttleInput != null && speed > 0)
        {
            if(GetInput(throttleInput) > 0f)
            {
                throttle = GetInput(throttleInput) - (GetInput(brakeInput)*2);
                Debug.Log(GetInput(throttleInput));
            }
            else if(GetInput(throttleInput) == 0f)
            {
                throttle = -0.2f;
                Debug.Log(GetInput(throttleInput));
            }
            else if(GetInput(throttleInput) < 0f)
            {
                throttle = GetInput(throttleInput);
                Debug.Log(GetInput(throttleInput));
            }
        }
        else if (throttleInput != "" && throttleInput != null && speed == 0)
        {
            throttle = GetInput(throttleInput);
            Debug.Log(GetInput(throttleInput));
        }
        else if (throttleInput != "" && throttleInput != null && speed < 0)
        {
            if(GetInput(throttleInput) < 0f)
            {
                throttle = GetInput(throttleInput) + (GetInput(brakeInput)*2);
                Debug.Log(GetInput(throttleInput));
            }
            else if(GetInput(throttleInput) == 0f)
            {
                throttle = 0.2f;
                Debug.Log(GetInput(throttleInput));
            }
            else if(GetInput(throttleInput) > 0f)
            {
                throttle = GetInput(throttleInput);
                Debug.Log(GetInput(throttleInput));
            }
        }      
    }

    private void Turning()
    {
        if(carFunctionsScript.isCarOnTheAsphalt && carFunctionsScript.isAsphaltMode)
            steering = turnInputCurve.Evaluate(GetInput(turnInput)) * steerAngle;
        else if(carFunctionsScript.isCarOnTheAsphalt && !carFunctionsScript.isAsphaltMode)
            steering = alternateTurnInputCurve.Evaluate(GetInput(turnInput)) * steerAngle;
        else if(!carFunctionsScript.isCarOnTheAsphalt && carFunctionsScript.isAsphaltMode)
            steering = alternateTurnInputCurve.Evaluate(GetInput(turnInput)) * steerAngle;
        else if(!carFunctionsScript.isCarOnTheAsphalt && !carFunctionsScript.isAsphaltMode)
            steering = turnInputCurve.Evaluate(GetInput(turnInput)) * steerAngle;
    }

    private void Direction()
    {
        foreach (WheelCollider wheel in turnWheel)
        {
            wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, steering, steerSpeed);
        }

        foreach (WheelCollider wheel in wheels)
        {
            wheel.brakeTorque = 0;
        }
    }

    private void HandBrake()
    {
        if (handbrake)
        {
            foreach (WheelCollider wheel in wheels)
            {
                wheel.motorTorque = 0.0001f;
                wheel.brakeTorque = brakeForce;
            }
        }
        else if (Mathf.Abs(speed) < 4 || Mathf.Sign(speed) == Mathf.Sign(throttle))
        {
            if(carFunctionsScript.isCarOnTheAsphalt && carFunctionsScript.isAsphaltMode)
            {
                foreach (WheelCollider wheel in driveWheel)
                {
                    wheel.motorTorque = throttle * motorTorque.Evaluate(speed) * diffGearing / driveWheel.Length;
                }
            }
            else if(carFunctionsScript.isCarOnTheAsphalt && !carFunctionsScript.isAsphaltMode)
            {
                foreach (WheelCollider wheel in driveWheel)
                {
                    wheel.motorTorque = throttle * alternateMotorTorque.Evaluate(speed) * diffGearing / driveWheel.Length;
                }
            }
            else if(!carFunctionsScript.isCarOnTheAsphalt && carFunctionsScript.isAsphaltMode)
            {
                foreach (WheelCollider wheel in driveWheel)
                {
                    wheel.motorTorque = throttle * alternateMotorTorque.Evaluate(speed) * diffGearing / driveWheel.Length;
                }
            }
            else if(!carFunctionsScript.isCarOnTheAsphalt && !carFunctionsScript.isAsphaltMode)
            {
                foreach (WheelCollider wheel in driveWheel)
                {
                    wheel.motorTorque = throttle * motorTorque.Evaluate(speed) * diffGearing / driveWheel.Length;
                }
            }
        }
        else
        {
            foreach (WheelCollider wheel in wheels)
            {
                wheel.brakeTorque = Mathf.Abs(throttle) * brakeForce;
            }
        }
    }
}
