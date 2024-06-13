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
	public Image currentComboImage;
	public TMP_Text currentComboName;
	public Image[] comboImageSlots; // Array to hold all the spaces for the current combo input sequence. (Directional images will fill this up depending on the combo)
	public Sprite leftImage; // Image representing left direction
	public Sprite rightImage; // Image representing right direction
	public Sprite upImage; // Image representing up direction
	public Sprite downImage; // Image representing down direction


	private void Start()
	{
		currentComboIndex = 0;
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
		}
		else
		{
			Debug.Log("All combos completed");
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
				Debug.Log("Slot is empty");
			}
		}
	}

	private Sprite GetDirectionSprite(string direction)
	{
		switch (direction.ToLower())
		{
			case "left":
				Debug.Log("Assigning leftImage");
				return leftImage;
			case "right":
				Debug.Log("Assigning rightImage");
				return rightImage;
			case "up":
				Debug.Log("Assigning upImage");
				return upImage;
			case "down":
				Debug.Log("Assigning downImage");
				return downImage;
			default:
				return null;
		}
	}

	public Combo GetCurrentCombo()
	{
		return currentCombo;
	}
}
