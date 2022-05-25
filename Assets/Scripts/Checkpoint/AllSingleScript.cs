using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSingleScript : MonoBehaviour
{
    public List<GameObject> singleList;
    public bool isSingleChecked = false;
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
        singleList = new List<GameObject>();
        foreach(Transform single in this.gameObject.transform)
        {
            singleList.Add(single.gameObject);
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
        if(singleList.Count == 0)
            isSingleChecked = true;
        else
        {
            foreach(GameObject single in singleList)
            {
                if(!single.GetComponent<CheckpointScript>().isCheckpointPassed)
                {
                    isSingleChecked = false;
                    break;
                }
                else
                    isSingleChecked = true;
            }
        }
    }
}
