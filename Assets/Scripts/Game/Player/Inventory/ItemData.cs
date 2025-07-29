using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Scripts/Player/Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    [Tooltip("Какая 3D модель у предмета?")]
    public GameObject ItemPrefab;
    [Tooltip("Какая 3D модель у  растения, который будет инстанцироваться на грядке")]
    public GameObject PlantPrefab;
    [Space]
    [Tooltip("Какое название у предмета?")]
    public LocalizedString ItemName;
    [Tooltip("Какая иконка у предмета в инвентаре?")]
    public Sprite Icon;
    [Tooltip("Сколько выдается в первый раз?")]
    public int DefaultCount = 1;
    [Tooltip("Сколько максимум можно носить в одном слоте?")]
    public int MaxStackSize = 99;
    [Tooltip("Какая цена предмета?")]
    public int ItemPrice = 0;
    [Tooltip("Можно ли этот предмет продать?")]
    public bool CanSell = false;
    [Tooltip("Можно ли этот предмет садить на грядке?")]
    public bool IsPlantable = false;
}
