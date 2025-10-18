using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (Gamepad.current != null && Gamepad.current.startButton.isPressed))
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    void UnpauseGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
