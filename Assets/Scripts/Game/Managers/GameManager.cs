using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private int _mainMenuSceneIndex = 0;
    void Update()
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
