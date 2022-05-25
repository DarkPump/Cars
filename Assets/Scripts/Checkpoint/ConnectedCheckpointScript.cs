using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedCheckpointScript : MonoBehaviour
{
    public List<GameObject> checkpointsList;
    public bool isCheckpointChecked = false;
    
    void Start()
    {
        CreateListFromChildObject();
    }

    void Update()
    {
        CheckpointCheck();
    }

    public void CreateListFromChildObject()
    {
        checkpointsList = new List<GameObject>();
        foreach(Transform checkpoint in this.gameObject.transform)
        {
            checkpointsList.Add(checkpoint.gameObject);
        }
    }

    public void CheckpointCheck()
    {
        if(checkpointsList.Count == 0)
        {
            isCheckpointChecked = true;
        }
        else
        {
            foreach(GameObject checkpoint in checkpointsList)
            {
                if(checkpoint.GetComponent<CheckpointScript>().isCheckpointPassed)
                {
                    isCheckpointChecked = true;
                    break;
                }
                else
                {
                    isCheckpointChecked = false;
                }
            }
        }

    }
}
