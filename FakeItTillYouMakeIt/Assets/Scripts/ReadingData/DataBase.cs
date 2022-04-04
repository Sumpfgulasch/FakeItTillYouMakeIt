using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class DataBase : MonoBehaviour
{
    public Dictionary<string, Statement> Statements;
    public Dictionary<string, Answer> Answers;
    public Dictionary<string, Situation> Situations;

    public List<Statement> statementsPreview;
    public List<Answer> answersPreview;
    List<Slide> slidesPreview;
    public List<Situation> situationsPreview;

    // Start is called before the first frame update
    void Start()
    {
        FetchStatements();
        statementsPreview = Statements.Values.ToList();

        FetchAnswers();
        answersPreview = Answers.Values.ToList();

        FetchSituations();
        situationsPreview = Situations.Values.ToList();
    }

     public void FetchStatements()
    {
        Debug.Log("Fetching Statements");
        Statements = new Dictionary<string, Statement>();

        List<Dictionary<string, object>> data = CSVReader.Read("StatementsDataTable");

		for (int i = 0; i < data.Count; i++)
		{
            string index = data[i]["Index"].ToString();
            string meaning = data[i]["Meaning"].ToString();

            Statements.Add(index, new Statement(index, meaning));
        }
    }

    public void FetchAnswers()
    {
        Debug.Log("Fetching Answers");

        Answers = new Dictionary<string, Answer>();

        List<Dictionary<string, object>> data = CSVReader.Read("AnswersDataTable");
        Debug.Log("answers count " + data.Count);

        for (int i = 0; i < data.Count; i++)
		{
			string index = data[i]["Index"].ToString();
			string text = data[i]["Text"].ToString();

			//negative conditions
			string negativeConditionsInputString = data[i]["Negative Conditions"].ToString();
			List<Statement> negativeConditions =  TurnStringIntoStatementList(negativeConditionsInputString);


            string addedStatementsInputString = data[i]["Added statements"].ToString();
            List<Statement> addedStatements = TurnStringIntoStatementList(addedStatementsInputString);


            Answer newAswer = new Answer(index, text, negativeConditions, addedStatements);
			Answers.Add(index, newAswer);
		}
	}

    public void FetchSituations()
    {
        Situations = new Dictionary<string, Situation>();


        // fetch slides
        Debug.Log("Fetching Slides");

        List<Dictionary<string, object>> data = CSVReader.Read("SlidesDataTable");

        // for each slide
        for (int i = 0; i < data.Count; i++)
        {
            // ------------ process slide ------------
            string situationIndex = data[i]["Situation Index"].ToString();
            //Debug.Log($"SituationIndex: {situationIndex}");

            string type = data[i]["Type"].ToString();
            //Debug.Log($"Type: {type}");

            string NPC = data[i]["NPC"].ToString();
            //Debug.Log($"NPC: {NPC}");

            string place = data[i]["Place"].ToString();
            //Debug.Log($"Place: {place}");

            string text = data[i]["Text"].ToString();
            //Debug.Log($"Text: {text}");

            string possibleAnswersInputString = data[i]["Possible Answers"].ToString();
            //Debug.Log($"Possible Answers: {possibleAnswersInputString}");


            //possible answers
            Debug.Log("possible answers are:" + possibleAnswersInputString);
            List<Answer> possibleAnswers = TurnStringIntoAnswerList(possibleAnswersInputString);

            // create slide
            Slide curSlide = new Slide();

            if (type == "Q")
            {
                Debug.Log("cur slide type was Q");
                curSlide = new QuestionSlide(situationIndex, NPC, place, text, possibleAnswers);

            }
            else if (type == "N")
            {
                Debug.Log("cur slide type was N");
                curSlide = new NarrativeSlide(situationIndex, NPC, place, text);
            }

            slidesPreview = new List<Slide>();
            slidesPreview.Add(curSlide);


            // check if a situation with that index exist, if yes, add this slide, if not, create a new situation

            if (!Situations.ContainsKey(situationIndex))
            {
                Situations.Add(situationIndex, new Situation(situationIndex, new List<Slide>()));
            }

            Situations[situationIndex].Slides.Add(curSlide);
        }

        Debug.Log("converting to situations");
    }

	private List<Statement> TurnStringIntoStatementList(string inputString)
	{
		string[] splitString = Regex.Split(inputString, ",");
        List<Statement> outputDict = new List< Statement>();

        foreach (string statementindex in splitString)
		{
            string index = statementindex.Trim(' ');
            if (index == string.Empty || index == " ") continue;
            outputDict.Add(GetStatementWithIndex(index));
		}

        return outputDict;
	}

    private List<Answer> TurnStringIntoAnswerList(string inputString)
    {
        string[] splitString = Regex.Split(inputString, ",");
        List<Answer> outputDict = new List<Answer>();

        foreach (string statementindex in splitString)
        {
            string index = statementindex.Trim(' ');
            if (index == string.Empty || index == " ") continue;

            outputDict.Add(GetAnswerWithIndex(index));
        }

        return outputDict;
    }

    public Statement GetStatementWithIndex(string index)
    {
        return Statements[index];
    }

    public Answer GetAnswerWithIndex(string index)
    {
        return Answers[index];
    }


}