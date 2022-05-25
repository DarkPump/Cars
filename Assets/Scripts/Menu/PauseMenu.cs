using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject rankingUI;
    private string mainMenu = "MainMenu";

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGamePaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        isGamePaused = false;

        if(!PlayerNameScript.isNamePicked)
        {
            Time.timeScale = 0f;
            rankingUI.SetActive(true);
        }
        else
        {
            if(CarFunctionsScript.isRankingShowing)
            {
                Time.timeScale = 1f;
                rankingUI.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                rankingUI.SetActive(false); 
            }
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        if(!PlayerNameScript.isNamePicked || CarFunctionsScript.isRankingShowing)
            rankingUI.SetActive(false);
    }

    public void LoadMenu()
    {
        Resume();
        SceneManager.LoadScene(mainMenu);
    }

    public void QuitGame()
    {
        Debug.Log("Wyszedłeś z gry!");
        Application.Quit();
    }
}
