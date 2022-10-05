using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    private int _sceneIndex;
    [SerializeField]
    private Button _playButton;

    void Start()
    {
        _playButton.onClick.AddListener(() => LoadScene(_sceneIndex));
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
    }

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
    }
}
