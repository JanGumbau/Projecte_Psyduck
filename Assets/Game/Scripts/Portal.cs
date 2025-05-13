using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public EnemyManager enemyManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemyManager && enemyManager.AreAllEnemiesDestroyed())
        {
            // Detener el cron�metro
            crono cronometro = FindObjectOfType<crono>();
            if (cronometro != null)
            {
                cronometro.StopCrono();
            }

           // Cambiar de escena
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
