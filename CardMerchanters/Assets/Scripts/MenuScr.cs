using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScr : MonoBehaviour 
{
    public void StartGame()
    {
        ChangeScene("scene");
    }
    public void BackToMenu()
    {
        ChangeScene("menu");
    }
    public void ToRules()
    {
        ChangeScene("rules");
    }
    public void ChangeScene(string SceneName) 
    { 
        SceneManager.LoadScene(SceneName); 
    }
    public void Exit() 
    { 
        Application.Quit();
    } 
}
