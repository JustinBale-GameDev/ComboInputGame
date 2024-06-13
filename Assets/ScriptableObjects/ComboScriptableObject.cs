using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "ScriptableObjects/ComboScriptableObject", order = 1)]
public class ComboScriptableObject : ScriptableObject
{
	public string comboName;
	public List<string> sequence;
	public Sprite image;
}
