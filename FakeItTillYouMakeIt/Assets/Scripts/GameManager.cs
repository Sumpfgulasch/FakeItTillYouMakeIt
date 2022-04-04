using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DataBase DataBase;
    public UIManager UIManager;

    private int _curSituationIndex;
    private int _curSlideIndex;
    public int MaxPlayerLifes;

    private int _playerLifes;

    private Situation CurrentSituation => DataBase.Situations[_curSituationIndex.ToString()];
    private Slide CurrentSlide => CurrentSituation.Slides[_curSlideIndex];
    private int CurrentMaxSlides => CurrentSituation.Slides.Count;
    private string CurrentQuestion => CurrentSlide.Text;
    private string CurrentNPC => CurrentSlide.NPC;
    private string CurrentLocation => CurrentSlide.Place;
    private List<Answer> CurrentAnswers => CurrentSlide.PossibleAnswers;
    private bool HasLifes => _playerLifes > 0;

    void Start()
    {
        _playerLifes = MaxPlayerLifes;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAnswerChosen()
    {
        var sequence = DOTween.Sequence();

        var correctAnswer = EvaluateAnswer();
        if (correctAnswer)
        {
            ProceedToNextSlide();
        }
        else
        {
            // var sequence = DOTween.Sequence();
            sequence.Append(UIManager.ShowErrorReaction());

            EvaluateError();
            if (HasLifes)
            {
                sequence.Append(UIManager.GoToNextScreen(true));
            }
        }
    }

    private bool EvaluateAnswer()
    {
        return false;
    }

    private void IncreaseSlideIndex()
    {

    }

    private void ProceedToNextSlide()
    {
        UIManager.GoToNextScreen(true);
    }

    private void EvaluateError()
    {
        _playerLifes--;
    }


}