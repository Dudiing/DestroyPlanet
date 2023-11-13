using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int lives = 3;
    private int score = 0;
    private UIManager uiManager;
    private SpawnManager spawnManager;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    public void StartGame()
    {
        ClearAllBombs();
        lives = 3;
        score = 0;
        UpdateUI();

        spawnManager.StartSpawning();
    }

    public void TakeDamage()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void AddScore()
    {
        score++;
        UpdateUI();
        CheckForBestScore();
    }

    public int GetScore() => score;

    private void UpdateUI()
    {
        if (livesText != null) livesText.text = "Lives: " + lives;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    private void GameOver()
    {
        if (spawnManager != null)
        {
            spawnManager.StopSpawning();
        }
        StopAllBombs();

        // Mostrar el GameOver menu si el bestScore es 0 o si el score es menor que el bestScore
        if (uiManager.GetBestScore() == 0 || score < uiManager.GetBestScore())
        {
            uiManager.ShowGameOver(false);
        }
        else
        {
            uiManager.ShowVictoryMenu();
        }
    }

    private void CheckForBestScore()
    {
        if (score > uiManager.GetBestScore())
        {
            uiManager.UpdateBestScoreUI(score);
        }
    }

    public void BackToMainMenu()
    {
        uiManager.ShowStartMenu();
    }

    private void StopAllBombs()
    {
        foreach (var bomb in FindObjectsOfType<BombScript>())
        {
            bomb.StopExplosion();
        }
    }

    private void ClearAllBombs()
    {
        foreach (var bomb in FindObjectsOfType<BombScript>())
        {
            Destroy(bomb.gameObject);
        }
    }
}
