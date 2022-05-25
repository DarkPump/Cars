using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayPanelController : MonoBehaviour
{
    public Button startStopRecordButton;
    public Text startStopRecordButtonText;
    public Button startStopReplayButton;
    public Text startStopReplayButtonText;
    public ReplayManager replayManager;
    public MachineTestingScript machineTestingScript;

    private void OnEnable()
    {
        replayManager.OnStartedRecording += OnStartedRecording;
        replayManager.OnStoppedRecording += OnStoppedRecording;
        replayManager.OnStartedReplaying += OnStartedReplaying;
        replayManager.OnStoppedReplaying += OnStoppedReplaying;
    }

    private void OnDisable()
    {
        replayManager.OnStartedRecording -= OnStartedRecording;
        replayManager.OnStoppedRecording -= OnStoppedRecording;
        replayManager.OnStartedReplaying -= OnStartedReplaying;
        replayManager.OnStoppedReplaying -= OnStoppedReplaying;
    }

    void OnStartedRecording()
    {
        startStopRecordButtonText.text = "Stop Recording";
        machineTestingScript.SpawnCar();
    }

    void OnStoppedRecording()
    {
        startStopRecordButtonText.text = "Start Recording";
        startStopReplayButton.interactable = true;
    }

    void OnStartedReplaying()
    {
        startStopReplayButtonText.text = "Stop Replay";
        startStopRecordButton.interactable = false;
    }

    void OnStoppedReplaying()
    {
        startStopReplayButtonText.text = "Start Replay";
        startStopRecordButton.interactable = true;
    }
}
