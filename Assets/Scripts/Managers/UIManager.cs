using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    [Space]
    [SerializeField] private Canvas _HUDCanvas;
    [SerializeField] private Button _interactionButton;
    [SerializeField] private TMP_Text _moneyCount;

    [Header("Buyer")]
    [Space]
    [SerializeField] private Canvas _buyerCanvas;
    [SerializeField] private Button _exitBuyerButton;

    [Header("Seller")]
    [Space]
    [SerializeField] private Canvas _sellerCanvas;
    [SerializeField] private Button _exitSellerButton;

    [Header("Backpack")]
    [Space]
    [SerializeField] private GameObject _backpackPanel;
    [SerializeField] private Button _backpackButton;

    private bool _openBackpack = false;
    private object _currentInteractionSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _HUDCanvas?.gameObject.SetActive(true);
        _buyerCanvas?.gameObject.SetActive(false);
        _sellerCanvas?.gameObject.SetActive(false);

        _interactionButton?.onClick.RemoveAllListeners();
        _interactionButton?.gameObject.SetActive(false);

        _exitBuyerButton?.onClick.RemoveAllListeners();
        _exitBuyerButton?.onClick.AddListener(CloseShopCanvas);

        _exitSellerButton?.onClick.RemoveAllListeners();
        _exitSellerButton?.onClick.AddListener(CloseSellerCanvas);

        _backpackButton?.onClick.RemoveAllListeners();
        _backpackButton?.onClick.AddListener(OpenCloseBackpackPanel);
        _backpackPanel?.gameObject.SetActive(_openBackpack);

        PlayerStatistic.Instance.OnMoneyChanged += UpdateMoneyCount;
        UpdateMoneyCount(PlayerStatistic.Instance.Money);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            OpenCloseBackpackPanel();
    }

    public void OpenCanvas(Canvas canvas)
    { 
        canvas?.gameObject.SetActive(true);
    }
    public void CloseShopCanvas()
    {
        _buyerCanvas?.gameObject.SetActive(false);
    }
    public void CloseSellerCanvas()
    {
        _sellerCanvas?.gameObject.SetActive(false);
    }

    public void OpenCloseBackpackPanel()
    {
        _openBackpack = !_openBackpack;
        _backpackPanel?.gameObject.SetActive(_openBackpack);
    }

    public void EnableInteractionButton(UnityAction call, object source)
    {
        if (_currentInteractionSource == source) return;

        Debug.Log("Кнопка включена");
        _currentInteractionSource = source;

        _interactionButton?.gameObject.SetActive(true);
        _interactionButton?.onClick.RemoveAllListeners();
        _interactionButton?.onClick.AddListener(call);
    }

    public void DisableInteractionButton(object source)
    {
        if (_currentInteractionSource != source) return;

        Debug.Log("Кнопка выключена");
        _currentInteractionSource = null;

        _interactionButton?.onClick.RemoveAllListeners();
        _interactionButton?.gameObject.SetActive(false);
    }

    public void UpdateMoneyCount(int money)
    {
        var nfi = new NumberFormatInfo
        {
            NumberGroupSeparator = ".",
            NumberDecimalDigits = 0
        };
        _moneyCount.text = money.ToString("N0", nfi);
    }
}
