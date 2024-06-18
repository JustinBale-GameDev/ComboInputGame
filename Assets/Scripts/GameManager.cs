using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public ComboManager comboManager;
	private PlayerInputHandler playerInputHandler;

	public InputActionAsset inputActions;
	private InputAction playGameAction;

	public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text gameOverScoreText;
    public TMP_Text gameOverTotalRoundsText;
    public TMP_Text roundText;
	public TMP_Text roundBonusText;
	public TMP_Text roundBonusValueText;
	public TMP_Text timeBonusText;
	public TMP_Text timeBonusValueText;
	public TMP_Text perfectBonusText;
	public TMP_Text perfectBonusValueText;
	public TMP_Text totalScoreText;
	public TMP_Text totalScoreValueText;

	public Image timerBarVisual;
    public Image timerBarColorChanger;

    public GameObject gameOverPanel;
    public GameObject gameStartPanel;
    public GameObject roundPausePanel;

	public AudioSource gameOverAudioSource;
	public AudioSource roundCompleteAudioSource;

	public Color startColor = Color.green; // Start color (max time)
	public Color endColor = Color.red; // End color (no time left)

	private bool isGameActive = false;
    private bool isBetweenRounds = false;
	private bool isDisplayingRoundSummary = false;

	private int score = 0;
    private int currentRound = 1;
    private int totalErrors = 0; // Track total errors
    private int roundErrors = 0; // Track errors per round
    private int perfectCombos = 0;
    private int totalPerfectCombos = 0;

	public float maxTime = 15f; // Initial timer value
	public float currentTimeLeft;



	private void Awake()
	{
		playGameAction = inputActions.FindActionMap("Gameplay").FindAction("PlayGame");
	}

	private void Start()
	{
		comboManager = FindAnyObjectByType<ComboManager>();
		playerInputHandler = FindAnyObjectByType<PlayerInputHandler>();
		StartNewRound();
        gameOverPanel.SetActive(false); // Gameover panel hidden at the start
        gameStartPanel.SetActive(true);
        roundPausePanel.SetActive(false);
	}

	private void Update()
	{
		if (isGameActive)
        {
			currentTimeLeft -= Time.deltaTime;
            if (currentTimeLeft <= 0)
            {
				currentTimeLeft = 0;
                GameOver();
            }
            UpdateTimerUI();
        }

        if (!isGameActive && playGameAction.WasPressedThisFrame() && !isDisplayingRoundSummary)
        {
            if (isBetweenRounds)
            {
                ResumeGame();
            }
            else
            {
				RestartGame();
			}
        }
	}

    void UpdateTimerUI()
    {
        timerText.text = $"Time: {currentTimeLeft:F2}";

		float timerScaleX = Mathf.Clamp(1 - currentTimeLeft / maxTime, 0, 1);
		timerBarVisual.transform.localScale = new Vector3(timerScaleX, 1, 1);

		// Interpolate the color of the timer bar
		float t = Mathf.Clamp(currentTimeLeft / maxTime, 0, 1);
		timerBarColorChanger.color = Color.Lerp(endColor, startColor, t);
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
		gameOverAudioSource.Play();
		isGameActive = false;
        playerInputHandler.IsNotPlaying(); // Disables input for combo directions
        gameOverPanel.SetActive(true);
		gameOverTotalRoundsText.text = "Rounds Completed: " + (currentRound - 1).ToString(); // minus 1 from current round to reflect completed rounds
		gameOverScoreText.text = "Total Score: " + Mathf.FloorToInt(score).ToString();
    }

    public void AddTime(float amount)
    {
		currentTimeLeft += amount;
    }

    public void ReduceTime(float amount)
    {
		currentTimeLeft -= amount;
	}

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    public void ReduceScore(int points)
    {
        if (score > 0)
        {
			score -= points;
		}
		UpdateScoreUI();
	}

    public void StartNewRound()
    {
		currentTimeLeft = maxTime;
        roundErrors = 0; // Reset round errors
        perfectCombos = 0;
        comboManager.GenerateCombosForRound(currentRound);
        comboManager.LoadNextCombo();
        UpdateRoundUI();
		UpdateScoreUI();
	}

    public void OnRoundCompleted()
    {
        roundCompleteAudioSource.Play();
        isGameActive = false;
        isBetweenRounds = true;
		isDisplayingRoundSummary = true;
		playerInputHandler.IsNotPlaying();
        StartCoroutine(DisplayRoundSummary());
        roundPausePanel.SetActive(true);
        currentRound++;
        StartNewRound();
    }

    public void ResumeGame()
    {
        // Reset round end text elements so they appear empty next time
        roundBonusText.text = "";
		roundBonusValueText.text = "";
		timeBonusText.text = "";
		timeBonusValueText.text = "";
		perfectBonusText.text = "";
		perfectBonusValueText.text = "";
		totalScoreText.text = "";
		totalScoreValueText.text = "";

		isGameActive = true;
        isBetweenRounds = false;
        playerInputHandler.IsPlaying();
        roundPausePanel.SetActive(false);
        StartNewRound();
    }

	public void RestartGame()
    {
		currentTimeLeft = maxTime;
        score = 0;
        currentRound = 1;
        totalErrors = 0;
        totalPerfectCombos = 0;

        isGameActive = true;
		playerInputHandler.IsPlaying();
		gameOverPanel.SetActive(false);
        gameStartPanel.SetActive(false);
        roundPausePanel.SetActive(false);

		UpdateScoreUI();
        UpdateRoundUI();
        StartNewRound();
	}

    public void AddError()
    {
        totalErrors++;
        roundErrors++;
    }

    public void AddPerfectCombo()
    {
        perfectCombos++;
        totalPerfectCombos++;
    }

    private IEnumerator DisplayRoundSummary()
    {
        // Calculate bonuses
        int roundBonus = Mathf.Max(100 - roundErrors * 10, 0);
        int timeBonus = Mathf.FloorToInt(currentTimeLeft * 5);
        int perfectBonus = perfectCombos * 20;
        int totalRoundScore = score + roundBonus + timeBonus + perfectBonus;
        score = totalRoundScore;

        // Update UI
        roundBonusText.text = "Round Bonus";
		roundBonusValueText.text = roundBonus.ToString();
		yield return new WaitForSeconds(0.75f);
        timeBonusText.text = "Time Bonus";
		timeBonusValueText.text = timeBonus.ToString();
		yield return new WaitForSeconds(0.75f);
        perfectBonusText.text = "Perfect Bonus";
		perfectBonusValueText.text = perfectBonus.ToString();
		yield return new WaitForSeconds(0.75f);
        totalScoreText.text = "Total Score";
		totalScoreValueText.text = score.ToString();

		// Allow proceeding to the next round
		isDisplayingRoundSummary = false;
	}

	//IEnumerator ShowText(string text, TMP_Text textComponent)
	//{
	//	textComponent.text = "";

	//	foreach (char c in text)
	//	{
	//		textComponent.text += c;
	//		yield return new WaitForSecondsRealtime(0.03f);
	//	}
	//}

	private void OnEnable()
	{
		playGameAction.Enable();
	}

	private void OnDisable()
	{
		playGameAction.Disable();
	}
}
