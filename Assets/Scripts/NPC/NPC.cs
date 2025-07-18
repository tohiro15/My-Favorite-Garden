using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _interactionButton;
    [SerializeField] private Canvas _npcCanvas;
    [SerializeField] private float _interactionDistance = 3f;
    private bool _isPlayerNear;
    private void Start()
    {
        if (_npcCanvas != null) _npcCanvas?.gameObject.SetActive(false);
        else Debug.Log("ShopCanvas - not found!");
    }
    private void Update()
    {
        CheckDistance();
        if (_npcCanvas != null && _isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenCanvas();
            }
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
            UIManager.Instance.EnableInteractionButton(OpenCanvas, this);
            Debug.Log("Игрок рядом");
        }
        else
        {
            _isPlayerNear = false;
            _interactionButton?.SetActive(false);
            UIManager.Instance.DisableInteractionButton(this);
            Debug.Log("Игрок далеко");
        }
    }

    private void OpenCanvas()
    {
        _npcCanvas?.gameObject.SetActive(true);
    }
}