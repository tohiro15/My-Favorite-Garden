using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Scripts/Player/Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public LocalizedString ItemName;
    public Sprite Icon;
    public int DefaultCount = 1;
    public int MaxStackSize = 99;
    public int ItemPrice = 0;
    public bool CanSell = false;
}
