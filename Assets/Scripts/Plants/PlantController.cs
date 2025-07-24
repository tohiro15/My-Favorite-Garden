using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

[RequireComponent(typeof(BoxCollider))]

public class PlantController : MonoBehaviour
{
    [Header("Growth Settings")]
    [SerializeField, Tooltip("Total time (seconds) to reach full growth when properly watered.")] private float _growthDuration = 60f;
    [SerializeField, Tooltip("Scale of the plant at full maturity.")] private Vector3 _maxScale = Vector3.one;
    [SerializeField, Tooltip("Curve to control scale over normalized growth progress.")] private AnimationCurve _growthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField, Tooltip("The harvest that will be given out after harvesting")] private ItemData _itemData;

    //[Header("Watering Settings")]
    //[SerializeField, Tooltip("How much water (units) the plant can hold.")] private float _maxMoisture = 100f;
    //[SerializeField, Tooltip("Rate at which moisture decreases per second.")] private float _moistureDecayRate = 0.5f;
    //[SerializeField, Tooltip("Minimum moisture required to grow.")] private float _moistureThreshold = 20f;

    [Header("FX Settings")]
    [SerializeField] private FXPool _fxPool;

    [Header("UI Settings")]
    [SerializeField] private LocalizedString _localizedHarvestString;
    [SerializeField, Tooltip("Pre-harvest timer UI")] private TextMeshProUGUI _harvestTimer;

    public event Action OnMature;
    //public event Action OnDry;

    private BoxCollider _boxCollider;
    //private float _currentMoisture;
    private float _elapsedGrowthTime;
    private bool _isMature;

    private void Awake()
    {
        if (_itemData == null) Debug.LogWarning("The seedling crop has not been initialized, and nothing will be given during harvesting!");

        _boxCollider = GetComponent<BoxCollider>();
        _fxPool = GetComponent<FXPool>();

        transform.localScale = Vector3.zero;
        //_currentMoisture = _maxMoisture * 0.5f;
        _elapsedGrowthTime = 0f;
        _isMature = false;
    }

    private void Update()
    {
        HandleInput();

        _harvestTimer.transform.LookAt(Camera.main.transform);
        _harvestTimer.transform.rotation = Camera.main.transform.rotation;

        if (_isMature)
        {
            _localizedHarvestString.StringChanged += s => _harvestTimer.text = s;
            return;
        }

        //_currentMoisture = Mathf.Max(0f, _currentMoisture - _moistureDecayRate * Time.deltaTime);

        //if (_currentMoisture < _moistureThreshold)
        //{
        //    OnDry?.Invoke();
        //    return;
        //}

        _elapsedGrowthTime = Mathf.Min(_growthDuration, _elapsedGrowthTime + Time.deltaTime);
        float normalizedProgress = Mathf.Clamp01(_elapsedGrowthTime / _growthDuration);

        float scaleFactor = _growthCurve.Evaluate(normalizedProgress);
        transform.localScale = Vector3.Lerp(Vector3.zero, _maxScale, scaleFactor);

        UpdateUI(Mathf.CeilToInt(_elapsedGrowthTime));

        if (normalizedProgress >= 1f)
        {
            _isMature = true;
            OnMature?.Invoke();
        }
    }

    public void HandleInput()
    {
        if (_isMature && Input.GetMouseButtonDown(0))
        {
            Harvest();
        }
    }
    public void Harvest()
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit) && hit.collider == _boxCollider)
        {
            if (_itemData != null)
                InventoryUI.Instance.AddItem(_itemData, _itemData.DefaultCount);

            _fxPool.DestroyParent(gameObject, hit.point);
        }
    }

    public void UpdateUI(float elapsedGrowthTime)
    {
        float remainingTime = _growthDuration - elapsedGrowthTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);

        if (timeSpan.Minutes > 1)
        {
            _harvestTimer.text = string.Format("{0:D2}m : {1:D2}s", timeSpan.Minutes, timeSpan.Seconds);
        }
        else
        {
            _harvestTimer.text = string.Format("{0:D2}s", timeSpan.Seconds);
        }
    }

    //public void Water(float amount)
    //{
    //    if (_isMature) return;
    //    _currentMoisture = Mathf.Clamp(_currentMoisture + amount, 0f, _maxMoisture);
    //}
    //public float GetMoisture() => _currentMoisture;


    public float GetGrowthProgress() => Mathf.Clamp01(_elapsedGrowthTime / _growthDuration);
}
