using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public EnemyManager enemyManager;
    public GameObject panelUI; // ← Asigna el panel desde el Inspector

    void Start()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(false); // Asegúrate de que el panel esté oculto al inicio
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemyManager && enemyManager.AreAllEnemiesDestroyed())
        {
            // Detener el cronómetro
            crono cronometro = FindObjectOfType<crono>();
            if (cronometro != null)
            {
                cronometro.StopCrono();
            }

            // Mostrar el panel
            if (panelUI != null)
            {
                panelUI.SetActive(true);
            }
        }
    }
}
