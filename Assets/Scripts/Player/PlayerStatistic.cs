using System;
using UnityEngine;

public class PlayerStatistic : MonoBehaviour
{
    public static PlayerStatistic Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        if (Input.GetKeyDown(KeyCode.X)) AddMoney(100);
        else if (Input.GetKeyDown(KeyCode.Z)) RemoveMoney(100);
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
        Money = PlayerPrefs.GetInt("MoneyKey", 0);
    }
}
