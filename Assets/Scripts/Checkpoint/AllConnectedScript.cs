using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllConnectedScript : MonoBehaviour
{
    public List<GameObject> connectedList;
    public bool isConnectedChecked = false;
    public CheckpointScript[] checkpointArray;

    void Start()
    {
        CreateListFromChildObject();
    }

    void Update()
    {
        ConnectedCheck();
    }

    public void CreateListFromChildObject()
    {
        checkpointArray = gameObject.GetComponentsInChildren<CheckpointScript>();
        connectedList = new List<GameObject>();
        foreach(Transform connected in this.gameObject.transform)
        {
            connectedList.Add(connected.gameObject);
        }
    }

    public void ResetAllCheckpoints()
    {
        for(int i = 0; i < checkpointArray.Length; i++)
        {
            checkpointArray[i].isCheckpointPassed = false;
        }
    }

    public void ConnectedCheck()
    {
        if(connectedList.Count == 0)
            isConnectedChecked = true;
        else
        {
            foreach(GameObject connected in connectedList)
            {
                if(!connected.GetComponent<ConnectedCheckpointScript>().isCheckpointChecked)
                {
                    isConnectedChecked = false;
                    break;
                }
                else
                    isConnectedChecked = true;
            }
        }
    }
}
