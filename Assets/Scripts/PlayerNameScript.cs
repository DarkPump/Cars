using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameScript : MonoBehaviour
{
    public InputField inputName;
    public GameObject rankingUI;
    public GameObject inputNameGameObject;
    public GameObject carUIGameObject;
    public string playerName;
    public static bool isNamePicked = false;

    private void Start() 
    {
        PauseGame();
        inputName.characterLimit = 3;
        inputName.Select();
        isNamePicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckCharacterCount()
    {
        playerName = inputName.text;
        if(playerName.Length < 3)
            Debug.Log("Wprowadź 3 literową nazwę!");
        else
            InputPlayerName();
    }

    public void InputPlayerName()
    {
        rankingUI.SetActive(false);
        CarFunctionsScript.isRankingShowing = false;
        inputNameGameObject.SetActive(false);
        carUIGameObject.SetActive(true);
        isNamePicked = true;
        UnpauseGame();

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
