using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputHandler : MonoBehaviour
{
    private Combo currentCombo;
    public int inputIndex;
    public ComboManager comboManager;
	private GameManager gameManager;
    public InputActionAsset inputActions;
	private InputAction upAction;
	private InputAction downAction;
	private InputAction leftAction;
	private InputAction rightAction;
	private string inputName;

	public bool inPlay = true;

	private void Awake()
	{
		upAction = inputActions.FindActionMap("Gameplay").FindAction("Up");
		downAction = inputActions.FindActionMap("Gameplay").FindAction("Down");
		leftAction = inputActions.FindActionMap("Gameplay").FindAction("Left");
		rightAction = inputActions.FindActionMap("Gameplay").FindAction("Right");
	}

	private void Start()
	{
		comboManager = FindObjectOfType<ComboManager>();
		gameManager = FindObjectOfType<GameManager>();
        SetCurrentCombo(comboManager.GetCurrentCombo());
	}

	private void Update()
	{
		if (inPlay)
		{
			CheckInput(upAction);
			CheckInput(downAction);
			CheckInput(leftAction);
			CheckInput(rightAction);
		}
	}

	public void IsPlaying()
	{
		inPlay = true;
	}
	public void IsNotPlaying()
	{
		inPlay = false;
	}

	private void CheckInput(InputAction action)
	{
		if (action.WasPressedThisFrame())
		{
			inputName = action.name;
			Debug.Log(HandleInput(inputName));
		}
	}

	// Method to set the current combo
	public void SetCurrentCombo(Combo combo)
    {
        currentCombo = combo;
        inputIndex = 0;
        comboManager.UpdateUI();
	}

	// Method of handle player input
	public string HandleInput(string input)
    {
		if (currentCombo == null)
		{
			return "No current combo";
		}

		if (input == currentCombo.GetSequence()[inputIndex])
        {
			comboManager.comboImageSlots[inputIndex].color = new Color(0, 1, 0, 1); // Change color to indicate success
            inputIndex++;
            if (inputIndex == currentCombo.GetSequence().Count)
            {
                comboManager.LoadNextCombo();
                SetCurrentCombo(comboManager.GetCurrentCombo());

				gameManager.AddTime(0.5f); // Add time when a combo is completed
				gameManager.AddScore(100); // Add score when a combo is completed

				return "Combo Completed";
			}
            return "Input Correct";
        }
        else
        {
			gameManager.ReduceTime(1f);
			gameManager.ReduceScore(25);
			gameManager.AddError();
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

	private void OnEnable()
	{
		upAction.Enable();
		downAction.Enable();
		leftAction.Enable();
		rightAction.Enable();
	}

	private void OnDisable()
	{
		upAction.Disable();
		downAction.Disable();
		leftAction.Disable();
		rightAction.Disable();
	}
}
