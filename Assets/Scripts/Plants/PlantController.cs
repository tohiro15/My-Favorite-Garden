using System;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [Header("Growth Settings")]
    [SerializeField, Tooltip("Total time (seconds) to reach full growth when properly watered.")] private float _growthDuration = 60f;
    [SerializeField, Tooltip("Scale of the plant at full maturity.")] private Vector3 _maxScale = Vector3.one;
    [SerializeField, Tooltip("Curve to control scale over normalized growth progress.")] private AnimationCurve _growthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    //[Header("Watering Settings")]
    //[SerializeField, Tooltip("How much water (units) the plant can hold.")] private float _maxMoisture = 100f;
    //[SerializeField, Tooltip("Rate at which moisture decreases per second.")] private float _moistureDecayRate = 0.5f;
    //[SerializeField, Tooltip("Minimum moisture required to grow.")] private float _moistureThreshold = 20f;

    public event Action OnMature;
    //public event Action OnDry;

    //private float _currentMoisture;
    private float _elapsedGrowthTime;
    private bool _isMature;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        //_currentMoisture = _maxMoisture * 0.5f;
        _elapsedGrowthTime = 0f;
        _isMature = false;
    }

    private void Update()
    {
        if (_isMature) return;

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

        if (normalizedProgress >= 1f)
        {
            _isMature = true;
            OnMature?.Invoke();
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
