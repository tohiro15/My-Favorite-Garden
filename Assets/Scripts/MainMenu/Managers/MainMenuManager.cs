using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Managers")]

    [SerializeField] private MainMenuUIManager _mainMenuUIManager;
    [SerializeField] private MainMenuSoundManager _mainMenuSoundManager;
    [SerializeField] private SettingsManager _settingsManager;
    [Space]

    [Header("Start Game Settings")]
    [SerializeField] private int _gameSceneIndex = 1;
    public MainMenuUIManager GetMainMenuUIManager => _mainMenuUIManager;
    public MainMenuSoundManager GetMainMenuSoundManager => _mainMenuSoundManager;
    public SettingsManager GetSettingsManager => _settingsManager;
    private void Start()
    {
        if (_mainMenuUIManager != null) _mainMenuUIManager?.Initialization(this);
        else Debug.LogError("MainMenu UI Manager - not initialized!");

        if (_settingsManager != null) _settingsManager?.Initialization(this);
        else Debug.LogError("Settings Manager - not initialized!");
    }

    public void StartGame()
    {
        if (GetCurrentScene().buildIndex != _gameSceneIndex)
        {
            SceneManager.LoadSceneAsync(_gameSceneIndex);
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exit pressed, quitting...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private Scene GetCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        return currentScene;
    }
}
