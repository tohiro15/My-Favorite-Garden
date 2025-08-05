using UnityEngine;
using StarterAssets;

public class Plant : MonoBehaviour
{
    [Header("Planting Settings")]
    [SerializeField] private FXPool _fxPool;
    [SerializeField] private GameObject _plantZone;
    [SerializeField] private float _interactionDistance = 3f;

    private ThirdPersonController _controller;
    private Transform _player;
    private ItemData _selectedData;
    private Collider _plantZoneCollider;
    private bool _isPlayerNear;

    private void Start()
    {
        _plantZoneCollider = _plantZone.GetComponent<Collider>();
        _fxPool = GetComponent<FXPool>();

        _controller = FindAnyObjectByType<ThirdPersonController>();
        if (_controller == null)
        {
            Debug.LogError("ThirdPersonController не найден в сцене!");
            enabled = false;
            return;
        }
        _player = _controller.transform;
    }

    private void Update()
    {
        CheckPlayerDistance();

        if (_isPlayerNear && Input.GetMouseButtonDown(0))
        {
            TryPlantAtCursor();
        }
    }


    private void CheckPlayerDistance()
    {
        float sqrDist = (_player.position - transform.position).sqrMagnitude;
        _isPlayerNear = sqrDist <= _interactionDistance * _interactionDistance;


        if (sqrDist < _interactionDistance * _interactionDistance)
        {
            _isPlayerNear = true;
        }
        else
        {
            _isPlayerNear = false;
        }
    }


    private void TryPlantAtCursor()
    {
        _selectedData = HandController.Instance.CurrentItemData;

        if (_selectedData == null || !_selectedData.IsPlantable)
        {
            Debug.Log("Нечего садить или предмет не посадочный");
            return;
        }

        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit) && hit.collider == _plantZoneCollider)
        {
            var plant = Instantiate(_selectedData.PlantPrefab, hit.point, Quaternion.identity, transform);

            _fxPool?.GetFromPool(hit.point);
            SoundManager.Instance.PlayDigSound();

            InventoryUI.Instance.InventorySlots[InventoryUI.Instance.SelectedSlotIndex].Remove(_selectedData, 1);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _interactionDistance);
    }
}
