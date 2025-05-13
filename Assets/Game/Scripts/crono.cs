using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class crono : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private float timeElapsed;
    private int minutes, seconds, miliseconds;

    private Transform player;
    private Vector3 initialPosition;
    private bool hasPlayerMoved = false;
    private bool isRunning = true;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            initialPosition = player.position;
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con el tag 'Player'");
        }
    }

    private void Update()
    {
        if (!hasPlayerMoved && player != null)
        {
            if (Vector3.Distance(player.position, initialPosition) > 0.10f)
            {
                hasPlayerMoved = true;
            }
        }

        if (hasPlayerMoved && isRunning)
        {
            timeElapsed += Time.deltaTime;
            minutes = (int)(timeElapsed / 60f);
            seconds = (int)(timeElapsed % 60f);
            miliseconds = (int)((timeElapsed % 1f) * 100f);

            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);
        }
    }

    public void StopCrono()
    {
        isRunning = false;
    }
}
