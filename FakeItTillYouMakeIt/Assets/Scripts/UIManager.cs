using DG.Tweening;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public GameManager GameManager;
    public CanvasGroup EntireCanvasGroup;
    public CanvasGroup SpeakerParent;
    public TMP_Text NPCName;
    public TMP_Text NPCLocation;
    public CanvasGroup Age;
    public TMP_Text Question;
    public LayoutGroup AnswersParent;
    public GameObject AnswerPrefab;
    public AnswerManager AnswerManager;
    public MeshRenderer Background;
    public MeshRenderer BackgroundFade;

    [Header("Settings")] [Range(0, 1f)] public float BackgroundFadeOutTime;
    [Range(0, 1f)] public float BackgroundFadeInDelay;
    [Range(0, 1f)] public float BackgroundFadeInTime;
    [Range(0, 1f)] public float BackgroundLightMidIntensity;
    [Range(0, 1f)] public float BackgroundLightMaxIntensity;
    [Range(0, 1f)] public float BackgroundLightIntensifyTime;
    [Range(0, 1f)] public float TextFadeInTime;
    [Range(0, 1f)] public float SpeakerTimeBefore;
    [Range(0, 2f)] public float SpeakerSpawnRyhthm;
    [Range(0, 5f)] public float StatementTimeBefore;
    [Range(0, 5f)] public float StatementTimeAfter;
    [Range(0, 1f)] public float AnswersSpawnRhythm;

    private CanvasGroup[] Answers => AnswersParent.GetComponentsInChildren<CanvasGroup>();
    private Sequence _screenSequence;


    public Sequence GoToNextScreen(Slide screen, GameManager.ScreenData screenPlayerData)
    {
        var sequence = DOTween.Sequence();

        // 1. Fade out
        sequence.Append(FadeBackgroundLight(BackgroundLightMaxIntensity, 0, BackgroundFadeOutTime));
        sequence.Join(NPCName.DOFade(0f, BackgroundFadeOutTime).SetEase(Ease.OutCirc));
        sequence.Join(NPCLocation.DOFade(0f, BackgroundFadeOutTime).SetEase(Ease.OutCirc));
        sequence.Join(Question.DOFade(0f, BackgroundFadeOutTime).SetEase(Ease.OutCirc));
        Answers.ForEach(answer => { sequence.Join(answer.DOFade(0, BackgroundFadeOutTime).SetEase(Ease.OutCirc)); });

        sequence.AppendInterval(BackgroundFadeInDelay);

        // 2. Load next screen
        sequence.AppendCallback(() => { FillScreenTexts(screen); });

        // 3. FADE IN
        // Background
        sequence.Append(FadeBackgroundLight(0, BackgroundLightMidIntensity, BackgroundFadeInTime));

        // NPC
        sequence.AppendInterval(SpeakerTimeBefore);
        sequence.Append(NPCName.DOFade(1f, TextFadeInTime));
        sequence.AppendInterval(SpeakerSpawnRyhthm);
        sequence.Append(NPCLocation.DOFade(1f, TextFadeInTime));
        sequence.AppendInterval(StatementTimeBefore);


        // Question & light
        sequence.Append(Question.DOFade(1f, 0));
        sequence.Join(FadeBackgroundLight(BackgroundLightMidIntensity, BackgroundLightMaxIntensity,
            BackgroundLightIntensifyTime));

        sequence.AppendInterval(StatementTimeAfter);
        Answers.ForEach(answer =>
        {
            // Answers
            sequence.Append(answer.DOFade(1f, 0));
            sequence.AppendInterval(AnswersSpawnRhythm);
        });

        return sequence;
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

    public Tween ShowLoseScreen()
    {
        return null;
    }

    public Tween ShowEndScreen()
    {
        return null;
    }

    public void FillScreenTexts(Slide slide)
    {
        NPCName.text = slide.NPC;
        NPCLocation.text = slide.Place;
        Question.text = slide.Text;

        foreach (Transform child in AnswersParent.transform)
            RemoveAnswer(child);
        foreach (var answer in slide.PossibleAnswers)
            CreateAnswer(answer);
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