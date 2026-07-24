using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private TMP_Text livesText;
    private float timeLength = 3.0f;
    private int score = 0;
    [SerializeField] private int maxScore = 300;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        scoreText.text = $"Score: 0 / {maxScore}";
        livesText.text = $"Lives: {PlayerStats.playerLives}";
    }

    /// <summary>
    /// Add points to the score
    /// </summary>
    /// <param name="points">points to add to socre.</param>
    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = $"Score: {score} / {maxScore}";

        if (score >= maxScore)
        {
            StartCoroutine(WinSequence());
        }
    }

    private IEnumerator WinSequence()
    {
        ShowWinScreen();

        yield return new WaitForSecondsRealtime(timeLength); // use WaitForSecondsRealtime to avoid conflict with Time.timeScale

        QuitGame();
    }

    /// <summary>
    /// Get the current score
    /// </summary>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// Exit game.
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    private void ShowWinScreen()
    {
        winText.gameObject.SetActive(true);
        Time.timeScale = 0f; // pause game
    }

    /// <summary>
    /// Display player's lives in UI
    /// </summary>
    public void DisplayPlayerLives()
    {
        livesText.text = $"Lives: {PlayerStats.playerLives}";
    }

}