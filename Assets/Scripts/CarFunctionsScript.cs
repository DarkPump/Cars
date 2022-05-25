using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarFunctionsScript : MonoBehaviour
{
    private AllCheckpointsCheckedScript metaCollider;
    private GameObject spawnPoint;
    private Quaternion defaultCarRotation;
    private bool isStartTriggered = false;
    private AllConnectedScript allConnectedScript;
    private AllSingleScript allSingleScript;
    private string carMode = "Asphalt mode";
    private float timer = 0;

    [Header("References")]
    public CarController3 carController;
    public Text carModeText;
    public Text currentTerrainText;
    public Text outOfTrackText;
    public Text speedText;
    public RankScript ranking1;
    public RankScript ranking2;
    public PlayerNameScript playerNameScript;
    public GameObject rankingUI;
    public GameObject inputNameObject;
    public Text timerText;

    [HideInInspector] public bool isCarOutOfTrack = false;
    [HideInInspector] public bool isAsphaltMode = true;
    [HideInInspector] public bool currentMode;
    [HideInInspector] public bool wasCarModeChanged = false;
    [HideInInspector] public string prefsString;
    [HideInInspector] public bool isCarOnTheGround;
    [HideInInspector] public bool isCarOnTheAsphalt;
    [HideInInspector] public Vector3 lastCheckpoint_position;
    [HideInInspector] public Quaternion lastCheckpoint_rotation;
    [HideInInspector] public bool checkpointFlag = false;
    public static bool isRankingShowing = true;
    public RaycastHit hit;
    
    


    private void Awake() 
    {
        metaCollider = GameObject.Find("MetaCollider").GetComponent<AllCheckpointsCheckedScript>();
        spawnPoint = GameObject.Find("SpawnPoint");
        allSingleScript = (AllSingleScript)FindObjectOfType(typeof(AllSingleScript));
        allConnectedScript = (AllConnectedScript)FindObjectOfType(typeof(AllConnectedScript));
        //allSingleScript = GameObject.Find("SingleCheckpoints").GetComponent<AllSingleScript>();
        //allConnectedScript = GameObject.Find("ConnectedCheckpoints").GetComponent<AllConnectedScript>();
        currentMode = isAsphaltMode;
    }

    void Start()
    {
        timerText.text = timer.ToString();
        carModeText.text = carMode;
        isRankingShowing = true;
    }

    void Update()
    {
        ResetCarPosition();
        CountTime();
        DisplayCurrentCarMode();
        ChangeCarMode();
        WheelRaycast();
        Speedometer();
        ResetCarToCheckpoint();
        if(isCarOutOfTrack == true && isCarOnTheAsphalt)
        {
            outOfTrackText.enabled = false;
            isCarOutOfTrack = false;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Start")
        {
            isStartTriggered = true;
            timer = 0;

            if(allSingleScript != null)
                allSingleScript.ResetAllCheckpoints();
            if(allConnectedScript != null)
                allConnectedScript.ResetAllCheckpoints();

            checkpointFlag = false;

            if(rankingUI.activeInHierarchy == true)
            {
                rankingUI.SetActive(false);
            }

            isRankingShowing = false;

            wasCarModeChanged = false;
            isAsphaltMode = true;
            carMode = "Asphalt mode";
        }
        else if(other.gameObject.tag == "Meta")
        {
            if(metaCollider.areAllCheckpointsChecked == true)
            {
                if(!wasCarModeChanged)
                {
                    prefsString = SceneManager.GetActiveScene().name;
                    ranking1.AddHighScoreEntry(timer, playerNameScript.playerName, prefsString);
                    ranking1.UpdateHighscoreEntryTransform(prefsString);
                }
                else
                {
                    prefsString = SceneManager.GetActiveScene().name + "Second";
                    ranking2.AddHighScoreEntry(timer, playerNameScript.playerName, prefsString);
                    ranking2.UpdateHighscoreEntryTransform(prefsString);
                }
                
                rankingUI.SetActive(true);
                isRankingShowing = true;

                isStartTriggered = false;
            }
        }
        else if(other.gameObject.tag == "OutsideTrack")
        {
            if(!isCarOutOfTrack)
            {
                outOfTrackText.enabled = true;
                isCarOutOfTrack = true;
            }
        }
        else if(other.gameObject.tag == "InsideTrack")
        {
            if(isCarOutOfTrack)
            {
                outOfTrackText.enabled = false;
                isCarOutOfTrack = false;
            }
        }
    }

    public void ResetCarPosition()
    {
        if(Input.GetKeyDown(KeyCode.R) && !inputNameObject.activeInHierarchy && !PauseMenu.isGamePaused)
        {
            carController._rb.isKinematic = true;
            carController._rb.isKinematic = false;
            carController._rb.transform.position = spawnPoint.transform.position;
            carController._rb.transform.rotation = spawnPoint.transform.rotation;

            timer = 0;
            timerText.text = timer.ToString();
            isStartTriggered = false;

            if(allSingleScript != null)
                allSingleScript.ResetAllCheckpoints();
            if(allConnectedScript != null)
                allConnectedScript.ResetAllCheckpoints();

            checkpointFlag = false;

            wasCarModeChanged = false;
            isAsphaltMode = true;
            carMode = "Asphalt mode";

            if(rankingUI.activeInHierarchy == true)
            {
                rankingUI.SetActive(false);
                isRankingShowing = false;
            }
                

            if(outOfTrackText.IsActive())
            {
                outOfTrackText.enabled = false;
                isCarOutOfTrack = false;
            }
            

        }
    }
    public void CountTime()
    {
        if(isStartTriggered)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString("F2") + "s";
        }
    }

    public void DisplayCurrentCarMode()
    {
        carModeText.text = carMode;
        if(isCarOnTheAsphalt)
            currentTerrainText.text = "Asphalt";
        else
            currentTerrainText.text = "Offroad";
    }

    public void ChangeCarMode()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && !PauseMenu.isGamePaused)
        {
            if(!wasCarModeChanged)
                wasCarModeChanged = true;
            if(currentMode != isAsphaltMode)
            {
                //Zmiana na Asfaltowy tryb
                carMode = "Asphalt mode";
                isAsphaltMode = true;
            }
            else
            {
                //Zmiana na szutrowy tryb
                carMode = "Offroad mode";
                isAsphaltMode = false;
            }
        }
    }

    public void WheelRaycast()
    {
        if(Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("asphaltLayer"))
            {
                isCarOnTheAsphalt = true;
                isCarOnTheGround = false;
            }
            else
            {
                isCarOnTheGround = true;
                isCarOnTheAsphalt = false;
            }
        }
    }

    public void Speedometer()
    {
        if(carController.speed >= 0)
            speedText.text = carController.speed.ToString("F0") + " km/h";
        else
            speedText.text = (carController.speed * -1).ToString("F0") + " km/h";
    }

    public void ResetCarToCheckpoint()
    {
        if(Input.GetKey(KeyCode.T) && checkpointFlag == true)
        {
            carController._rb.isKinematic = true;
            carController._rb.isKinematic = false;

            carController._rb.transform.position = lastCheckpoint_position;
            carController._rb.transform.rotation = lastCheckpoint_rotation;
        }
    }
}
