using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprova si l'enemic xoca contra un objecte amb el tag "GROUND"
        if (collision.gameObject.CompareTag("GROUND"))
        {
            Destroy(gameObject); // Destrueix aquest GameObject (l'enemic)
        }
    }
}
