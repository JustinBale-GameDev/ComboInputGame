using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float timer = 20f; // Initial timer value
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text roundText;

    private bool isGameActive = true;
    private int score = 0;
    private int currentRound = 1;
    public ComboManager comboManager;

	private void Start()
	{
		comboManager = FindAnyObjectByType<ComboManager>();
        StartNewRound();
	}

	private void Update()
	{
		if (isGameActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                GameOver();
            }
            UpdateTimerUI();
        }
	}

    void UpdateTimerUI()
    {
        timerText.text = "Time: " + Mathf.FloorToInt(timer).ToString();
    }
    void UpdateScoreUI()
    {
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }
    void UpdateRoundUI()
    {
        roundText.text = currentRound.ToString();
    }

    void GameOver()
    {
        isGameActive = false;
        Debug.Log("Game Over!");

        // Game over logic
    }

    public void AddTime(float amount)
    {
        timer += amount;
    }

    public void ReduceTime(float amount)
    {
        timer -= amount;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    public void StartNewRound()
    {
        timer = 20f;
        comboManager.GenerateCombosForRound(currentRound);
        comboManager.LoadNextCombo();
        UpdateRoundUI();
    }

    public void OnRoundCompleted()
    {
        currentRound++;
        StartNewRound();
    }

    public void RestartGame()
    {
        timer = 20f;
        score = 0;
        currentRound = 0;

        isGameActive = true;
        UpdateScoreUI();
        UpdateRoundUI();
        StartNewRound();
    }
}
