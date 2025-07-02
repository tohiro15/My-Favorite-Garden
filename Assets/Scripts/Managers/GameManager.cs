using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _exitAction;

    void Awake()
    {
        _exitAction = _playerInput.actions["Escape"];
    }

    private void OnEnable()
    {
        _exitAction.Enable();
    }

    private void OnDisable()
    {
        _exitAction.Disable();
    }

    void Update()
    {
        if (_exitAction.triggered)
        {
            ExitGame();
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exit pressed, quitting...");
        Application.Quit();
#if UNITY_EDITOR
        // В редакторе выйдем из Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
