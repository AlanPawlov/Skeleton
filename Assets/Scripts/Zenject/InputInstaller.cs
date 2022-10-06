using UnityEngine;

public class InputInstaller : MonoBehaviour
{
    [SerializeField]
    private GameObject _pcInput;
    [SerializeField]
    private GameObject _mobileInput;
    private void Awake()
    {
#if UNITY_ANDROID
        _pcInput.gameObject.SetActive(false);
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        _mobileInput.gameObject.SetActive(false);
#endif
    }
}
