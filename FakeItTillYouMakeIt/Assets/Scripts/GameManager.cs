using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DataBase DataBase;
    public UIManager UIManager;

    private int _currentSituationIndex;

    private int _currentSlideIndex;
    public int MaxPlayerLifes;

    private int _playerLifes;

    private Situation CurrentSituation => DataBase.SituationsPreview[_currentSituationIndex];
    private Slide CurrentSlide => CurrentSituation.Slides[_currentSlideIndex];
    private int CurrentMaxSlides => CurrentSituation.Slides.Count;
    private string CurrentQuestion => CurrentSlide.Text;
    private string CurrentNPC => CurrentSlide.NPC;
    private string CurrentLocation => CurrentSlide.Place;
    private List<Answer> CurrentAnswers => CurrentSlide.PossibleAnswers;
    private bool HasLifes => _playerLifes > 0;

    void Start()
    {
        _playerLifes = MaxPlayerLifes;

        UIManager.LoadSlideTexts(CurrentSlide);
    }

    public void OnAnswerChosen(Answer answer)
    {
        var sequence = DOTween.Sequence();

        IncreaseSlideIndex();
        var nextSlideData = GetNextSlideData();
        var correctAnswer = EvaluateAnswer(answer);

        if (correctAnswer)
            sequence.Append(UIManager.ShowCorrectReaction());
        else
        {
            sequence.Append(UIManager.ShowErrorReaction());

            _playerLifes--;
            if (!HasLifes)
            {
                sequence.Append(UIManager.GoToLostScreen());
                return;
            }
        }

        sequence.Append(nextSlideData.IsEnd
            ? UIManager.GoToEndScreen()
            : UIManager.GoToSlide(CurrentSlide, nextSlideData));
    }

    public void OnContinueButtonClicked()
    {
        IncreaseSlideIndex();
        UIManager.GoToSlide(CurrentSlide, GetNextSlideData());
    }

    private bool EvaluateAnswer(Answer answer)
    {
        // to do: do stuff
        return true;
    }

    private ScreenData GetNextSlideData()
    {
        var nextScreenData = new ScreenData();

        if (_currentSituationIndex < DataBase.Situations.Count)
            nextScreenData = new ScreenData(true, true, true, false);
        else
            nextScreenData = new ScreenData(false, false, false, true);

        return nextScreenData;
    }

    private void IncreaseSlideIndex()
    {
        _currentSlideIndex++;
        if (_currentSlideIndex < CurrentMaxSlides)
            return;

        _currentSlideIndex = 0;
        _currentSituationIndex++;
    }

    public struct ScreenData
    {
        public bool IsNewSituation;
        public bool IsNewNPC;
        public bool IsNewLocation;
        public bool IsEnd;

        public ScreenData(bool isNewSituation, bool isNewNpc, bool isNewLocation, bool isEnd)
        {
            IsNewSituation = isNewSituation;
            IsNewNPC = isNewNpc;
            IsNewLocation = isNewLocation;
            IsEnd = isEnd;
        }
    }


}