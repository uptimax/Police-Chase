using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ScreenController : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void goToStore()
    {
        //TabButton openTab = GameObject.Find("Character Tab").GetComponent<TabGroup>().tabButtons[0];
        //TabGroup openedTab = GameObject.Find("Tab Area").GetComponent<TabGroup>();
        //openedTab.selectedTab = openTab;

        SceneManager.LoadSceneAsync("Store Page");
    }

    public void goToSettings()
    {
        SceneManager.LoadSceneAsync("Settings Page");
    }

    public void back()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void pause()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
