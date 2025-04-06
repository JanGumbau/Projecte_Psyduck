using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public CharacterController controller;
    
    public  static bool GameIsPaused = false;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
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
        
        Time.timeScale = 1f; //reprèn el joc
        GameIsPaused = false;
        
       
    }

    void Pause()
    {
        //pausar joc
        GameIsPaused = true;
        
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
       
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame() called");
        Application.Quit();
    }

    public void RestartGame()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu_scene");
    }
}

