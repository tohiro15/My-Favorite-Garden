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

    [Header("Plant")]
    [SerializeField] private Canvas _plantCanvas;
    [SerializeField] private GameObject _plantPanel;
    [SerializeField] private Button _exitPlantButton;

    [Header("Backpack")]
    [SerializeField] private GameObject _backpackPanel;
    [SerializeField] private Button _backpackButton;

    private Vector2 _interactionButtonTargetPosition;

    private object _currentInteractionSource;
    private object _currentExitSource;

    private bool _anyPanelOpen = false;
    private bool _isAnimatingPanel = false;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        _interactionButtonTargetPosition = _interactionButton.GetComponent<RectTransform>().anchoredPosition;

        _HUDCanvas.gameObject.SetActive(true);

        _buyerCanvas.gameObject.SetActive(true);
        _buyerPanel.SetActive(false);
        _exitBuyerButton.onClick.AddListener(() => ClosePanel(_buyerPanel));

        _sellerCanvas.gameObject.SetActive(true);
        _sellerPanel.SetActive(false);
        _exitSellerButton.onClick.AddListener(() => ClosePanel(_sellerPanel));

        _plantCanvas?.gameObject.SetActive(true);
        _plantPanel?.gameObject.SetActive(false);
        _exitPlantButton?.onClick.AddListener(() => ClosePanel(_plantPanel));

        _interactionButton.gameObject.SetActive(false);

        _backpackButton.onClick.AddListener(() => ToggleBackpackPanel(_backpackPanel));
        _backpackPanel.SetActive(false);

        PlayerStatistic.Instance.OnMoneyChanged += UpdateMoneyCount;
        UpdateMoneyCount(PlayerStatistic.Instance.Money);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) ToggleBackpackPanel(_backpackPanel);
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

        if (panel == _backpackPanel && playOpenSound)
        {
            SoundManager.Instance.PlayBackpackOpenSound();
        }
        else if (playOpenSound)
        {
            SoundManager.Instance.PlayOpenPanelSound();
        }
    }

    public void ClosePanel(GameObject panel, bool playOpenSound = false)
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

        if (panel == _backpackPanel && playOpenSound)
        {
            SoundManager.Instance.PlayBackpackCloseSound();
        }
    }

    public void ToggleBackpackPanel(GameObject panel)
    {
        bool opening = !panel.activeSelf;
        if (opening)
        {
            OpenPanel(panel, true);
        }
        else
        {
            ClosePanel(panel, true);
        }
    }

    public void ToggleBuyerPanel()
    {
        bool opening = !_buyerPanel.activeSelf;
        if (opening)
        {
            OpenPanel(_buyerPanel);
        }
        else
        {
            ClosePanel(_buyerPanel);
        }
    }

    public void OpenNPCPanel(NPCType npcType)
    {
        switch (npcType)
        {
            case NPCType.None:

                break;
            case NPCType.Seller:

                OpenPanel(_sellerPanel);

                break;
            case NPCType.Buyer:

                OpenPanel(_buyerPanel);

                break;
        }
    }
    public void OpenPlantPanel() => OpenPanel(_plantPanel);
    public void ClosePlantPanel() => ClosePanel(_plantPanel);

    public void EnableInteractionButton(UnityAction call, object source)
    {
        if (_anyPanelOpen) return;
        if (_currentInteractionSource == source) return;
        if (_interactionButton == null) return;

        _currentInteractionSource = source;
        _interactionButton.onClick.RemoveAllListeners();
        _interactionButton.onClick.AddListener(call);

        RectTransform rt = _interactionButton.GetComponent<RectTransform>();
        rt.DOKill();

        rt.anchoredPosition = new Vector2(_interactionButtonTargetPosition.x, -Screen.height);
        _interactionButton.gameObject.SetActive(true);

        rt.DOAnchorPos(_interactionButtonTargetPosition, 0.3f)
          .SetEase(Ease.OutBack);
    }



    public void DisableInteractionButton(object source)
    {
        if (_currentInteractionSource != source) return;
        if (_interactionButton == null) return;

        _currentInteractionSource = null;
        _interactionButton.onClick.RemoveAllListeners();

        RectTransform rt = _interactionButton.GetComponent<RectTransform>();
        rt.DOKill();

        rt.DOAnchorPos(new Vector2(_interactionButtonTargetPosition.x, -Screen.height), 0.3f)
          .SetEase(Ease.InBack)
          .OnComplete(() => _interactionButton.gameObject.SetActive(false));
    }


    public void EnableExitButton(UnityAction call, object source)
    {
        if (_currentExitSource == source) return;

        _currentExitSource = source;
        _exitPlantButton.onClick.RemoveAllListeners();
        _exitPlantButton.onClick.AddListener(call);
        _exitPlantButton.gameObject.SetActive(true);
    }

    public void DisableExitButton(object source)
    {
        if (_currentExitSource != source) return;

        _currentExitSource = null;
        _exitPlantButton.onClick.RemoveAllListeners();
        ClosePlantPanel();
    }
}
