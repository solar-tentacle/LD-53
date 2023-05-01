using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEncounterWindow : ActivateView
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private GameObject _buttonYes;
    [SerializeField] private GameObject _buttonNo;
    [SerializeField] private GameObject _buttonEnd;
    [SerializeField] private TextMeshProUGUI _buttonYesText;
    [SerializeField] private TextMeshProUGUI _buttonNoText;
    [SerializeField] private TextMeshProUGUI _buttonEndText;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _image;
    private EncounterData _data;
    private AnswerEncounterData _currentAnswerData;
    private CoroutineService _coroutineService;

    public bool Ended { get; private set; }
    public AnswerEncounterData CurrentAnswerData => _currentAnswerData;

    private void Start()
    {
        _coroutineService = Services.Get<CoroutineService>();
        _buttonEnd.SetActive(false);
        _buttonEndText.text = "OK";
    }

    protected override void OnShow()
    {
        base.OnShow();
        _currentAnswerData = null;
    }

    public void ShowEncounterWindow()
    {
        Show();
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1.0f, 0.5f);
        _buttonNo.SetActive(true);
        _buttonYes.SetActive(true);
        Ended = false;
    }

    public void ButtonYesClick()
    {
        _currentAnswerData = _data.AnswerData1;
        _buttonEnd.SetActive(true);
        _buttonNo.SetActive(false);
        _buttonYes.SetActive(false);
        ChangeEncounterWindowTextTo(_currentAnswerData.ResultText);
    }

    public void ButtonNoClick()
    {
        _currentAnswerData = _data.AnswerData2;
        _buttonEnd.SetActive(true);
        _buttonNo.SetActive(false);
        _buttonYes.SetActive(false);
        ChangeEncounterWindowTextTo(_currentAnswerData.ResultText);
    }

    public void ButtonEndClick()
    {
        Ended = true;
        _buttonEnd.SetActive(false);
        _buttonNo.SetActive(false);
        _buttonYes.SetActive(false);
        Hide();
    }

    public void SetContent(EncounterData data)
    {
        _data = data;
        _image.sprite = data.Icon;
        ChangeEncounterWindowTextTo(data.Question);
        ChangeYesButtonTextTo(data.AnswerData1.AnswerText);
        ChangeNoButtonTextTo(data.AnswerData2.AnswerText);
    }

    public void ChangeEncounterWindowTextTo(string text)
    {
        _messageText.text = text;
    }

    public void ChangeYesButtonTextTo(string text)
    {
        _buttonYesText.text = text;
    }

    public void ChangeNoButtonTextTo(string text)
    {
        _buttonNoText.text = text;
    }
}