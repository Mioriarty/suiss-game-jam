using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseHandler : MonoBehaviour
{
    public GameObject player;
    public GameObject loadManager;
    Vector3 playerStart;

    void Start()
    {
        playerStart = player.transform.position;
    }
    public GameObject pauseMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (Gamepad.current != null && Gamepad.current.startButton.isPressed))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void ToMainMenu()
    {
        UnpauseGame();
        loadManager.GetComponent<LoadController>().LoadScene("MainMenu");
    }
    public void ResetPosition()
    {
        player.transform.position = playerStart;
        UnpauseGame();
    }
}
