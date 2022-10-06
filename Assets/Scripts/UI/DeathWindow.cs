using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathWindow : BaseWindow
{
    [SerializeField]
    private Button _restartButton;

    public override void Show()
    {
        base.Show();
        _restartButton.onClick.AddListener(RestartButtonClick);
    }

    private void RestartButtonClick()
    {
        var targetScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(targetScene);
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(RestartButtonClick);
    }
}
