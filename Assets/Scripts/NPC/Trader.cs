using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _interactionButton;
    [SerializeField] private float _interactionDistance = 3f;
    private bool _isPlayerNear;
    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        Vector3 offset = transform.position - _player.transform.position;
        float sqrDistance = offset.sqrMagnitude;

        if (sqrDistance < _interactionDistance * _interactionDistance)
        {
            _isPlayerNear = true;

            _interactionButton?.SetActive(true);
            Debug.Log("Trader - находится в зоне активности.");
        }
        else
        {
            _isPlayerNear = false;
            _interactionButton?.SetActive(false);
        }
    }
}
