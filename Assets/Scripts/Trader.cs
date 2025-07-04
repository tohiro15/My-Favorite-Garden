using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _interactionButton;
    [SerializeField] private GameObject _traderCanvas;
    [SerializeField] private float _interactionDistance = 3f;
    private bool _isPlayerNear;
    private void Start()
    {
        if (_traderCanvas != null) _traderCanvas?.SetActive(false);
        else Debug.Log("ShopCanvas - not found!");
    }
    private void Update()
    {
        CheckDistance();
        if (_traderCanvas != null && Input.GetKeyDown(KeyCode.E) && _isPlayerNear)
        {
            OpenCanvas();
        }
    }

    private void CheckDistance()
    {
        Vector3 offset = transform.position - _player.transform.position;
        float sqrDistance = offset.sqrMagnitude;

        if (sqrDistance < _interactionDistance * _interactionDistance)
        {
            _isPlayerNear = true;

            _interactionButton?.SetActive(true);
        }
        else
        {
            _isPlayerNear = false;
            _interactionButton?.SetActive(false);
        }
    }

    private void OpenCanvas()
    {
        _traderCanvas?.SetActive(true);
    }
}