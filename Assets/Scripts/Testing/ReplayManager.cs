using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    private Transform[] transforms;
    private MemoryStream memoryStream = null;
    private BinaryWriter binaryWriter = null;
    private BinaryReader binaryReader = null;
    private bool recordingInitialized;

    private int currentRecordingFrames = 0;
    public int maxRecordingFrames = 360;




    private bool recording = false;
    private bool replaying = false;

    public Action OnStartedRecording;
    public Action OnStoppedRecording;
    public Action OnStartedReplaying;
    public Action OnStoppedReplaying;
    [SerializeField] private MachineTestingScript machineTestingScript;

    public void Start()
    {
        transforms = FindObjectsOfType<Transform>();
    }

    public void FixedUpdate()
    {
        if (recording)
        {
            UpdateRecording();
        }
        else if (replaying)
        {
            UpdateReplaying();
        }
    }
    
    private void Update() 
    {
        if(machineTestingScript.tempStartTrigger == true)
        {
            StartRecording();
            if(currentRecordingFrames == maxRecordingFrames)
                machineTestingScript.tempStartTrigger = false;
        }
    }

    private void InitializeRecording()
    {
        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        binaryReader = new BinaryReader(memoryStream);
        recordingInitialized = true;
    }



    public void StartStopRecording()
    {
        if (!recording)
        {
            StartRecording();
        }
        else
        {
            StopRecording();
        }
    }


    private void StartRecording()
    {
        if (!recordingInitialized)
        {
            InitializeRecording();
        }
        else
        {
            memoryStream.SetLength(0);
        }

        ResetReplayFrame();

        recording = true;
        if (OnStartedRecording != null)
        {
            OnStartedRecording();
        }
    }

    private void UpdateRecording()
    {
        if (currentRecordingFrames > maxRecordingFrames)
        {
            StopRecording();
            currentRecordingFrames = 0;
            return;
        }
        SaveTransforms(transforms);
        ++currentRecordingFrames;
    }

    private void StopRecording()
    {
        recording = false;
        if (OnStoppedRecording != null)
        {
            OnStoppedRecording();
        }
    }

    public void StartStopReplaying()
    {
        if (!replaying)
        {
            StartReplaying();
        }
        else
        {
            StopReplaying();
        }
    }

    private void StartReplaying()
    {
        ResetReplayFrame();

        replaying = true;
        if (OnStartedReplaying != null)
        {
            OnStartedReplaying();
        }
    }

    private void UpdateReplaying()
    {
        if (memoryStream.Position >= memoryStream.Length)
        {
            StopReplaying();
            return;
        }

        LoadTransforms(transforms);
    }

    private void StopReplaying()
    {
        replaying = false;
        if (OnStoppedReplaying != null)
        {
            OnStoppedReplaying();
        }
    }

    private void ResetReplayFrame()
    {
        memoryStream.Seek(0, SeekOrigin.Begin);
        binaryWriter.Seek(0, SeekOrigin.Begin);
    }

    private void SaveTransforms(Transform[] transforms)
    {
        foreach (Transform transform in transforms)
        {
            SaveTransform(transform);
        }
    }


    private void SaveTransform(Transform transform)
    {
        binaryWriter.Write(transform.localPosition.x);
        binaryWriter.Write(transform.localPosition.y);
        binaryWriter.Write(transform.localPosition.z);
    }

    private void LoadTransforms(Transform[] transforms)
    {
        foreach (Transform transform in transforms)
        {
            LoadTransform(transform);
        }
    }

    private void LoadTransform(Transform transform)
    {
        float x = binaryReader.ReadSingle();
        float y = binaryReader.ReadSingle();
        float z = binaryReader.ReadSingle();
        transform.localPosition = new Vector3(x, y, z);
    }
}
