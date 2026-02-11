using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int player1Lives = 3;
    public int player2Lives = 3;
    public int enemyLives = 3;

    public string gameSceneName = "MainMenu";
    public string winSceneName = "WinMenu";
    public string loseSceneName = "LoseMenu";

    public AudioSource sfxAudio;
    public AudioClip explosionClip;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when something dies
    public void TankDestroyed(string tankTag)
    {
        if (sfxAudio != null && explosionClip != null)
            sfxAudio.PlayOneShot(explosionClip);

        if (tankTag == "Player1")
        {
            player1Lives--;
            if (player1Lives <= 0)
            {
                SceneManager.LoadScene(loseSceneName);
                return;
            }
        }
        else if (tankTag == "Player2")
        {
            player2Lives--;
            if (player2Lives <= 0)
            {
                SceneManager.LoadScene(loseSceneName);
                return;
            }
        }
        else if (tankTag == "Enemy")
        {
            enemyLives--;
            if (enemyLives <= 0)
            {
                SceneManager.LoadScene(winSceneName);
                return;
            }
        }

        // Reload game scene for next round
        SceneManager.LoadScene(gameSceneName);
    }
}