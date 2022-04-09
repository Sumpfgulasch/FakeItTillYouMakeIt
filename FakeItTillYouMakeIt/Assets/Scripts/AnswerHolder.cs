using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AnswerHolder : MonoBehaviour
{
    public event Action<Answer> OnClick;
    public Answer Answer { get; set; }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnAnswerClicked);
    }

    private void OnAnswerClicked()
    {
        OnClick?.Invoke(Answer);
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}