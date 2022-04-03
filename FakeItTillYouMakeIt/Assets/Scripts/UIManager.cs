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
    [Range(0, 1f)] public float BackgroundFadeInTime;
    [Range(0, 1f)] public float TextFadeInTime;
    [Range(0, 3f)] public float TextSpawnRyhthm;
    [Range(0, 5f)] public float StatementTimeAfter;
    [Range(0, 1f)] public float AnswersSpawnRhythm;

    public CanvasGroup[] Answers => AnswersParent.transform.Cast<CanvasGroup>().ToArray();


    // Start is called before the first frame update
    void Start()
    {
        NextScreen(true, true, true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void NextScreen(bool isNewSpeaker, bool isNewLocation, bool isNewSituation)
    {
        DOVirtual.Float(0, 1f, BackgroundFadeOutTime, alpha => { BackgroundFade.material.SetFloat("_Fading", alpha); })
            .OnComplete(() =>
            {
                Statement.alpha = 0;
                Answers.ForEach(answer => answer.alpha = 0);

                var sequence = DOTween.Sequence();
                sequence.Append(DOVirtual.Float(1f, 0, BackgroundFadeInTime,
                    alpha => { BackgroundFade.material.SetFloat("_Fading", alpha); }));

                if (isNewSpeaker)
                {
                    SpeakerName.alpha = 0;
                    SpeakerLocation.alpha = 0;
                    sequence.Append(SpeakerName.DOFade(1f, TextFadeInTime));
                    sequence.AppendInterval(TextSpawnRyhthm);
                }
                else if (isNewLocation)
                {
                    SpeakerLocation.alpha = 0;
                    sequence.Append(SpeakerName.DOFade(1f, TextFadeInTime));
                    sequence.AppendInterval(TextSpawnRyhthm);
                }

                sequence.Append(Statement.DOFade(1f, TextFadeInTime));
                sequence.AppendInterval(StatementTimeAfter);
                Answers.ForEach(answer =>
                {
                    sequence.Append(answer.DOFade(1f, TextFadeInTime));
                    sequence.AppendInterval(AnswersSpawnRhythm);
                });
            });
    }
}