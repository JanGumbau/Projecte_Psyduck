using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class canvi_escenea : MonoBehaviour
{

    public void LoadScene(string nextScene)
    {
        SceneManager.LoadScene("tutorial_1");
        Cursor.visible = false;
         
          



        
    }
    public void QuitGame()
    {
        Application.Quit();
    }  
}
