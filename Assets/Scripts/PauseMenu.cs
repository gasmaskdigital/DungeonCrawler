using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject container;
    [SerializeField] GameObject inventory;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObject panel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            togglePause();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggleInventory();
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

    public void toggleInventory()
    {
        if (Time.timeScale == 1f)
        {
            Inventory();
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

    public void Inventory()
    {
        // Enable the container and pause menu
        inventory.SetActive(true);
        
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Home()
    {
        Time.timeScale = 1f;
        playerStats.ResetStats();
        levelManager.currentLevel = 0;
        PlayerStats.healthPotionStack = 0;
        PlayerStats.strengthPotionStack = 0;
        PlayerStats.dexterityPotionStack = 0;
        PlayerStats.magicPotionStack = 0;
        SceneManager.LoadScene("Mainmenu");
    }

    public void Resume()
    {
        // Disable the container and pause menu
        pauseMenu.SetActive(false);
        container.SetActive(false);
        inventory.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        playerStats.ResetStats();
        levelManager.currentLevel = 0;
        PlayerStats.healthPotionStack = 0;
        PlayerStats.strengthPotionStack = 0;
        PlayerStats.dexterityPotionStack = 0;
        PlayerStats.magicPotionStack = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

