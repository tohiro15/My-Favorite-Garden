using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exit pressed, quitting...");
        Application.Quit();
#if UNITY_EDITOR
        // � ��������� ������ �� Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
