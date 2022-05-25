using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public CarFunctionsScript car;
    public CarController3 carController;
    public bool isCheckpointPassed = false;

    [Header("Position and rotation")]
    public Quaternion fixedRotation;

    private void Start() 
    {

    }

    public void OnTriggerEnter(Collider other) 
    {
        if(!car.checkpointFlag)
            car.checkpointFlag = true;
        if(other.gameObject.tag == "Car" && !isCheckpointPassed)
        {
            isCheckpointPassed = true;

            car.lastCheckpoint_position.x = gameObject.transform.position.x;
            car.lastCheckpoint_position.y = carController._rb.position.y;
            car.lastCheckpoint_position.z = gameObject.transform.position.z;
            car.lastCheckpoint_rotation = gameObject.transform.rotation;
            Debug.Log("Przejechałeś przez checkpoint");
        }
    }
}
