using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class CSVReader
{
	//,(?=(?:[^""]*""[^""]*"")*(?![^""]*""))|
	static string SPLIT_RE = @";";
    static string LINE_SPLIT_RE = @"\r\n|n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read( string file)
    {
        Debug.Log("Reading CSV Data");
		// Read data
		var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

		// split into lines
		var lines = Regex.Split(data.text, LINE_SPLIT_RE);
		//Debug.Log("lines count " + lines.Length);
		
		
		// return empty if split was unsucessfull
		string[] header = Regex.Split(lines[0], SPLIT_RE);

		//for (int i = 0; i < header.Length; i++)
		//{
		//	Debug.Log(header[i]);
		//}

		// create dict for each entry
		if (lines.Length <= 1) return list;

		//remove empty lines

		for (int i = 1; i < lines.Length; i++)
		{

			string[] values = Regex.Split(lines[i], SPLIT_RE);

			//Debug.Log("line" + i + values.Length);
			if (values.Length == 0 || values[0] == string.Empty) continue;

            Dictionary<string, object> entry = new Dictionary<string, object>();

			for (int j = 0; j < header.Length && j < values.Length; j++)
			{
                string curValue = values[j];
                curValue = curValue.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

				//Debug.Log($"adding entry. key: {header[j]}, value {curValue}");

				entry.Add(header[j], curValue);
    		}

            list.Add(entry);


            
		}
        Debug.Log("entries count" + list.Count);
        
        

        return list;
    }
}

