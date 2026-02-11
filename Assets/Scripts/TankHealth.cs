using UnityEngine;
using TMPro;

public class TankHealth : MonoBehaviour
{
    private int currentHealth;
    private string currentTank;

    GameManager gameManager;

    public TextMeshProUGUI healthText;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(gameObject.CompareTag("Player1"))
        {
            currentHealth = gameManager.player1Lives;

            if(GameObject.Find("TankP2"))
            {
                currentTank = "Player 1";
            }
            else if(GameObject.Find("TankEnemy"))
            {
                currentTank = "Player";
            }
        }
        else if(gameObject.CompareTag("Player2"))
        {
            currentHealth = gameManager.player2Lives;
            currentTank = "Player 2";
        }
        else if(gameObject.CompareTag("Enemy"))
        {
            currentHealth = gameManager.enemyLives;
            currentTank = "Enemy";
        }

        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = currentTank + " - " + currentHealth.ToString() + " HP";
        }
    }
}
