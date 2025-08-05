using System;
using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance { get; private set; }

    [SerializeField] private SeasonTypes _currentSeason = SeasonTypes.None;

    public event Action<SeasonTypes> OnSeasonChanged;

    private const string SeasonKey = "CurrentSeason";
    public SeasonTypes CurrentSeason => _currentSeason;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSeason();
    }


    public void LoadSeason()
    {
        int seasonValue = PlayerPrefs.GetInt(SeasonKey, (int)SeasonTypes.None);
        _currentSeason = (SeasonTypes)seasonValue;
        Debug.Log($"Season loaded: {_currentSeason}");
        OnSeasonChanged?.Invoke(_currentSeason);
    }

    public void SaveSeason()
    {
        PlayerPrefs.SetInt(SeasonKey, (int)_currentSeason);
        PlayerPrefs.Save();
        Debug.Log($"Season saved: {_currentSeason}");
    }

    public void SetSeason(SeasonTypes newSeason)
    {
        if (_currentSeason == newSeason) return;
        _currentSeason = newSeason;
        Debug.Log($"Season changed to: {_currentSeason}");
        SaveSeason();
        OnSeasonChanged?.Invoke(_currentSeason);
    }

    public string GetCurrentSeasonLocalizationKey()
    {
        switch (_currentSeason)
        {
            case SeasonTypes.StrawberrySeason:
                return "season_strawberry";
            case SeasonTypes.CarrotSeason:
                return "season_carror";
            case SeasonTypes.TomatoSeason:
                return "season_tomato";
            case SeasonTypes.PotatoSeason:
                return "season_potato";
            case SeasonTypes.CabbageSeason:
                return "season_cabbage";
            default:
                return "season_none";
        }
    }
}