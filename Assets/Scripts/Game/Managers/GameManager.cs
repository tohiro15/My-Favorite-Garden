using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _mainMenuSceneIndex = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(_mainMenuSceneIndex);
    }
}
