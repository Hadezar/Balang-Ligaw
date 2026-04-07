using UnityEngine;
using UnityEngine.UI;
using TMPro; // Remove this if you're using legacy UI Text

public class GameManager : MonoBehaviour
{
    [Header("Player References")]
    public PlayerHealth player1;
    public PlayerHealth player2;

    [Header("UI")]
    public GameObject winnerPanel;       // A UI Panel to show on game over
    public TextMeshProUGUI winnerText;   // "Player 1 Wins!" etc.
    // If using legacy Text instead of TMP, swap to:
    // public Text winnerText;

    private bool gameOver = false;

    void Update()
    {
        if (gameOver) return;

        bool p1Dead = player1 == null || !player1.IsAlive();
        bool p2Dead = player2 == null || !player2.IsAlive();

        if (p1Dead || p2Dead)
        {
            gameOver = true;

            if (p1Dead && p2Dead)
            {
                DeclareWinner("Draw! Both players are down!");
            }
            else if (p1Dead)
            {
                DeclareWinner("Player 2 Wins!");
            }
            else
            {
                DeclareWinner("Player 1 Wins!");
            }
        }
    }

    void DeclareWinner(string message)
    {
        Debug.Log("GAME OVER - " + message);

        if (winnerText != null)
            winnerText.text = message;

        if (winnerPanel != null)
            winnerPanel.SetActive(true);

        // Optional: pause the game
        Time.timeScale = 0f;
    }

    // Call this from a "Restart" button
    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}