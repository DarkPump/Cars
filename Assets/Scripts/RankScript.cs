using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankScript : MonoBehaviour
{
    public Transform entryContainer;
    public CarFunctionsScript carFunctionsScript;

    public List<Transform> highscoreEntryTransformList;
    public string prefsString;
    public Transform entryTransform;
    [SerializeField] GameObject entryPrefab;
    [SerializeField] GameObject rankingType;
    private void Awake() 
    {
        carFunctionsScript = GameObject.Find("Car").GetComponent<CarFunctionsScript>();
        entryContainer = transform.Find("rankingEntryContainer");
        
        if(rankingType.name == "Ranking")
            prefsString = SceneManager.GetActiveScene().name;
        else
            prefsString = SceneManager.GetActiveScene().name + "Second";
        
        string jsonString = PlayerPrefs.GetString(prefsString);
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        if (highScores == null) 
        {
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            AddHighScoreEntry(999.99f, "AAA", prefsString);
            
            jsonString = PlayerPrefs.GetString(prefsString);
            highScores = JsonUtility.FromJson<HighScores>(jsonString);
        }

        SortList(highScores.highscoreEntryList);

        highscoreEntryTransformList = new List<Transform>();
        foreach(HighscoreEntry highscoreEntry in highScores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void Update() 
    {

    }

    private void SortList(List<HighscoreEntry> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            for(int j = i + 1; j < list.Count; j++)
            {
                if(list[j].time < list[i].time)
                {
                    HighscoreEntry tmp = list[i];
                    list[i] = list[j];
                    list[j] = tmp;
                }
            }
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 40f;
        entryTransform = Instantiate(entryPrefab.transform, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);

        int rank = transformList.Count + 1;
        string rankString;

        switch(rank) 
        {
            default:
                rankString = rank + "TH"; break;
            case 1 : rankString = "1ST"; break;
            case 2 : rankString = "2ND"; break;
            case 3 : rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;
        float raceTime = highscoreEntry.time;
        entryTransform.Find("timeText").GetComponent<Text>().text = raceTime.ToString("0.00") + "s";

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    public void UpdateHighscoreEntryTransform(string playPrefsString)
    {
        string jsonString = PlayerPrefs.GetString(playPrefsString);
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        if(highScores == null)
        {
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
            AddHighScoreEntry(999.99f, "AAA", playPrefsString);
        }
        else
        {
            SortList(highScores.highscoreEntryList);
            foreach(Transform child in entryContainer)
            {
                GameObject.Destroy(child.gameObject);
            }

            highscoreEntryTransformList = new List<Transform>();

            foreach(HighscoreEntry highscoreEntry in highScores.highscoreEntryList)
            {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }
        }


    }

    public void AddHighScoreEntry(float time, string name, string playPrefsString)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { time = time , name = name};

        string jsonString = PlayerPrefs.GetString(playPrefsString);
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        if (highScores == null) {
            // There's no stored table, initialize
            highScores = new HighScores() {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        if(highScores.highscoreEntryList.Count == 10)
        {
            SortList(highScores.highscoreEntryList);
            for(int i = 9; i >= 0; i--)
            {
                if(highScores.highscoreEntryList[i].time == 999.99f)
                {
                    Debug.Log("Czas " + highScores.highscoreEntryList[i].time + " został zamieniony na " + highscoreEntry.time);
                    highScores.highscoreEntryList[i].time = highscoreEntry.time;
                    highScores.highscoreEntryList[i].name = highscoreEntry.name;
                    break;
                }
                else if(highScores.highscoreEntryList[i].time > highscoreEntry.time)
                {
                    Debug.Log("Czas " + highScores.highscoreEntryList[i].time + " został zamieniony na " + highscoreEntry.time);
                    highScores.highscoreEntryList[i].time = highscoreEntry.time;
                    highScores.highscoreEntryList[i].name = highscoreEntry.name;
                    break;
                }
            }
        }

        if(highScores.highscoreEntryList.Count < 10)
            highScores.highscoreEntryList.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString(playPrefsString, json);
        PlayerPrefs.Save();
    }

    private class HighScores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry 
    {
        public float time;
        public string name;
        public TrackType trackType = TrackType.NONE;
    }

    private enum TrackType
    {
        NONE,
        ASPHALT,
        GRAVEL
    }
}


