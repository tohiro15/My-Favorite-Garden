using UnityEngine;
using Unity.Cinemachine;
using StarterAssets;

public class Plant : MonoBehaviour
{
    [Header("Cameras")]
    [Tooltip("Новая CinemachineCamera над грядкой")]
    [SerializeField] private CinemachineCamera _plantCamera;

    private CinemachineCamera _playerCamera;

    [Header("Planting Settings")]
    [SerializeField] private FXPool _fxPool;
    [SerializeField] private GameObject _plantZone;
    [SerializeField] private float _interactionDistance = 3f;

    private ThirdPersonController _controller;
    private ItemData _selectedData;
    private Collider _plantZoneCollider;
    private Transform _player;
    private bool _isPlayerNear;
    private bool _inPlantMode;

    private const int PlayerCamPriority = 10;
    private const int PlantCamPriority = 20;
    private const int PlantCamIdlePriority = 0;

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

        _playerCamera = _controller.MainCamera;
        if (_playerCamera == null)
        {
            Debug.LogError("Не найдена CinemachineCamera на игроке!");
            enabled = false;
            return;
        }

        _playerCamera.Priority = PlayerCamPriority;
        _plantCamera.Priority = PlantCamIdlePriority;
    }

    private void Update()
    {
        CheckPlayerDistance();

        if (Input.GetKeyDown(KeyCode.E) && _isPlayerNear)
        {
            if (!_inPlantMode)
                EnterPlantMode();
            else
                ExitPlantMode();
        }

        if (_inPlantMode && Input.GetMouseButtonDown(0))
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

            UIManager.Instance.EnableInteractionButton(() => EnterPlantMode(), this);
        }
        else
        {
            _isPlayerNear = false;

            UIManager.Instance.DisableInteractionButton(this);
        }
    }

    private void EnterPlantMode()
    {
        _inPlantMode = true;
        _controller.ToggleThirdPersonController(true);

        UIManager.Instance.OpenPlantPanel();
        UIManager.Instance.EnableExitButton(ExitPlantMode, this);

        _plantCamera.Priority = PlantCamPriority;
        _playerCamera.Priority = PlayerCamPriority;
    }


    public void ExitPlantMode()
    {
        if (!_inPlantMode) return;

        _inPlantMode = false;

        UIManager.Instance.DisableExitButton(this);

        _controller.ToggleThirdPersonController(_inPlantMode);

        _plantCamera.Priority = PlantCamIdlePriority;
        _playerCamera.Priority = PlayerCamPriority;
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
