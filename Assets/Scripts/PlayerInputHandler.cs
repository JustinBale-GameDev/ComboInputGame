using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // Attributes
    private Combo currentCombo;
    public int inputIndex;
    public ComboManager comboManager;
    public InputActionAsset inputActions;
	private InputAction comboActions;

	private void Awake()
	{
		comboActions = inputActions.FindActionMap("Gameplay").FindAction("ComboInputs");
	}

	private void OnEnable()
	{
		comboActions.performed += OnComboInput;
		comboActions.Enable();
	}

	private void OnDisable()
	{
		comboActions.performed -= OnComboInput;
		comboActions.Disable();
	}

	private void Start()
	{
		comboManager = FindObjectOfType<ComboManager>();
        SetCurrentCombo(comboManager.GetCurrentCombo());
	}

	// Method to set the current combo
	public void SetCurrentCombo(Combo combo)
    {
        currentCombo = combo;
        inputIndex = 0;
        comboManager.UpdateUI();
		//Debug.Log($"SetCurrentCombo: New combo set - {currentCombo.GetName()}");
		//Debug.Log($"New combo sequence: {string.Join(", ", currentCombo.GetSequence())}");
	}

    public void OnComboInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            string input = context.control.name;
			Debug.Log(HandleInput(input));
        }
    }

	// Method to map input keys to directions
	private string MapInputToDirection(string input)
	{
		switch (input)
		{
			case "w":
				return "Up";
			case "a":
				return "Left";
			case "s":
				return "Down";
			case "d":
				return "Right";
			default:
				return "";
		}
	}

	// Method of handle player input
	public string HandleInput(string input)
    {
		if (currentCombo == null)
		{
			return "No current combo";
		}

		string mappedInput = MapInputToDirection(input);
		//Debug.Log($"Handling input: {mappedInput}, expected: {currentCombo.GetSequence()[inputIndex]}");
		if (mappedInput == currentCombo.GetSequence()[inputIndex])
        {
			comboManager.comboImageSlots[inputIndex].color = new Color(0, 1, 0, 1); // Change color to indicate success
            inputIndex++;
            if (inputIndex == currentCombo.GetSequence().Count)
            {
                comboManager.LoadNextCombo();
                SetCurrentCombo(comboManager.GetCurrentCombo());
				return "Combo Completed";
			}
            return "Input Correct";
        }
        else
        {
			ResetInput();
            return "Input Incorrect";
        }
    }

    // Method to reset input index and image alpha
    public void ResetInput()
    {
        inputIndex = 0;

		// Reset the colors of the combo images
		for (int i = 0; i < comboManager.comboImageSlots.Length; i++)
        {
			if (i < currentCombo.GetSequence().Count)
			{
				comboManager.comboImageSlots[i].color = new Color(1, 1, 1, 1); // Reset to default color
			}
			else
			{
				comboManager.comboImageSlots[i].color = new Color(1, 1, 1, 0); // Set alpha to 0 for empty slots
			}
		}
    }
}
