using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Vector3 respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = transform.position;
    }
    public void RespawnCharacter()
    {
        transform.position = respawnPoint;
    }
    // Update is called once per frame
     public void UpdateRespawnPoint(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ENEMIC"))
        {
            GetComponent<Respawn>().RespawnCharacter();
        }
    }

}

