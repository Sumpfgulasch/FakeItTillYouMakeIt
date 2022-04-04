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
    [Range(0, 1f)] public float BackgroundLightIntensifyTime;
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

        // 1. Fade out
        sequence.Append(FadeBackgroundLight(BackgroundLightMaxIntensity, 0, BackgroundFadeOutTime));
        sequence.Join(FadeBackgroundImage(BackgroundFade1Value, BackgroundFadeOutTime));
        sequence.Join(SpeakerName.DOFade(0f, BackgroundFadeOutTime).SetEase(Ease.OutCirc));
        sequence.Join(SpeakerLocation.DOFade(0f, BackgroundFadeOutTime).SetEase(Ease.OutCirc));
        sequence.Join(Statement.DOFade(0f, BackgroundFadeOutTime).SetEase(Ease.OutCirc));
        Answers.ForEach(answer =>
        {
            sequence.Join(answer.DOFade(0, BackgroundFadeOutTime).SetEase(Ease.OutCirc));
        });

        sequence.AppendInterval(BackgroundFadeInDelay);

        // 2. FADE IN
        // Background
        sequence.Append(FadeBackgroundLight(0, BackgroundLightMidIntensity, BackgroundFadeInTime));
        sequence.Join(FadeBackgroundImage(BackgroundFadeHalfValue, BackgroundFadeInTime).SetEase(Ease.OutCubic));

        // Speaker
        if (isNewSpeaker)
        {
            sequence.AppendInterval(SpeakerTimeBefore);
            sequence.Append(SpeakerName.DOFade(1f, TextFadeInTime));
            sequence.AppendInterval(SpeakerSpawnRyhthm);
            sequence.Append(SpeakerLocation.DOFade(1f, TextFadeInTime));
            sequence.AppendInterval(StatementTimeBefore);
        }

        // Statement & light
        sequence.Append(Statement.DOFade(1f, 0));
        sequence.Join(FadeBackgroundLight(BackgroundLightMidIntensity, BackgroundLightMaxIntensity, BackgroundLightIntensifyTime));

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
        return DOVirtual.Float(from, targetValue, duration, alpha => BackgroundFade.material.SetFloat("_Fading", alpha));
    }
}