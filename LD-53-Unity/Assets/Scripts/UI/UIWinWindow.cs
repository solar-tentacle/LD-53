﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class  UIWinWindow : ActivateView
{
    [SerializeField] private Button _nextLevelButton;
    private bool _clicked;

    private void Start()
    {
        _nextLevelButton.onClick.AddListener(OnNextButtonClicked);
    }

    protected override void OnInit()
    {
        base.OnInit();
        _clicked = false;
    }

    private void OnNextButtonClicked()
    {
        _clicked = true;
    }

    public IEnumerator WaitForClose()
    {
        while (!_clicked)
        {
            yield return null;
        }
    }
}