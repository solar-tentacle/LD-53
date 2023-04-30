﻿using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UICardsHand : ActivateView
{
    [SerializeField] private CardView _cardViewPrefab;
    [SerializeField] private GameObject _cardShirt;
    [SerializeField] private Transform _cardViewParent;
    [SerializeField] private Transform _selectedCardParent;
    [SerializeField] private GameObject _blocker;
    [SerializeField] private Transform _drawStartPoint;

    public CardView CreateCard(CardConfig cardConfig)
    {
        var view = Instantiate(_cardViewPrefab, _cardViewParent);
        view.SetContent(cardConfig);

        return view;
    }

    public void ClearHand()
    {
        _cardViewParent.Clear();
    }

    public IEnumerator SelectCard(CardView view)
    {
        view.transform.SetParent(_selectedCardParent);
        yield return null;
        view.enabled = false;
        yield return null;
        yield return view.transform.DOMove(_selectedCardParent.position, 0.3f).WaitForCompletion();
    }

    public IEnumerator HideCard(CardView view)
    {
        yield return view.transform.DOScale(Vector3.zero, 0.3f);
    }

    public void EnableBlocker() => _blocker.SetActive(true);

    public void DisableBlocker() => _blocker.SetActive(false);

    public IEnumerator DrawAnimation(CardView view)
    {
        Transform shirtTransform = Instantiate(_cardShirt, transform).transform;
        shirtTransform.position = _drawStartPoint.position;
        view.CanvasGroup.alpha = 0;
        yield return shirtTransform.DOMove(view.transform.position, 1).WaitForCompletion();
        view.CanvasGroup.DOFade(1, 0.3f);
        yield return shirtTransform.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
        Destroy(shirtTransform.gameObject);
    }
}