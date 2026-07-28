using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject container;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            togglePause();
        }
    }

    public void togglePause() 
    {
        if (Time.timeScale == 1f)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        // Enable the container and pause menu
        container.SetActive(true);
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void Resume()
    {
        // Disable the container and pause menu
        pauseMenu.SetActive(false);
        container.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

