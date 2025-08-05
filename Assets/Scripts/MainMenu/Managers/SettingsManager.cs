using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System;

public class SettingsManager : MonoBehaviour
{
    private MainMenuManager _mainMenuManager;
    private string _currentLocaleCode;

    public string CurrentLocaleCode => _currentLocaleCode;

    public event Action<float> OnVolumeSFXChanged;
    public event Action<float> OnVolumeMusicChanged;

    public void Initialization(MainMenuManager mainMenuManager)
    {
        _mainMenuManager = mainMenuManager;

        _currentLocaleCode = LocalizationSettings.SelectedLocale.Identifier.Code;

        LoadSettings();
    }

    public void SaveSettings()
    {
        _mainMenuManager.GetMainMenuSoundManager.SaveVolume();
        PlayerPrefs.SetString("LanguageKey", _currentLocaleCode);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        _mainMenuManager.GetMainMenuSoundManager.LoadVolume();
        _currentLocaleCode = PlayerPrefs.GetString("LanguageKey", "en");

        ChangeLanguage(_currentLocaleCode);
        ChangeSFXVolume(_mainMenuManager.GetMainMenuSoundManager.GetSFXVolume);
        ChangeMusicVolume(_mainMenuManager.GetMainMenuSoundManager.GetMusicVolume);
    }

    public void ChangeLanguage(string localeCode)
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        Locale target = locales.Find(l => l.Identifier.Code == localeCode);

        if (target != null)
        {
            LocalizationSettings.SelectedLocale = target;
            _currentLocaleCode = localeCode;
            Debug.Log($"язык переключен на: {localeCode}");
        }
        else
        {
            Debug.LogWarning($"Locale с кодом {localeCode} не найден в Available Locales!");
        }

        _mainMenuManager?.GetMainMenuUIManager?.GetSettingsUIManager?.ChangeCurrentLanguage(_currentLocaleCode);
    }

    public void ChangeSFXVolume(float volume)
    {
        _mainMenuManager.GetMainMenuSoundManager.ChangeSFXVolume(volume);
        OnVolumeSFXChanged?.Invoke(volume);
    }
    public void ChangeMusicVolume(float volume)
    {
        _mainMenuManager.GetMainMenuSoundManager.ChangeMusicVolume(volume);
        OnVolumeMusicChanged?.Invoke(volume);
    }
}
