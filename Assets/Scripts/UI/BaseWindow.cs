using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    private bool _isActive;

    public bool IsActive => _isActive;

    public virtual void Show()
    {
        _isActive = true;
        gameObject.SetActive(_isActive);
    }

    public virtual void Hide()
    {
        _isActive = false;
        gameObject.SetActive(_isActive);
    }
}
