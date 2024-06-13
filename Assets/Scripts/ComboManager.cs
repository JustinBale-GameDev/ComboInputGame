using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboManager : MonoBehaviour
{
    public List<ScriptableObject> combos;
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


	private void Start()
	{
		currentComboIndex = 0;
		playerInputHandler = FindAnyObjectByType<PlayerInputHandler>();
		LoadNextCombo();
	}

	public void LoadNextCombo()
	{
		if (currentComboIndex < combos.Count)
		{
			ComboScriptableObject comboSO = (ComboScriptableObject)combos[currentComboIndex];
			currentCombo = new Combo(comboSO.comboName, comboSO.sequence, comboSO.image);
			currentComboIndex++;
			UpdateUI();
			playerInputHandler.SetCurrentCombo(currentCombo); // Update the PlayerInputHandler with the new combo
			//Debug.Log($"LoadNextCombo: New combo loaded - {currentCombo.GetName()}");
			//Debug.Log($"Loaded combo sequence: {string.Join(", ", currentCombo.GetSequence())}");
		}
		else
		{
			Debug.Log("All combos completed");
			currentCombo = null;
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
				comboImageSlots[i].color = new Color(1, 1, 1, 1); // Set alpha to 1 (visible)
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
			if (comboIndex < combos.Count)
			{
				ComboScriptableObject comboSO = (ComboScriptableObject)combos[comboIndex];
				upcomingComboImages[i].sprite = comboSO.image;
				upcomingComboImages[i].color = new Color(1, 1, 1, 1); // Set alpha to 1 (visible)
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
