using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Shop")]
    [Space]

    [SerializeField] private Canvas _shopCanvas;
    [SerializeField] private Button _exitButton;

    [Header("Backpack")]
    [Space]

    [SerializeField] private GameObject _backpackPanel;
    [SerializeField] private Button _backpackButton;
    private bool _openBackpack = false;

    private void Start()
    {
        _exitButton?.onClick.RemoveAllListeners();
        _exitButton?.onClick.AddListener(CloseShopCanvas);

        _backpackButton?.onClick.RemoveAllListeners();
        _backpackButton?.onClick.AddListener(OpenCloseBackpackPanel);
        _backpackPanel?.gameObject.SetActive(_openBackpack);

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) OpenCloseBackpackPanel();
    }
    private void CloseShopCanvas()
    {
        _shopCanvas?.gameObject.SetActive(false);
    }

    private void OpenCloseBackpackPanel()
    {
        _openBackpack = !_openBackpack;
        _backpackPanel?.gameObject.SetActive(_openBackpack);
    }
}
