using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Situation 
{
	public string Index;
	public List<Slide> Slides;

	public Situation(string index, List<Slide> slides) 
	{
		Index = index;
		Slides = slides;
	
	}
}
