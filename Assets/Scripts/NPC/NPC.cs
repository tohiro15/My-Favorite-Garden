using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _interactionButton;
    [SerializeField] private float _interactionDistance = 3f;
    [SerializeField] private NPCType _npcType = NPCType.None;
    private bool _isPlayerNear;
    private void Update()
    {
        CheckDistance();
        if (_isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.Instance.OpenNPCPanel(_npcType);
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

            if (_npcType != NPCType.None)
            {
                _interactionButton?.SetActive(true);
                UIManager.Instance.EnableInteractionButton(() => UIManager.Instance.OpenNPCPanel(_npcType), this);
            }
        }
        else
        {
            _isPlayerNear = false;
            _interactionButton?.SetActive(false);
            UIManager.Instance.DisableInteractionButton(this);
        }
    }
}