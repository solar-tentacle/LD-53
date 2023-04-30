using TMPro;
using UnityEngine;

namespace UI
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        
        public void SetHealth(uint value)
        {
            _valueText.text = value.ToString();
        }
    }
}