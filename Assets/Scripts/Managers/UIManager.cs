using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Shop")]
    [Space]

    [SerializeField] private Canvas _buyerCanvas;
    [SerializeField] private Button _exitShopButton;

    [Header("Seller")]
    [Space]

    [SerializeField] private Canvas _sellerCanvas;
    [SerializeField] private Button _exitSellerButton;


    [Header("Backpack")]
    [Space]

    [SerializeField] private GameObject _backpackPanel;
    [SerializeField] private Button _backpackButton;
    private bool _openBackpack = false;

    private void Start()
    {
        _exitShopButton?.onClick.RemoveAllListeners();
        _exitShopButton?.onClick.AddListener(CloseShopCanvas);

        _exitSellerButton?.onClick.RemoveAllListeners();
        _exitSellerButton?.onClick?.AddListener(CloseSellerCanvas);

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
        _buyerCanvas?.gameObject.SetActive(false);
    }

    private void CloseSellerCanvas()
    {
        _sellerCanvas?.gameObject.SetActive(false);
    }

    private void OpenCloseBackpackPanel()
    {
        _openBackpack = !_openBackpack;
        _backpackPanel?.gameObject.SetActive(_openBackpack);
    }
}
