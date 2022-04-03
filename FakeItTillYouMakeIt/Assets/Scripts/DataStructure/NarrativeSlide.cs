using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarrativeSlide : Slide
{
	public NarrativeSlide() { }
	public NarrativeSlide(string situationIndexex, string npc, string place, string text)
	{
		SituationIndex = situationIndexex;
		NPC = npc;
		Place = place;
		Text = text;
	}

}
