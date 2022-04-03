using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QuestionSlide: Slide
{
	

	public QuestionSlide() { }
	public QuestionSlide(string situationIndex, string npc, string place, string text, List<Answer> possibleAnswers)
	{
		SituationIndex = situationIndex;
		NPC = npc;
		Place = place;
		Text = text;
		PossibleAnswers = possibleAnswers;
	}
}
