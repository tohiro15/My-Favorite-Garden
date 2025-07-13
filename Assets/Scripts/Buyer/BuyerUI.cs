using UnityEngine;
using System.Linq;

public class BuyerUI : MonoBehaviour
{
    [SerializeField] private InventoryUI _playerInventory;
    [SerializeField] private GameObject _buyerItemPrefab;
    [SerializeField] private BuyerLine[] _lines;

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
    }
}
