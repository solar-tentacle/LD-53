using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UICardsHand : ActivateView
{
    [SerializeField] private CardView _cardViewPrefab;
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
        view.enabled = false;
        yield return view.Container.DOMove(_selectedCardParent.position, 0.3f).WaitForCompletion();
    }
    
    public IEnumerator MoveCardToHand(CardView view)
    {
        view.enabled = false;
        yield return view.Container.DOLocalMove(view.ContainerStartLocalPos, 0.3f).WaitForCompletion();
        view.enabled = true;
    }

    public IEnumerator HideCard(CardView view)
    {
        yield return view.transform.DOScale(Vector3.zero, 0.3f);
        Destroy(view.gameObject);
    }

    public void EnableBlocker() => _blocker.SetActive(true);

    public void DisableBlocker() => _blocker.SetActive(false);

    public IEnumerator DrawAnimation(CardView view)
    {
        view.Container.position = _drawStartPoint.position;
        yield return view.Container.DOLocalMove(view.ContainerStartLocalPos, 1).WaitForCompletion();
    }
}