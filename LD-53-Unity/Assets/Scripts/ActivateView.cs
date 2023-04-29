using UnityEngine;

public class ActivateView : MonoBehaviour
{
    [SerializeField] private float _hideTime = 1f;
        
    private bool _isShow;
    public bool IsShow => _isShow;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (!_isShow)
        {
            Hide();
        }
            
        OnInit();
    }

    protected virtual void OnInit()
    {
            
    }

    public void Show()
    {
        if (_isShow)
        {
            return;
        }
            
        _isShow = true;
        gameObject.SetActive(true);
            
        OnShow();
    }

    protected virtual void OnShow()
    {
    }
        
    public void Hide()
    {
        _isShow = false;
        gameObject.SetActive(false);

        OnHide();
    }
        
    protected virtual void OnHide()
    {
            
    }

    public void ShowAndHideAfterTime()
    {
        Show();
        Invoke(nameof(Hide), _hideTime);
    }
}