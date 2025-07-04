using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/Player/Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public int DefaultCount = 1;
    public int MaxStackSize = 99;
    public float ItemPrice = 0;
}
