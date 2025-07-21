using StarterAssets;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private FXPool _fxPool;

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _plantZone;
    [SerializeField] private float _interactionDistance = 3f;

    private Collider _plantZoneCollider;
    private Camera _mainCamera;
    private bool _isPlayerNear;

    private void Start()
    {
        _plantZoneCollider = _plantZone.GetComponent<Collider>();
        _mainCamera = Camera.main;
        _player = FindAnyObjectByType<ThirdPersonController>().transform;
        _fxPool = GetComponent<FXPool>();
    }

    private void Update()
    {
        CheckDistance();
        HandleInteraction();
    }

    private void CheckDistance()
    {
        Vector3 offset = transform.position - _player.position;
        float sqrDistance = offset.sqrMagnitude;

        _isPlayerNear = sqrDistance < _interactionDistance * _interactionDistance;
    }

    private bool TryGetInteractionPosition(out Vector2 screenPos)
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            screenPos = Input.GetTouch(0).position;
            return true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            screenPos = Input.mousePosition;
            return true;
        }

        screenPos = default;
        return false;
    }

    private void HandleInteraction()
    {
        if (!TryGetInteractionPosition(out Vector2 pos) || !_isPlayerNear)
            return;

        Ray ray = _mainCamera.ScreenPointToRay(pos);
        CheckRay(ray);
    }

    private void CheckRay(Ray ray)
    {
        if (!_isPlayerNear) return;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider == _plantZoneCollider)
            {
                Instantiate(_itemPrefab, hit.point, Quaternion.identity, transform);

                _fxPool?.GetFromPool(hit.point);
                SoundManager.Instance.PlayDigSound();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _interactionDistance);
    }
}
