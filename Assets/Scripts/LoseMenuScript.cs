using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseMenuScript : MonoBehaviour {

    public void BackToMainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }

}
