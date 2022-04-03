using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Statement
{
	public string _index;
	public string _meaning;

	public Statement(Statement statement) 
	{
		_index = statement._index;
		_meaning = statement._meaning;
	}

	public Statement(string index, string meaning) 
	{
		_index = index;
		_meaning = meaning;
	}
}
