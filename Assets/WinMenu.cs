using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinMenuScript : MonoBehaviour {

    public void BackToMainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }
    
}
