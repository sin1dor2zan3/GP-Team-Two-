using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour {

    public void BackToMainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }
    
}
