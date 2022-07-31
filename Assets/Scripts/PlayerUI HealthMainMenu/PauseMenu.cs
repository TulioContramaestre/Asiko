using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool GamePaused = false;
    public GameObject pauseMenu;
    public GameObject settings;
    public GameObject ConfirmationMenu;
    public GameObject ConfirmationRestart;
    public GameObject cheats;

    PlayerHealth player;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        settings.SetActive(false);
        cheats.SetActive(false);
        ConfirmationMenu.SetActive(false);
        ConfirmationRestart.SetActive(false);
        GamePaused = false;
        Time.timeScale = 1f;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void Restart()
    {
        Resume();
        player = GameObject.Find("Player_Stickman").GetComponent<PlayerHealth>();
        PlayerHealth.curHealth = 0;
        player.death();
    }

    public void Menu()
    {
        Resume();
        //Destroy(GameObject.Find("Player_Stickman"));
        SceneManager.LoadScene("Menu");
    }
}
