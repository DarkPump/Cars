using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFPSScript : MonoBehaviour
{
    public Text currentPreset;
    public string presetName = "Unlimited";
    private void Awake() 
    {
        currentPreset = GameObject.Find("Preset").GetComponent<Text>();
        currentPreset.text = presetName;
    }

    public void Set30Fps()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        presetName = "30FPS";
        currentPreset.text = presetName;
    }
    public void Set60Fps()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        presetName = "60FPS";
        currentPreset.text = presetName;
    }
    public void SetUnlimited()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
        presetName = "Unlimited";
        currentPreset.text = presetName;
    }
}
