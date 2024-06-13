using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo
{
    // Attributes
    public string Name {  get; private set; }
    public List<string> Sequence { get; private set; }
    public Sprite Image { get; private set; }

    // Constructor
    public Combo(string name, List<string> sequence, Sprite image)
    {
        Name = name;
        Sequence = sequence;
        Image = image;
    }

    // Methods to get attrubutes
    public string GetName()
    {
        return Name;
    }
	public List<string> GetSequence()
	{
		return Sequence;
	}
	public Sprite GetImage()
	{
		return Image;
	}
}
