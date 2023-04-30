using System.Collections;
using System.Collections.Generic;
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
    private EncounterData _data;
    private AnswerEncounterData _currentAnswerData;
    private CoroutineService _coroutineService;
    
    private void Start()
    {
        _coroutineService = Services.Get<CoroutineService>();
        _buttonEnd.SetActive(false);
        _buttonEndText.text = "OK";
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
        if (_currentAnswerData.ResultAction != null)
        {
            _coroutineService.ExecuteAction(_currentAnswerData.ResultAction);
        }
        _buttonEnd.SetActive(false);
        _buttonNo.SetActive(false);
        _buttonYes.SetActive(false);
        Hide();
    }

    public void SetContent(EncounterData data)
    {
        _data = data;
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