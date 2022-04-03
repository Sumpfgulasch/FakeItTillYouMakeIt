using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class UIManager : MonoBehaviour
{
    [Header("References")] public Canvas Canvas;
    public CanvasGroup EntireCanvasGroup;
    public CanvasGroup Speaker;
    public CanvasGroup Age;
    public TMP_Text Statement;
    public MeshRenderer Background;

    [Header("Settings")] [Range(0, 1f)] public float FadeOutTime;
    [Range(0, 1f)] public float FadeInTime;
    [Range(0, 3f)] public float AnswersSpawnDelay;
    [Range(0, 1f)] public float AnswersSpawnRhythm;


    // Start is called before the first frame update
    void Start()
    {
        NextScreen();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void NextScreen()
    {
        var sequence = DOTween.Sequence();
        var backgroundTween =
            DOVirtual.Float(1f, 0, FadeOutTime, alpha => { Background.material.SetFloat("_Fading", alpha); });

        sequence
            .Join(backgroundTween)
            .Join(Speaker.DOFade(0, FadeOutTime))
            .Join(Age.DOFade(0, FadeOutTime))
            .Join(Statement.DOFade(0, FadeOutTime));
    }
}