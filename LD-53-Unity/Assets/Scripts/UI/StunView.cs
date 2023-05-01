using TMPro;
using UnityEngine;

namespace UI
{
    public class StunView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        
        public void SetCounter(uint value)
        {
            gameObject.SetActive(value > 0);
            _valueText.text = value.ToString();
        }
    }
}