using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [Space]

    [SerializeField] private PlayerStatistic _playerStatistics;
    [SerializeField] private TMP_Text _moneyCount;

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

        if (_playerStatistics != null)
            _playerStatistics.OnMoneyChanged += UpdateMoneyCount;

        UpdateMoneyCount(_playerStatistics.Money);
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

    public void UpdateMoneyCount(int money)
    {
        _moneyCount.text = $"{money}";
    }
}
