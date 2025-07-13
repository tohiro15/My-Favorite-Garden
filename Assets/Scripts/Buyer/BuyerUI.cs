using UnityEngine;

public class BuyerUI : MonoBehaviour
{
    [SerializeField] private InventoryUI _playerInventory;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private BuyerLine[] _lines;

    private void Start()
    {
        for (int i = 0; i > _lines.Length; i++)
        {
            _lines[i].Initialize(_playerInventory, _itemPrefab);
        }
    }
}
