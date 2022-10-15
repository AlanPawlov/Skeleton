using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class PauseWindow : BaseWindow
{
    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _restartButton;
    private ECSSharedData _sharedData;

    [Inject]
    public void Construct(ECSSharedData sharedData)
    {
        _sharedData = sharedData;
    }

    private void RestartButtonClick()
    {
        var targetScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(targetScene);
        _sharedData.IsPause = false;
    }

    public override void Show()
    {
        base.Show();
        Cursor.lockState = CursorLockMode.None;
        _continueButton.onClick.AddListener(Hide);
        _restartButton.onClick.AddListener(RestartButtonClick);
    }

    public override void Hide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _continueButton.onClick.RemoveListener(Hide);
        _restartButton.onClick.RemoveListener(RestartButtonClick);
        _sharedData.IsPause = false;
        base.Hide();
    }
}