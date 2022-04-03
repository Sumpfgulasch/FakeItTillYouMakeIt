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

    [Header("Settings")] [Range(0, 1f)] public float BackgroundFadeOutTime;
    [Range(0, 1f)] public float BackgroundFadeInTime;
    [Range(0, 1f)] public float TextFadeInTime;
    [Range(0, 3f)] public float TextSpawnRyhthm;
    [Range(0, 1f)] public float SpeakerTimeBefore;
    [Range(0, 5f)] public float StatementTimeBefore;
    [Range(0, 5f)] public float StatementTimeAfter;
    [Range(0, 1f)] public float AnswersSpawnRhythm;

    private CanvasGroup[] Answers => AnswersParent.GetComponentsInChildren<CanvasGroup>();


    void Start()
    {
        NextScreen(true, true, true);
    }




    private void NextScreen(bool isNewSpeaker, bool isNewLocation, bool isNewSituation)
    {
        var sequence = DOTween.Sequence();

        sequence.AppendInterval(2f);

        sequence.Append(FadeBackground(false, BackgroundFadeOutTime));
        sequence.AppendCallback(() =>
        {
            Statement.alpha = 0;
            SpeakerName.alpha = 0;
            SpeakerLocation.alpha = 0;
            Answers.ForEach(answer => answer.alpha = 0);

        });
        sequence.Append(FadeBackground(true, BackgroundFadeInTime));

        if (isNewSpeaker)
        {
            sequence.AppendInterval(SpeakerTimeBefore);
            sequence.Append(SpeakerName.DOFade(1f, TextFadeInTime));
            sequence.AppendInterval(TextSpawnRyhthm);
            sequence.Append(SpeakerLocation.DOFade(1f, TextFadeInTime));
            sequence.AppendInterval(StatementTimeBefore);
        }

        sequence.Append(Statement.DOFade(1f, TextFadeInTime));
        sequence.AppendInterval(StatementTimeAfter);
        Answers.ForEach(answer =>
        {
            sequence.Append(answer.DOFade(1f, TextFadeInTime));
            sequence.AppendInterval(AnswersSpawnRhythm);
        });
    }

    private Tweener FadeBackground(bool fadeIn, float duration)
    {
        var from = fadeIn ? 1f : 0;
        var to = fadeIn ? 0 : 1f;
        return DOVirtual.Float(from, to, duration, alpha => BackgroundFade.material.SetFloat("_Fading", alpha));
    }
}