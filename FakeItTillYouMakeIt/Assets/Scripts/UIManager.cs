using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")] public Canvas Canvas;
    public CanvasGroup EntireCanvasGroup;
    public CanvasGroup SpeakerParent;
    public TMP_Text SpeakerName;
    public TMP_Text SpeakerLocation;
    public CanvasGroup Age;
    public TMP_Text Statement;
    public LayoutGroup AnswersParent;
    public MeshRenderer Background;
    public MeshRenderer BackgroundFade;

    [Header("Settings")]
    [Range(0, 1f)] public float BackgroundFadeOutTime;
    [Range(0, 1f)] public float BackgroundFadeInDelay;
    [Range(0, 1f)] public float BackgroundFadeInTime;
    [Range(0, 1f)] public float BackgroundFade1Value;
    [Range(0, 1f)] public float BackgroundFadeHalfValue;
    [Range(0, 1f)] public float BackgroundLightMidIntensity;
    [Range(0, 1f)] public float BackgroundLightMaxIntensity;
    [Range(0, 1f)] public float TextFadeInTime;
    [Range(0, 3f)] public float TextSpawnRyhthm;
    [Range(0, 1f)] public float SpeakerTimeBefore;
    [Range(0, 2f)] public float SpeakerSpawnRyhthm;
    [Range(0, 5f)] public float StatementTimeBefore;
    [Range(0, 5f)] public float StatementTimeAfter;
    [Range(0, 1f)] public float AnswersSpawnRhythm;

    private CanvasGroup[] Answers => AnswersParent.GetComponentsInChildren<CanvasGroup>();
    private Sequence _screenSequence;

    void Start()
    {
        _screenSequence = NextScreen(true, true, true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _screenSequence.Kill();
            _screenSequence = NextScreen(true, true, true);
        }
    }


    private Sequence NextScreen(bool isNewSpeaker, bool isNewLocation, bool isNewSituation)
    {
        var sequence = DOTween.Sequence();

        sequence.Append(FadeBackgroundLight(0, BackgroundFadeOutTime).SetEase(Ease.InExpo));
        sequence.Join(FadeBackgroundImage(1f, BackgroundFadeOutTime).SetEase(Ease.InExpo));

        sequence.AppendInterval(BackgroundFadeInDelay);

        sequence.Join(SpeakerName.DOFade(0f, BackgroundFadeOutTime));
        sequence.Join(SpeakerLocation.DOFade(0f, BackgroundFadeOutTime));
        sequence.Join(Statement.DOFade(0f, BackgroundFadeOutTime));
        Answers.ForEach(answer =>
        {
            sequence.Join(answer.DOFade(0, BackgroundFadeOutTime));
        });

        sequence.AppendCallback(() =>
        {
            Statement.alpha = 0;
            SpeakerName.alpha = 0;
            SpeakerLocation.alpha = 0;
            Answers.ForEach(answer => answer.alpha = 0);

        });

        // Fade in background
        sequence.Append(FadeBackgroundLight(BackgroundLightMidIntensity, BackgroundFadeInTime).SetEase(Ease.OutCubic));
        sequence.Join(FadeBackgroundImage(BackgroundFadeHalfValue, BackgroundFadeInTime).SetEase(Ease.OutCubic));

        if (isNewSpeaker)
        {
            sequence.AppendInterval(SpeakerTimeBefore);
            sequence.Append(SpeakerName.DOFade(1f, TextFadeInTime));
            sequence.AppendInterval(SpeakerSpawnRyhthm);
            sequence.Append(SpeakerLocation.DOFade(1f, TextFadeInTime));
            sequence.AppendInterval(StatementTimeBefore);
        }

        sequence.Append(Statement.DOFade(1f, 0));
        sequence.Join(FadeBackgroundLight(BackgroundLightMaxIntensity, 0.2f));

        sequence.AppendInterval(StatementTimeAfter);
        Answers.ForEach(answer =>
        {
            sequence.Append(answer.DOFade(1f, 0));
            sequence.AppendInterval(AnswersSpawnRhythm);
        });

        return sequence;
    }

    private Tweener FadeBackgroundLight(float targetValue, float duration)
    {
        var from = Background.material.GetFloat("_MainColorIntensity");
        return DOVirtual.Float(from, targetValue, duration, alpha => Background.material.SetFloat("_MainColorIntensity", alpha)); // _Fading
    }

    private Tweener FadeBackgroundImage(float targetValue, float duration)
    {
        var from = BackgroundFade.material.GetFloat("_Fading");
        return DOVirtual.Float(from, targetValue, duration, alpha => BackgroundFade.material.SetFloat("_Fading", alpha));
    }
}