using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Portal : MonoBehaviour
{
    public EnemyManager enemyManager;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyManager && enemyManager.AreAllEnemiesDestroyed())
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

}
