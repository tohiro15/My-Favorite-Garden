using System;
using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance { get; private set; }

    [SerializeField] private SeasonTypes _currentSeason = SeasonTypes.None;
    public event Action<SeasonTypes> OnSeasonChanged;

    private DateTime _lastCheckedDate;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateSeasonForToday();
    }

    private void Update()
    {
        if (DateTime.UtcNow.Date != _lastCheckedDate)
        {
            UpdateSeasonForToday();
        }
    }

    private void UpdateSeasonForToday()
    {
        _lastCheckedDate = DateTime.UtcNow.Date;
        _currentSeason = GetSeasonForDate(_lastCheckedDate);
        OnSeasonChanged?.Invoke(_currentSeason);
        Debug.Log($"Season for {_lastCheckedDate.ToShortDateString()}: {_currentSeason}");
    }

    private SeasonTypes GetSeasonForDate(DateTime date)
    {
        int seed = date.Year * 1000 + date.DayOfYear;
        System.Random rng = new System.Random(seed);

        Array values = Enum.GetValues(typeof(SeasonTypes));
        int index = rng.Next(values.Length);

        return (SeasonTypes)values.GetValue(index);
    }

    public string GetCurrentSeasonLocalizationKey()
    {
        switch (_currentSeason)
        {
            case SeasonTypes.StrawberrySeason:
                return "season_strawberry";
            case SeasonTypes.CarrotSeason:
                return "season_carrot";
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

    public SeasonTypes CurrentSeason => _currentSeason;
}
