using System;
using System.Xml.Serialization;
using DG.Tweening;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup StartScreen;
    public CanvasGroup QuestionScreen;
    public CanvasGroup NarrativeScreen;
    public CanvasGroup EndScreen;
    public CanvasGroup LostScreen;
    public CanvasGroup HUD;
    public CanvasGroup NPCTexts;
    [Space]
    public TMP_Text NPCName;
    public TMP_Text NPCLocation;
    public TMP_Text Question;
    public LayoutGroup AnswersParent;
    public MeshRenderer Background;
    public MeshRenderer BackgroundFade;
    public TMP_Text NarrativeText;
    public TMP_Text NarrativeButton;
    public Button StartButton;
    [Space]
    public GameManager GameManager;
    public GameObject AnswerPrefab;

    [Header("Settings")]
    [Range(0, 1f)] public float BackgroundFadeOutTime;
    [Range(0, 1f)] public float BackgroundFadeInDelay;
    [Range(0, 1f)] public float BackgroundFadeInTime;
    [Range(0, 1f)] public float BackgroundLightMidIntensity;
    [Range(0, 1f)] public float BackgroundLightMaxIntensity;
    [Range(0, 1f)] public float BackgroundLightIntensifyTime;
    [Range(0, 1f)] public float TextFadeInTime;
    [Range(0, 1f)] public float TextFadeInTime2;
    [Range(0, 1f)] public float SpeakerTimeBefore;
    [Range(0, 2f)] public float SpeakerSpawnRyhthm;
    [Range(0, 5f)] public float QuestionTimeBefore;
    [Range(0, 5f)] public float QuestionTimeAfter;
    [Range(0, 1f)] public float AnswersSpawnRhythm;
    [Range(0, 5f)] public float NarrativeTextTimeAfter;

    private CanvasGroup[] Answers => AnswersParent.GetComponentsInChildren<CanvasGroup>();
    private Sequence _screenSequence;
    private CanvasGroup _currentScreen;

    private void Start()
    {
        _currentScreen = StartScreen;

        StartButton.onClick.AddListener(GameManager.OnContinueButtonClicked);
        NarrativeButton.GetComponent<Button>().onClick.AddListener(GameManager.OnContinueButtonClicked);
    }


    public Sequence GoToSlide(Slide slide, GameManager.ScreenData screenPlayerData)
    {
        var sequence = DOTween.Sequence();

        // 1. FADE OUT
        JoinFadeOutScreenTo(sequence, _currentScreen);
        if (slide is QuestionSlide)
            JoinFadeOutScreenTo(sequence, NPCTexts);

        sequence.AppendInterval(BackgroundFadeInDelay);

        // 2. Load next screen data
        sequence.AppendCallback(() => { LoadSlideTexts(slide); });

        // 3. FADE IN
        switch (slide)
        {
            case QuestionSlide:
            {
                _currentScreen = QuestionScreen;
                sequence.AppendCallback(() => { _currentScreen.gameObject.SetActive(true); });

                // Background
                sequence.Append(FadeBackgroundLight(0, BackgroundLightMidIntensity, BackgroundFadeInTime));

                // NPC
                sequence.AppendInterval(SpeakerTimeBefore);
                sequence.Append(NPCName.DOFade(1f, TextFadeInTime));
                sequence.AppendInterval(SpeakerSpawnRyhthm);
                sequence.Append(NPCLocation.DOFade(1f, TextFadeInTime));
                sequence.AppendInterval(QuestionTimeBefore);

                // Question & background
                sequence.Append(Question.DOFade(1f, 0));
                sequence.Join(FadeBackgroundLight(BackgroundLightMidIntensity, BackgroundLightMaxIntensity,
                    BackgroundLightIntensifyTime));

                sequence.AppendInterval(QuestionTimeAfter);
                Answers.ForEach(answer =>
                {
                    // Answers
                    sequence.Append(answer.DOFade(1f, 0));
                    sequence.AppendInterval(AnswersSpawnRhythm);
                });
                break;
            }

            case NarrativeSlide:
            {
                _currentScreen = NarrativeScreen;
                sequence.AppendCallback(() => { _currentScreen.gameObject.SetActive(true); });

                // Text & button
                sequence.Append(NarrativeText.DOFade(1f, TextFadeInTime2));
                sequence.AppendInterval(NarrativeTextTimeAfter);
                sequence.Append(NarrativeButton.DOFade(1f, TextFadeInTime2));
                break;
            }

        }
        return sequence;
    }

    private void JoinFadeOutScreenTo (Sequence sequence, CanvasGroup screen)
    {
        var children = screen.GetComponentsInChildren<TMP_Text>();

        children.ForEach(text => sequence.Join(text.DOFade(0, BackgroundFadeOutTime)));
        sequence.AppendCallback(() => screen.gameObject.SetActive(false));
    }

    private Tweener FadeBackgroundLight(float from, float to, float duration)
    {
        return DOVirtual.Float(from, to, duration, alpha => Background.material.SetFloat("_MainColorIntensity", alpha));
    }

    private Tweener FadeBackgroundImage(float targetValue, float duration)
    {
        var from = BackgroundFade.material.GetFloat("_Fading");
        return DOVirtual.Float(from, targetValue, duration,
            alpha => BackgroundFade.material.SetFloat("_Fading", alpha));
    }

    public Tween ShowCorrectReaction()
    {
        return null;
    }

    public Tween ShowErrorReaction()
    {
        return null;
    }

    public Tween GoToLostScreen()
    {
        _currentScreen = LostScreen;
        return null;
    }

    public Tween GoToEndScreen()
    {
        _currentScreen = EndScreen;
        return null;
    }

    public void LoadSlideTexts(Slide slide)
    {
        NPCName.text = slide.NPC;
        NPCLocation.text = slide.Place;

        switch (slide)
        {
            case QuestionSlide:
            {
                Question.text = slide.Text;
                foreach (Transform child in AnswersParent.transform)
                    RemoveAnswer(child);
                foreach (var answer in slide.PossibleAnswers)
                    CreateAnswer(answer);
                break;
            }
            case NarrativeSlide:
            {
                NarrativeText.text = slide.Text;
                break;
            }
        }


    }

    private void RemoveAnswer(Transform transform)
    {
        transform.GetComponent<AnswerHolder>().OnClick -= GameManager.OnAnswerChosen;
        Destroy(transform.gameObject);
    }

    private void CreateAnswer(Answer answer)
    {
        var answerObject = Instantiate(AnswerPrefab, AnswersParent.transform);
        answerObject.GetComponentInChildren<TMP_Text>().text = answer.Text;
        answerObject.GetComponent<AnswerHolder>().Answer = answer;
        answerObject.GetComponent<AnswerHolder>().OnClick += GameManager.OnAnswerChosen;
    }
}