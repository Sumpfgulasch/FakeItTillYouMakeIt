using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Answer
{
	string index;
	string text;
	List<Statement> negativeConditions;
	List<Statement> addedStatements;

	public Answer(Answer answer) {
		index = answer.index;
		text = answer.text;
		negativeConditions = answer.negativeConditions;
		addedStatements = answer.addedStatements;
	}
}
