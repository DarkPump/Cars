using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCheckpointsCheckedScript : MonoBehaviour
{
    private AllSingleScript singleCheckpoints;
    private AllConnectedScript connectedCheckpoints;
    public bool areAllSingleCheckpointsChecked = false;
    public bool areAllConnectedCheckpointsChecked = false;
    public bool areAllCheckpointsChecked = false;
    void Start()
    {
        singleCheckpoints = (AllSingleScript)FindObjectOfType(typeof(AllSingleScript));
        connectedCheckpoints = (AllConnectedScript)FindObjectOfType(typeof(AllConnectedScript));
    }

    void Update()
    {
        CheckpointChecker();
    }

    public void CheckpointChecker()
    {
        if(singleCheckpoints != null && connectedCheckpoints != null)
        {
            areAllSingleCheckpointsChecked = singleCheckpoints.GetComponent<AllSingleScript>().isSingleChecked;
            areAllConnectedCheckpointsChecked = connectedCheckpoints.GetComponent<AllConnectedScript>().isConnectedChecked;
        }
        else
        {
            areAllSingleCheckpointsChecked = true;
            areAllConnectedCheckpointsChecked = true;
        }

        if(areAllSingleCheckpointsChecked && areAllConnectedCheckpointsChecked)
            areAllCheckpointsChecked = true;
        else
            areAllCheckpointsChecked = false;

    }
}
