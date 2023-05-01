using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class  UIAllLevelsCompletedWindow : ActivateView
{
    [SerializeField] private Button _nextLevelButton;
    private bool _clicked;

    private void Start()
    {
        _nextLevelButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void OnNextButtonClicked()
    {
        _clicked = true;
    }

    protected override void OnShow()
    {
        _clicked = false;
        base.OnShow();
    }

    public IEnumerator WaitForClose()
    {
        while (!_clicked)
        {
            yield return null;
        }
    }
}