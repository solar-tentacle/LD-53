using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ChangeHealthView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _positiveValueText;
        [SerializeField] private TextMeshProUGUI _negativeValueText;
        [SerializeField] private GameObject _positiveView;
        [SerializeField] private GameObject _negativeView;
        [SerializeField] public CanvasGroup CanvasGroup;

        public void SetHealth(int value)
        {
            _positiveView.SetActive(value >= 0);
            _negativeView.SetActive(value < 0);
            _positiveValueText.text = $"+{value}";
            _negativeValueText.text = value.ToString();
        }
    }
}