using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class Portal : MonoBehaviour
{
    public EnemyManager enemyManager;
    public event EventHandler TocaJugadorPortal;    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyManager && enemyManager.AreAllEnemiesDestroyed())
        {
            TocaJugadorPortal?.Invoke(this, EventArgs.Empty);
        }
    }


}
