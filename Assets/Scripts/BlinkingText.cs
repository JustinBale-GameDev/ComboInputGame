using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
	public TMP_Text blinkingText;
	public float blinkDuration = 1.0f; // Total time for one blink cycle

	private void OnEnable()
	{
		StartCoroutine(BlinkText());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator BlinkText()
	{
		while (true)
		{
			// Fade in
			for (float t = 0; t < blinkDuration; t += Time.deltaTime)
			{
				float normalizedTime = t / blinkDuration;
				float alpha = Mathf.Sin(normalizedTime * Mathf.PI); // Smooth fade in and out
				SetAlpha(alpha);
				yield return null;
			}
			// Ensure the text is fully visible before starting the next cycle
			SetAlpha(1);
		}
	}

	private void SetAlpha(float alpha)
	{
		Color color = blinkingText.color;
		color.a = alpha;
		blinkingText.color = color;
	}
}
