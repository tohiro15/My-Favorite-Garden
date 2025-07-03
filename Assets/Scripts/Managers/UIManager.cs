using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas _shopCanvas;
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        _exitButton?.onClick.RemoveAllListeners();
        _exitButton?.onClick.AddListener(CloseShopCanvas);
    }
    private void CloseShopCanvas()
    {
        _shopCanvas?.gameObject.SetActive(false);
    }
}
