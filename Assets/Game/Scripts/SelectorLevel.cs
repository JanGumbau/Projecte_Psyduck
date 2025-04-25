using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectorLevel : MonoBehaviour
{
    public void CambiarNivel(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }

    
}
