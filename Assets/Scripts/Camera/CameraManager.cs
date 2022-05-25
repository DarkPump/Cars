using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera driverCamera;
    [SerializeField] private Camera followCamera;
    [SerializeField] private GameObject inputNameObject;
    
    public bool wasCameraModeChanged = false;

    void Update()
    {
        ChangeCameraMode();
        ChangeCamerView();
    }

    
    private void ChangeCameraMode()
    {
        if(Input.GetKeyDown(KeyCode.C) && !inputNameObject.activeInHierarchy && !PauseMenu.isGamePaused)
            wasCameraModeChanged = !wasCameraModeChanged;
    }

    private void ChangeCamerView()
    {
        if(!wasCameraModeChanged)
        {
            //followCamera.gameObject.SetActive(true);
            driverCamera.gameObject.SetActive(false);
        }
        else if(wasCameraModeChanged)
        {
            //followCamera.gameObject.SetActive(false);
            driverCamera.gameObject.SetActive(true);
        }
    }
}
