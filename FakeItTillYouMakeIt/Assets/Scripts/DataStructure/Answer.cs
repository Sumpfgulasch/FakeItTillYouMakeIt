using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Answer
{
	public string Index;
	public string Text;
	public List<Statement> NegativeConditions;
	public List<Statement> AddedStatements;

	public Answer(Answer answer)
	{
		Index = answer.Index;
		Text = answer.Text;
		NegativeConditions = answer.NegativeConditions;
		AddedStatements = answer.AddedStatements;
	}

	public Answer(string index, string text, List<Statement> negativeConditions, List<Statement> addedStatements) 
	{
		Index = index;
		Text = text;
		NegativeConditions = negativeConditions;
		AddedStatements = addedStatements;
	}
}
