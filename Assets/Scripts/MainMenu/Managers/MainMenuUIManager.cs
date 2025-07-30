using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("MainMenu")] [Space]
    [Header("Buttons")]
    [SerializeField] private Button _playGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitGameButton;
    [Space]

    [Header("Settings")] [Space]
    [Header("Panels")]
    [SerializeField] private GameObject _settingsPanel;
    [Header("Buttons")]
    [SerializeField] private Button _exitSettingsButton;

    [Header("Updates")] [Space]
    [Header("Panels")]
    [SerializeField] private GameObject _updatesPanel;
    [Header("Animation Settings")]
    [SerializeField] private float _animationSpeed;
    [SerializeField] private float _offset;

    private Tween _updatesTween;

    private Vector2 _defaultPosition;
    public void Initialization(MainMenuManager mainMenuManager)
    {
        // mainMenu debug
        if (_playGameButton == null) Debug.Log("Start Button - not initialized!");
        if (_settingsButton == null) Debug.Log("Settings Button - not initialized!");
        if (_exitGameButton == null) Debug.Log("Exit Button - not initialized!");

        // settings debug
        if (_settingsPanel == null) Debug.Log("Settings Panel - not initialized!");
        if (_exitSettingsButton == null) Debug.Log("Exit Settings Button - not initialized!");

        // updates debug

        if (_updatesPanel == null) Debug.Log("Updates Panel - not initialized!");

        // mainMenu init
        _playGameButton?.onClick.RemoveAllListeners();
        _playGameButton?.onClick.AddListener(mainMenuManager.StartGame);

        _settingsButton?.onClick.RemoveAllListeners();
        _settingsButton?.onClick?.AddListener(() => ToggleSettings(true));

        _exitGameButton?.onClick.RemoveAllListeners();
        _exitGameButton?.onClick?.AddListener(mainMenuManager.ExitGame);

        // settings init
        _settingsPanel?.gameObject.SetActive(false);

        _exitSettingsButton?.onClick.RemoveAllListeners();
        _exitSettingsButton?.onClick.AddListener(() => ToggleSettings(false));

        // updates init

        _defaultPosition = _updatesPanel.transform.position;
    }

    public void ToggleSettings(bool toggle)
    {
        if (_settingsPanel == null) return;

        _settingsPanel?.gameObject.SetActive(toggle);
    }

    public void ShowUpdatesPanel()
    {
        if(_updatesTween != null) _updatesTween.Kill();

        _updatesTween = _updatesPanel.transform
            .DOMoveX(_defaultPosition.x - _offset, _animationSpeed)
            .SetEase(Ease.Flash);
    }

    public void HideUpdatesPanel()
    {
        if (_updatesTween != null) _updatesTween.Kill();
        _updatesTween = _updatesPanel.transform
            .DOMoveX(_defaultPosition.x, _animationSpeed)
            .SetEase(Ease.Flash);
    }
}
