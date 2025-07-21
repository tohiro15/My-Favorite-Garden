using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController Instance { get; private set; }

    [SerializeField] private Transform _handPosition;
    private GameObject _prefabObject;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void Hold(GameObject itemPrefab)
    {
        if (itemPrefab == null || _handPosition == null) return;
        Clear();

        _prefabObject = Instantiate(itemPrefab, _handPosition);
        var holdable = _prefabObject.GetComponent<Holdable>();

        if (holdable != null)
        {
            _prefabObject.transform.localPosition = holdable.positionOffset;
            _prefabObject.transform.localEulerAngles = holdable.rotationOffset;
        }
        else
        {
            _prefabObject.transform.localPosition = Vector3.zero;
            _prefabObject.transform.localRotation = Quaternion.identity;
        }
    }

    public void Clear()
    {
        if (_handPosition == null) return;

        if (_prefabObject != null)
            Destroy(_prefabObject);
    }
}
