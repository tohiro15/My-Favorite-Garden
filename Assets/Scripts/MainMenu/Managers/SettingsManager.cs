using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class SettingsManager : MonoBehaviour
{
    private MainMenuManager _mainMenuManager;
    public void Initialization(MainMenuManager mainMenuManager)
    {
        _mainMenuManager = mainMenuManager;
    }

    public void ChangeLanguage(string localeCode)
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        Locale target = locales.Find(l => l.Identifier.Code == localeCode);

        if (target != null)
        {
            LocalizationSettings.SelectedLocale = target;
            Debug.Log($"язык переключен на: {localeCode}");
        }
        else
        {
            Debug.LogWarning($"Locale с кодом {localeCode} не найден в Available Locales!");
        }
    }
}
