using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController Instance { get; private set; }

    private GameObject _currentView;
    public ItemData CurrentItemData { get; private set; }

    private void Awake() => Instance = this;

    public void Hold(GameObject prefab, ItemData data)
    {
        Clear();
        if (prefab != null)
        {
            _currentView = Instantiate(prefab, transform);
        }
        CurrentItemData = data;
        Debug.Log($"У вас в руках {data.ItemName}");
    }

    public void Clear()
    {
        if (_currentView != null) Destroy(_currentView);
        CurrentItemData = null;
    }
}
