using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour {

    public void BackToMainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }

}
