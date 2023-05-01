using TMPro;
using UnityEngine;

namespace UI
{
    public class UIPanel : ActivateView
    {
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private string _textPrefix;

        public void SetText(string text)
        {
            _text.text = _textPrefix + text;
        }
        
        public void SetTextColor(Color color)
        {
            _text.color = color;
        }
    }
}