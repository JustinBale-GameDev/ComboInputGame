using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // Attributes
    private Combo currentCombo;
    private int inputIndex;
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
    }

    public void OnComboInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            string input = context.control.name;
            Debug.Log(input.ToUpper());
            HandleInput(input);
        }
    }

    // Method of handle player input
    public string HandleInput(string input)
    {
        if (input == currentCombo.GetSequence()[inputIndex])
        {
            inputIndex++;
            if (inputIndex == currentCombo.GetSequence().Count)
            {
                string result = "Combo Completed";
                comboManager.LoadNextCombo();
                SetCurrentCombo(comboManager.GetCurrentCombo());
				return result;
			}
            return "Input Correct";
        }
        else
        {
            ResetInput();
            return "Input Incorrect";
        }
    }

    // Method to reset input index
    public void ResetInput()
    {
        inputIndex = 0;
    }

}
