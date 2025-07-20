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
    [SerializeField] private Canvas _HUDCanvas;
    [SerializeField] private Button _interactionButton;
    [SerializeField] private TMP_Text _moneyCount;

    [Header("Buyer")]
    [SerializeField] private Canvas _buyerCanvas;
    [SerializeField] private GameObject _buyerPanel;
    [SerializeField] private Button _exitBuyerButton;

    [Header("Seller")]
    [SerializeField] private Canvas _sellerCanvas;
    [SerializeField] private GameObject _sellerPanel;
    [SerializeField] private Button _exitSellerButton;

    [Header("Backpack")]
    [SerializeField] private GameObject _backpackPanel;
    [SerializeField] private Button _backpackButton;

    private object _currentInteractionSource;
    private bool _anyPanelOpen = false;
    private bool _isAnimatingPanel = false;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        _HUDCanvas.gameObject.SetActive(true);

        _buyerCanvas.gameObject.SetActive(true);
        _buyerPanel.SetActive(false);
        _exitBuyerButton.onClick.AddListener(() => ClosePanel(_buyerPanel));

        _sellerCanvas.gameObject.SetActive(true);
        _sellerPanel.SetActive(false);
        _exitSellerButton.onClick.AddListener(() => ClosePanel(_sellerPanel));

        _interactionButton.gameObject.SetActive(false);

        _backpackButton.onClick.AddListener(OpenCloseBackpackPanel);
        _backpackPanel.SetActive(false);

        PlayerStatistic.Instance.OnMoneyChanged += UpdateMoneyCount;
        UpdateMoneyCount(PlayerStatistic.Instance.Money);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) OpenCloseBackpackPanel();
    }

    private void UpdateMoneyCount(int money)
    {
        var nfi = new NumberFormatInfo
        {
            NumberGroupSeparator = ".",
            NumberDecimalDigits = 0
        };
        _moneyCount.text = money.ToString("N0", nfi);
    }

    public void OpenPanel(GameObject panel, bool playOpenSound = false)
    {
        if (panel == null || _anyPanelOpen || _isAnimatingPanel) return;

        _isAnimatingPanel = true;
        _anyPanelOpen = true;
        _interactionButton.gameObject.SetActive(false);

        RectTransform rt = panel.GetComponent<RectTransform>();
        panel.SetActive(true);

        rt.DOKill();

        rt.anchoredPosition = new Vector2(0, -Screen.height);
        rt.DOAnchorPos(Vector2.zero, 0.5f)
          .SetEase(Ease.OutBack)
          .OnComplete(() =>
          {
              _isAnimatingPanel = false;
          });

        if (playOpenSound) SoundManager.Instance.PlayOpenPanelSound();
    }

    public void ClosePanel(GameObject panel)
    {
        if (panel == null || !_anyPanelOpen || _isAnimatingPanel) return;

        _isAnimatingPanel = true;

        RectTransform rt = panel.GetComponent<RectTransform>();

        rt.DOKill();

        rt.DOAnchorPos(new Vector2(0, -Screen.height), 0.5f)
          .SetEase(Ease.InBack)
          .OnComplete(() =>
          {
              panel.SetActive(false);
              _anyPanelOpen = false;
              _isAnimatingPanel = false;
              _currentInteractionSource = null;
          });
    }

    public void OpenCloseBackpackPanel()
    {
        bool opening = !_backpackPanel.activeSelf;
        if (opening)
        {
            OpenPanel(_backpackPanel);
            SoundManager.Instance.PlayBackpackOpenSound();
        }
        else
        {
            ClosePanel(_backpackPanel);
            SoundManager.Instance.PlayBackpackCloseSound();
        }
    }

    public void EnableInteractionButton(UnityAction call, object source)
    {
        if (_anyPanelOpen) return;
        if (_currentInteractionSource == source) return;

        _currentInteractionSource = source;
        _interactionButton.onClick.RemoveAllListeners();
        _interactionButton.onClick.AddListener(call);
        _interactionButton.gameObject.SetActive(true);
    }

    public void DisableInteractionButton(object source)
    {
        if (_currentInteractionSource != source) return;

        _currentInteractionSource = null;
        _interactionButton.onClick.RemoveAllListeners();
        _interactionButton.gameObject.SetActive(false);
    }
}
