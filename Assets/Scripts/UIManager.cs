using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject gamePanel;
    public GameObject gameOverMenu;
    public GameObject victoryMenu;

    public TextMeshProUGUI gamePanelBestScore;
    public TextMeshProUGUI mainMenuBestScore;
    public TextMeshProUGUI victoryMenuBestScore;

    // Sonidos para las bombas y menús
    public AudioClip bombDefuseSound;
    public AudioClip bombExplosionSound;
    public AudioClip gameOverSound;
    public AudioClip victorySound;

    private AudioSource audioSource;
    private int bestScore = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateBestScoreUI(bestScore);
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        startMenu.SetActive(true);
        gamePanel.SetActive(false);
        gameOverMenu.SetActive(false);
        victoryMenu.SetActive(false);
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        gamePanel.SetActive(true);
        gameOverMenu.SetActive(false);
        victoryMenu.SetActive(false);
        FindObjectOfType<GameManager>().StartGame(); // Reiniciar el juego
    }

    public void ShowGameOver(bool newHighScore)
    {
        startMenu.SetActive(false);
        gamePanel.SetActive(false);
        gameOverMenu.SetActive(true);
        victoryMenu.SetActive(false);

        if (newHighScore)
        {
            UpdateBestScoreUI(FindObjectOfType<GameManager>().GetScore());
        }
        PlaySound(gameOverSound);
    }

    public void ShowVictoryMenu()
    {
        startMenu.SetActive(false);
        gamePanel.SetActive(false);
        gameOverMenu.SetActive(false);
        victoryMenu.SetActive(true);
        UpdateBestScoreUI(bestScore);
        PlaySound(victorySound);
    }

    public void UpdateBestScoreUI(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
        }

        UpdateTextMeshProText(gamePanelBestScore, bestScore);
        UpdateTextMeshProText(mainMenuBestScore, bestScore);
        UpdateTextMeshProText(victoryMenuBestScore, bestScore);
    }

    private void UpdateTextMeshProText(TextMeshProUGUI textMesh, int score)
    {
        if (textMesh != null)
        {
            textMesh.text = "Best Score: " + score;
        }
    }

    public int GetBestScore()
    {
        return bestScore;
    }

    // Métodos para reproducir sonidos
    public void PlayBombDefuseSound()
    {
        PlaySound(bombDefuseSound);
    }

    public void PlayBombExplosionSound()
    {
        PlaySound(bombExplosionSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
