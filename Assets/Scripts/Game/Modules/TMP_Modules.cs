using TMPro;
using UnityEngine;

public class TMP_Modules : MonoBehaviour
{
    [SerializeField] private Vector3 _pressedPosition = new Vector3(0, -2, 0);
    private Vector3 _normalPosition;

    private RectTransform m_RectTransform;
    private TextMeshProUGUI m_TextMeshProUGUI;
    private void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        if (m_TextMeshProUGUI != null)
        {
            m_RectTransform = m_TextMeshProUGUI.rectTransform;
            _normalPosition = m_RectTransform.localPosition;
        }
    }
    public void SetPosition(Vector3 pos)
    {
        if (m_TextMeshProUGUI == null || m_RectTransform == null) return;

        m_RectTransform.localPosition = pos;
    }

    public void SetPressedPosition()
    {
        SetPosition(_pressedPosition);
    }

    public void SetNormalPosition()
    {
        SetPosition(_normalPosition);
    }

}
