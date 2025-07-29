using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private int _gameSceneIndex = 1;
    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (_gameSceneIndex == currentScene.buildIndex) return;
        _startButton?.onClick.RemoveAllListeners();
        _startButton?.onClick.AddListener(StartGame);

        _exitButton?.onClick.RemoveAllListeners();
        _exitButton?.onClick?.AddListener(ExitGame);

    }

    private void StartGame()
    {
        SceneManager.LoadSceneAsync(_gameSceneIndex);
    }

    public void ExitGame()
    {
        Debug.Log("Exit pressed, quitting...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
