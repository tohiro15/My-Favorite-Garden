using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _interactionButton;
    [SerializeField] private GameObject _npcPanel;
    [SerializeField] private float _interactionDistance = 3f;
    private bool _isPlayerNear;
    private void Start()
    {
        if (_npcPanel != null) _npcPanel?.gameObject.SetActive(false);
        else Debug.Log("ShopPanel - not found!");
    }
    private void Update()
    {
        CheckDistance();
        if (_npcPanel != null && _isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.Instance.OpenPanel(_npcPanel, true);
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
            UIManager.Instance.EnableInteractionButton(() => UIManager.Instance.OpenPanel(_npcPanel),this);
        }
        else
        {
            _isPlayerNear = false;
            _interactionButton?.SetActive(false);
            UIManager.Instance.DisableInteractionButton(this);
        }
    }
}