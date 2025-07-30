using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    [Header("Language Settings")]
    [SerializeField] private Button _russianLanguageButton;
    [SerializeField] private Button _englishLanguageButton;
    [Space]

    [Header("Volume Settings")]
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;

    private MainMenuManager _mainMenuManager;

    public void Initialization(MainMenuManager mainMenuManager)
    {
        _mainMenuManager = mainMenuManager;

        // language debug

        if (_russianLanguageButton == null) Debug.Log("Russian Language Button - not initialized!");
        if (_englishLanguageButton == null) Debug.Log("English Language Button - not initialized!");

        // volume debug

        if (_sfxSlider == null) Debug.Log("SFX Slider - not initialized!");
        if (_musicSlider == null) Debug.Log("Music Slider - not initialized!");

        // language init
        _russianLanguageButton?.onClick.RemoveAllListeners();
        _russianLanguageButton?.onClick.AddListener(() => _mainMenuManager?.GetSettingsManager?.ChangeLanguage("ru"));

        _englishLanguageButton?.onClick.RemoveAllListeners();
        _englishLanguageButton?.onClick.AddListener(() => _mainMenuManager?.GetSettingsManager?.ChangeLanguage("en"));

        // volume init

        if (_sfxSlider != null) _sfxSlider.value = _mainMenuManager.GetMainMenuSoundManager.GetSFXVolume;
        if (_musicSlider != null) _musicSlider.value = _mainMenuManager.GetMainMenuSoundManager.GetMusicVolume;

        _sfxSlider.value = _mainMenuManager.GetMainMenuSoundManager.GetSFXVolume;
        _musicSlider.value = _mainMenuManager.GetMainMenuSoundManager.GetMusicVolume;

        _sfxSlider?.onValueChanged.RemoveAllListeners();
        _sfxSlider?.onValueChanged.AddListener(v => _mainMenuManager.GetSettingsManager.ChangeSFXVolume(v));

        _musicSlider?.onValueChanged.RemoveAllListeners();
        _musicSlider?.onValueChanged.AddListener(v => _mainMenuManager.GetSettingsManager.ChangeMusicVolume(v));

        if(_sfxSlider != null) _mainMenuManager.GetSettingsManager.OnVolumeSFXChanged += v => _sfxSlider.value = v;
        if(_musicSlider != null) _mainMenuManager.GetSettingsManager.OnVolumeMusicChanged += v => _musicSlider.value = v;
    }

}
