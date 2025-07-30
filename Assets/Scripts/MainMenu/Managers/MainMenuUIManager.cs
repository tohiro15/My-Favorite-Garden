using DG.Tweening;
using GamePush;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] private Canvas _mainMenuCanvas;
    [Space]

    [Header("MainMenu")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    [Space]

    [Header("Settings Panel")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _closeButton;
    [SerializeField] private float _settingsOpenSpeed = 1f;
    [Space]

    [Header("Updates panel")]
    [SerializeField] private RectTransform _updatesPanelRT;
    [SerializeField] private float _updatesOpenSpeed = 0.3f;
    [SerializeField] private float _offset = 385;
    [Space]

    [Header("Managers")]

    [SerializeField] private SettingsUIManager _settingsUIManager;

    private Tween _settingsTween;
    private Tween _updatesTween;

    private Vector2 _updateHidePos;

    private Vector2 _centerScreenPos;
    private Vector2 _settingsStartPos;
    public void Initialization(MainMenuManager mainMenuManager)
    {
        // canvas debug
        if (_mainMenuCanvas == null) Debug.Log("Main Menu Canvas - not initialized!");

        // mainMenu debug
        if (_playButton == null) Debug.Log("Start Button - not initialized!");
        if (_settingsButton == null) Debug.Log("Settings Button - not initialized!");
        if (_exitButton == null) Debug.Log("Exit Button - not initialized!");

        // settings debug
        if (_settingsPanel == null) Debug.Log("Settings Panel - not initialized!");
        if (_closeButton == null) Debug.Log("Exit Settings Button - not initialized!");

        // updates debug
        if (_updatesPanelRT == null) Debug.Log("Updates Panel - not initialized!");

        // managers debug

        if (_settingsUIManager == null) Debug.LogError("Settings UI Manager - not initialized!");

        // mainMenu init
        _playButton?.onClick.RemoveAllListeners();
        _playButton?.onClick.AddListener(mainMenuManager.StartGame);

        _settingsButton?.onClick.RemoveAllListeners();
        _settingsButton?.onClick?.AddListener(ShowSettingsPanel);

        _exitButton?.onClick.RemoveAllListeners();
        _exitButton?.onClick?.AddListener(mainMenuManager.ExitGame);

        // settings init
        _settingsPanel?.gameObject.SetActive(false);

        _closeButton?.onClick.RemoveAllListeners();
        _closeButton?.onClick.AddListener(HideSettingsPanel);

        _centerScreenPos = _mainMenuCanvas.GetComponent<RectTransform>().anchoredPosition;
        _settingsStartPos = new Vector2(_centerScreenPos.x, -Screen.height);

        _settingsPanel.transform.position = _settingsStartPos;

        // updates init
        _updateHidePos = _updatesPanelRT.anchoredPosition;

        // managers init

        _settingsUIManager?.Initialization(mainMenuManager);
    }

    public void ShowSettingsPanel()
    {
        if (_settingsPanel == null) return;
        _settingsPanel?.gameObject.SetActive(true);

        _settingsTween.Kill();
        _settingsTween = _settingsPanel?.transform
            .DOMoveY(_centerScreenPos.y, _settingsOpenSpeed)
            .SetEase(Ease.OutBack);
    }

    public void HideSettingsPanel()
    {
        if (_settingsPanel == null) return;

        _settingsTween.Kill();
        _settingsTween = _settingsPanel?.transform
            .DOMoveY(_settingsStartPos.y, _settingsOpenSpeed)
            .SetEase(Ease.OutBack)
            .OnComplete(() => _settingsPanel?.gameObject.SetActive(false));
    }
    public void ShowUpdatesPanel()
    {
        _updatesTween?.Kill();
        _updatesTween = _updatesPanelRT.DOAnchorPosX(-_offset, _updatesOpenSpeed)
            .SetEase(Ease.OutCubic);
    }

    public void HideUpdatesPanel()
    {
        _updatesTween?.Kill();
        _updatesTween = _updatesPanelRT.DOAnchorPosX(_updateHidePos.x, _updatesOpenSpeed)
            .SetEase(Ease.InCubic);
    }

}
