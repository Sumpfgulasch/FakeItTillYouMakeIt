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

        UIManager.FillScreenTexts(CurrentSlide);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAnswerChosen()
    {
        var sequence = DOTween.Sequence();

        var correctAnswer = EvaluateAnswer();
        var nextSlideData = GetNextSlideData();

        if (correctAnswer)
            sequence.Append(UIManager.ShowCorrectReaction());
        else
        {
            sequence.Append(UIManager.ShowErrorReaction());

            _playerLifes--;
            if (!HasLifes)
            {
                sequence.Append(UIManager.ShowLoseScreen());
                return;
            }
        }

        sequence.Append(!nextSlideData.IsEnd
            ? UIManager.GoToNextScreen(CurrentSlide, nextSlideData)
            : UIManager.ShowEndScreen());
    }

    private bool EvaluateAnswer()
    {
        return false;
    }

    private ScreenData GetNextSlideData()
    {
        var nextScreenData = new ScreenData();
        _curSlideIndex++;

        if (_curSlideIndex < CurrentMaxSlides)
            return nextScreenData;

        _curSlideIndex = 0;
        _curSituationIndex++;

        if (_curSituationIndex < DataBase.Situations.Count)
            nextScreenData = new ScreenData(true, true, true, false);
        else
            nextScreenData = new ScreenData(false, false, false, true);

        return nextScreenData;
    }

    private void EvaluateError()
    {
        _playerLifes--;
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