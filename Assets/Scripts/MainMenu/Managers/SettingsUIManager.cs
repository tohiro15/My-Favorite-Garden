using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SettingsUIManager : MonoBehaviour
{
    [Header("Language Settings")]
    [SerializeField] private Button _russianLanguageButton;
    [SerializeField] private Button _englishLanguageButton;
    [Space]

    private MainMenuManager _mainMenuManager;

    public void Initialization(MainMenuManager mainMenuManager)
    {
        _mainMenuManager = mainMenuManager;

        // language debug

        if (_russianLanguageButton == null) Debug.Log("Russian Language Button - not initialized!");
        if (_englishLanguageButton == null) Debug.Log("English Language Button - not initialized!");

        // language init
        _russianLanguageButton?.onClick.RemoveAllListeners();
        _russianLanguageButton?.onClick.AddListener(() => _mainMenuManager?.GetSettingsManager?.ChangeLanguage("ru"));

        _englishLanguageButton?.onClick.RemoveAllListeners();
        _englishLanguageButton?.onClick.AddListener(() => _mainMenuManager?.GetSettingsManager?.ChangeLanguage("en"));
    }

}
