using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController Instance { get; private set; }

    [SerializeField] private Transform _handPosition;

    private GameObject _currentView;
    public ItemData CurrentItemData { get; private set; }

    private void Awake() => Instance = this;

    public void Hold(GameObject prefab, ItemData data)
    {
        Clear();
        if (prefab != null)
        {
            _currentView = Instantiate(prefab, _handPosition);
        }
        CurrentItemData = data;
        Debug.Log($"Вы взяли предмет в руку");
    }

    public void Clear()
    {
        if (_currentView != null) Destroy(_currentView);
        CurrentItemData = null;
    }
}
