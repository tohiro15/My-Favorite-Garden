using GamePush;
using UnityEngine;

public class MobileDisableAutoSwitchControls : MonoBehaviour
{
    [SerializeField] private Canvas _mobileJoysticksCanvas;
    private void Start()
    {
        _mobileJoysticksCanvas = GetComponent<Canvas>();
        if (_mobileJoysticksCanvas != null)
        {
            if (GP_Device.IsMobile()) _mobileJoysticksCanvas.gameObject.SetActive(true);
            else _mobileJoysticksCanvas.gameObject.SetActive(false);
        }
    }
}
