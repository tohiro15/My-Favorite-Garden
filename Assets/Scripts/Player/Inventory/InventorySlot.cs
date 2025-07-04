using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Item _item;
    public Item Item => _item;
    public bool IsFree { get; private set; }

}
