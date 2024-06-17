using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboManager : MonoBehaviour
{
	public List<ComboScriptableObject> allCombos; // All possible combos
	public List<ComboScriptableObject> currentRoundCombos; // Combos for the current round
    public int currentComboIndex;
    public Combo currentCombo;

	[Header("UI Elements")]
	public Image[] comboImageSlots; // Array to hold all the spaces for the current combo input sequence. (Directional images will fill this up depending on the combo)
	public Image[] upcomingComboImages; // Array to hold the upcoming combo images
	public Image currentComboImage;
	public TMP_Text currentComboName;
	public Sprite leftImage;
	public Sprite rightImage;
	public Sprite upImage;
	public Sprite downImage;

	private PlayerInputHandler playerInputHandler;
	private GameManager gameManager;

	private void Start()
	{
		playerInputHandler = FindAnyObjectByType<PlayerInputHandler>();
		gameManager = FindAnyObjectByType<GameManager>();
	}

	public void GenerateCombosForRound(int roundNumber)
	{
		// generate a fixed number of combos based on round number
		currentRoundCombos = new List<ComboScriptableObject>();
		int comboCount = Mathf.Min(roundNumber + 2, allCombos.Count); // Increase number of combos with each round (TODO: Cap at 8 total combos for a single round(For now??))
		for (int i = 0; i < comboCount; i++)
		{
			int randomNumber = Random.Range(0, allCombos.Count); // Choose a random combo to be put in the curren round combo list
			currentRoundCombos.Add(allCombos[randomNumber]);
		}
		currentComboIndex = 0;
	}

	public void LoadNextCombo()
	{
		if (currentComboIndex < currentRoundCombos.Count)
		{
			ComboScriptableObject comboSO = currentRoundCombos[currentComboIndex];
			currentCombo = new Combo(comboSO.comboName, comboSO.sequence, comboSO.image);
			currentComboIndex++;
			UpdateUI();
			playerInputHandler.SetCurrentCombo(currentCombo);
		}
		else
		{
			Debug.Log("ROUND COMPLETED");
			gameManager.OnRoundCompleted();
		}
	}

	public void UpdateUI()
	{
		if (currentCombo == null) return;

		currentComboImage.sprite = currentCombo.GetImage();
		currentComboName.text = currentCombo.GetName();

		List<string> sequence = currentCombo.GetSequence();
		for (int i = 0; i < comboImageSlots.Length; i++)
		{
			if (i < sequence.Count)
			{
				comboImageSlots[i].sprite = GetDirectionSprite(sequence[i]);
				comboImageSlots[i].color = new Color(1, 1, 0, 1); // Set alpha to 1 (visible)
			}
			else
			{
				comboImageSlots[i].color = new Color(1, 1, 1, 0); // Set alpha to 0 (invisible)
			}
		}

		UpdateUpcomingCombos();
	}

	private Sprite GetDirectionSprite(string direction)
	{
		switch (direction.ToLower())
		{
			case "left":
				return leftImage;
			case "right":
				return rightImage;
			case "up":
				return upImage;
			case "down":
				return downImage;
			default:
				return null;
		}
	}

	private void UpdateUpcomingCombos()
	{
		for (int i = 0;i < upcomingComboImages.Length; i++)
		{
			int comboIndex = currentComboIndex + i;
			if (comboIndex < currentRoundCombos.Count)
			{
				ComboScriptableObject comboSO = currentRoundCombos[comboIndex];
				upcomingComboImages[i].sprite = comboSO.image;
				upcomingComboImages[i].color = new Color(1, 1, 0, 1); // Set alpha to 1 (visible)
			}
			else
			{
				upcomingComboImages[i].color = new Color(1, 1, 1, 0); // Set alpha to 0 (invisible)
			}
		}
	}

	public Combo GetCurrentCombo()
	{
		return currentCombo;
	}
}
