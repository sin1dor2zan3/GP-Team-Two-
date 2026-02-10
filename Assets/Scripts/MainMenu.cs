using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string[] levelScenes;
    
    public void PlayGame()
    {
        int index = Random.Range(0, levelScenes.Length);
        SceneManager.LoadScene(levelScenes[index]);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}