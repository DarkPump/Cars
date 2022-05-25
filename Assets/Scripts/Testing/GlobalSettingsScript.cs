using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSettingsScript : MonoBehaviour
{
    public Text currentFPSText;
    public float fpsRefreshRate = 1f;
    public float timer, avgFramerate;
    string display = "{0} FPS";
    private void Awake() 
    {  
        Application.targetFrameRate = -1;
        currentFPSText = GameObject.Find("CurrentFPS").GetComponent<Text>();
    }

    private void Update() 
    {
        SetFpsText();
    }

    public void SetFpsText()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? fpsRefreshRate : timer -= timelapse;

        if(timer <= 0) avgFramerate = (int) (1f / timelapse);
        currentFPSText.text = string.Format(display,avgFramerate.ToString());
    }
}
