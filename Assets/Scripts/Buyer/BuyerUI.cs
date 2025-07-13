using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BuyerUI : MonoBehaviour
{
    [Header("Item List Settings")]
    [Space]
    [SerializeField] private GameObject _buyerItemPrefab;
    [Space]
    [SerializeField] private InventoryUI _playerInventory;
    [SerializeField] private BuyerLine[] _lines;

    [Header("Buyer Settings")]
    [Space]

    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _sellAllButton;
    private void Start()
    {
        var filledSlots = _playerInventory.InventorySlots.Where(s => !s.IsEmpty).ToArray();

        int totalItems = filledSlots.Length;
        int neededLines = Mathf.CeilToInt(totalItems / 3f);

        for (int lineIndex = 0; lineIndex < _lines.Length; lineIndex++)
        {
            if (lineIndex < neededLines)
            {
                _lines[lineIndex].gameObject.SetActive(true);

                int startIdx = lineIndex * 3;
                int countInThisLine = Mathf.Min(3, totalItems - startIdx);

                var slice = new InventorySlot[countInThisLine];
                System.Array.Copy(filledSlots, startIdx, slice, 0, countInThisLine);

                _lines[lineIndex].Initialize(slice, _buyerItemPrefab);
            }
            else
            {
                _lines[lineIndex].gameObject.SetActive(false);
            }
        }

        _sellAllButton?.onClick.RemoveAllListeners();
        _sellButton?.onClick.RemoveAllListeners();
    }

}
