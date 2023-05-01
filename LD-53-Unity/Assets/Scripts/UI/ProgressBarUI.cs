using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] protected Image FillImage;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }

        protected virtual void OnHide()
        {
            
        }

        public void SetValue(float current, float max)
        {
            var value = Mathf.Clamp01(current / max);
            
            UpdateFillImage(value);

            OnValueUpdated(current, max);
        }

        protected virtual void UpdateFillImage(float value)
        {
            if (FillImage == null) return;
            FillImage.fillAmount = value;
        }

        public void SetFillColor(Color color)
        {
            if (FillImage == null) return;
            FillImage.color = color;
        }

        public virtual void OnValueUpdated(float current, float max)
        {

        }
    }
}


