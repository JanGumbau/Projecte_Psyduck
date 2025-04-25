using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour

{
    public int enemiesCount = 0;
    public int enemiesDestroyed = 0;
    
    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("EnemyManager");
                    instance = singletonObject.AddComponent<EnemyManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
    public bool AreAllEnemiesDestroyed()
    {
        return enemiesCount == enemiesDestroyed;
    } 
   
}
