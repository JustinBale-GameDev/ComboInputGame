using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    // Attributes
    private Combo currentCombo;
    private int inputIndex;

    // Method to set the current combo
    public void SetCurrentCombo(Combo combo)
    {
        currentCombo = combo;
        inputIndex = 0;
    }

    // Method of handle player input
    public string HandleInput(string input)
    {
        if (input == currentCombo.GetSequence()[inputIndex])
        {
            inputIndex++;
            if (inputIndex == currentCombo.GetSequence().Count)
            {
                return "Combo Completed";
            }
        }
        else
        {
            ResetInput();
            return "Input Incorrect";
        }
        return "Input Correct";
    }

    // Method to reset input index
    public void ResetInput()
    {
        inputIndex = 0;
    }

}
