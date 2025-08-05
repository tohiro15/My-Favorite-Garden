using System;
using UnityEngine;

public class PlayerStatistic : MonoBehaviour
{
    public static PlayerStatistic Instance { get; private set; }

    [Header("Start Settings")]
    [SerializeField] private int _startMoneyCount = 1;
    [Space]
    [Header("Development mode")]
    [SerializeField] private Canvas _developmentCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int Money { get; private set; }

    public event Action<int> OnMoneyChanged;
    private void Start()
    {
        LoadStatistic();
        OnMoneyChanged?.Invoke(Money);
    }
    private void Update()
    {
        if (_developmentCanvas != null && Debug.isDebugBuild || Application.isEditor) HandleDevInput();
    }
    private void HandleDevInput()
    {
        _developmentCanvas.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.X)) AddMoney(100);
        if (Input.GetKeyDown(KeyCode.Z)) RemoveMoney(100);
    }
    public void AddMoney(int amount)
    {
        ChangeMoney(Money + amount);
    }

    public void RemoveMoney(int amount)
    {
        ChangeMoney(Mathf.Max(Money - amount, 0));
    }

    private void ChangeMoney(int newAmount)
    {
        Money = newAmount;
        OnMoneyChanged?.Invoke((int)Money);
        SaveStatistic();
    }

    public void SaveStatistic()
    {
        PlayerPrefs.SetInt("MoneyKey", (int)Money);
        PlayerPrefs.Save();
    }

    public void LoadStatistic()
    {
        Money = PlayerPrefs.GetInt("MoneyKey", _startMoneyCount);
    }
}
