using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Wyszedłeś z gry!");
        Application.Quit();
    }
}
