using System;
using UnityEngine;

public class PlayerStatistic : MonoBehaviour
{
    public int Money { get; private set; }

    public event Action<int> OnMoneyChanged;
    private void Start()
    {
        LoadStatistic();
        OnMoneyChanged?.Invoke(Money);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) AddMoney(1);
        else if (Input.GetKeyDown(KeyCode.Z)) RemoveMoney(1);
    }
    public void AddMoney(int count)
    {
        Money += count;
        PlayerPrefs.SetInt("MoneyKey", Money);
        SaveStatistic();

        OnMoneyChanged?.Invoke(Money);
    }

    public void RemoveMoney(int count)
    {
        Money -= count;

        if(Money < 0) Money = 0;

        PlayerPrefs.SetInt("MoneyKey", Money);
        SaveStatistic();

        OnMoneyChanged?.Invoke(Money);
    }
    public void SaveStatistic()
    {
        PlayerPrefs.Save();
    }
    public void LoadStatistic()
    {
        Money = PlayerPrefs.GetInt("MoneyKey", 0);
    }
}
