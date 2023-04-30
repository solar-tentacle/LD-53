using TMPro;
using UnityEngine;

public class UIDeckIndicator : ActivateView
{
    [SerializeField] private TMP_Text _moveCardCountText;
    [SerializeField] private TMP_Text _actionCardCountText;

    public void UpdateView(int moveCardCount, int actionCardCount)
    {
        _moveCardCountText.text = moveCardCount.ToString();
        _actionCardCountText.text = actionCardCount.ToString();
    }
}