using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MachineTestingScript : MonoBehaviour
{
    CarController3 carController3;
    public float currentTime = 0f;
    public float maxTime = 10f;
    public float startCarTime = 3f;
    public float distance;
    public bool isStartTriggered = false;
    public Rigidbody carRB;
    public GameObject startingPoint;
    public Text distanceText;
    private bool debugged = false;
    public GameObject car;
    public Action OnStartedRecording;
    public bool tempStartTrigger = false;

    private void Awake() 
    {
        carController3 = (CarController3)FindObjectOfType(typeof(CarController3));
        distanceText = GameObject.Find("Distance").GetComponent<Text>();
        distanceText.enabled = false;
    }

    private void Start() 
    {
        car.SetActive(false);
        carRB.isKinematic = true;
    }

    void FixedUpdate()
    {
        Timer();
        if(car.activeSelf)
            CarCountdown();
    }

    private void Update() 
    {
        if(Input.GetKey(KeyCode.X))
            SpawnCar();
    }

    public void SpawnCar()
    {
        car.SetActive(true);
    }

    private void CarCountdown()
    {
        if(currentTime < startCarTime)
        {
            distanceText.text = ((int)startCarTime).ToString();
            distanceText.enabled = true;
            startCarTime -= Time.fixedDeltaTime;
            if(startCarTime < 0)
                distanceText.enabled = false;
        }
        else
        {
            carRB.isKinematic = false;
        }
    }

    private void Timer()
    {
        if(isStartTriggered)
        {
            if(currentTime < maxTime)
            {
                currentTime += Time.fixedDeltaTime;
                Debug.Log(Time.fixedDeltaTime);
            }
            else
            {
                //currentTime = maxTime;
                carRB.isKinematic = true;
                distance = Vector3.Distance(carRB.transform.position, startingPoint.transform.position);
                if(!debugged)
                {
                    distanceText.text = string.Format("{0:0.000}m", distance);
                    distanceText.enabled = true;
                    debugged = true;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        isStartTriggered = true;
        tempStartTrigger = true;
        Debug.Log("Start");
    }
}
