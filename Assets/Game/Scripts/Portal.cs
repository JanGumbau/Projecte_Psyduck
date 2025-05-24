using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public EnemyManager enemyManager;
    public GameObject panelUI; 

    void Start()
    {

        if (panelUI != null)
        {
            panelUI.SetActive(false); 
            Cursor.visible = false;
        }

    }

    private bool hasActivated = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!hasActivated && other.CompareTag("Player") && enemyManager && enemyManager.AreAllEnemiesDestroyed())
        {
            hasActivated = true;

            crono cronometro = FindObjectOfType<crono>();
            if (cronometro != null)
            {
                cronometro.StopCrono();
            }

            if (panelUI != null)
            {
                panelUI.SetActive(true);
                Cursor.visible = true;
            }
        }
    }

}
