using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private Transform _handPosition;
    [SerializeField] private GameObject _prefabObject;

    private void Start()
    {
        Instantiate(_prefabObject, _handPosition);
    }

    private void Update()
    {
        _prefabObject.transform.position = Vector3.zero;
    }
}
