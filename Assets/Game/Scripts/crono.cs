using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class crono : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [Header("Configuración de Tiempos")]
    [SerializeField] private float tiempoImagen1 = 12f;
    [SerializeField] private float tiempoImagen2 = 10f;
    [SerializeField] private float tiempoImagen3 = 7f;
    [SerializeField] private float tiempoImagen4 = 5f;

    [Header("Imágenes para ≤ 5 segundos")]
    [SerializeField] private Image imagen7;
    [SerializeField] private Image imagen8;
    [SerializeField] private Image imagen9;
    [SerializeField] private Image imagen10;

    [Header("Imágenes para ≤ 7 segundos")]
    [SerializeField] private Image imagen4;
    [SerializeField] private Image imagen5;
    [SerializeField] private Image imagen6;

    [Header("Imágenes para ≤ 10 segundos")]
    [SerializeField] private Image imagen2;
    [SerializeField] private Image imagen3;

    [Header("Imágenes para ≤ 12 segundos")]
    [SerializeField] private Image imagen1;

    private float timeElapsed;
    private int minutes, seconds, miliseconds;

    private Transform player;
    private Vector3 initialPosition;
    private bool hasPlayerMoved = false;
    private bool isRunning = true;

    private bool imagenesFijadas = false;

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

        OcultarTodasLasImagenes();
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

            MostrarImagenesSegunTiempo();
        }
    }

    private void MostrarImagenesSegunTiempo()
    {
        if (timeElapsed > tiempoImagen1 && !imagenesFijadas)
        {
           
            imagenesFijadas = true;
            return;
        }

        if (imagenesFijadas) return;

        OcultarTodasLasImagenes();

        if (timeElapsed <= tiempoImagen1)
            imagen1.gameObject.SetActive(true);

        if (timeElapsed <= tiempoImagen2)
        {
            imagen2.gameObject.SetActive(true);
            imagen3.gameObject.SetActive(true);
        }

        if (timeElapsed <= tiempoImagen3)
        {
            imagen4.gameObject.SetActive(true);
            imagen5.gameObject.SetActive(true);
            imagen6.gameObject.SetActive(true);
        }

        if (timeElapsed <= tiempoImagen4)
        {
            imagen7.gameObject.SetActive(true);
            imagen8.gameObject.SetActive(true);
            imagen9.gameObject.SetActive(true);
            imagen10.gameObject.SetActive(true);
        }
    }

    private void OcultarTodasLasImagenes()
    {
        imagen1.gameObject.SetActive(false);
        imagen2.gameObject.SetActive(false);
        imagen3.gameObject.SetActive(false);
        imagen4.gameObject.SetActive(false);
        imagen5.gameObject.SetActive(false);
        imagen6.gameObject.SetActive(false);
        imagen7.gameObject.SetActive(false);
        imagen8.gameObject.SetActive(false);
        imagen9.gameObject.SetActive(false);
        imagen10.gameObject.SetActive(false);
    }

    public void StopCrono()
    {
        isRunning = false;
    }

   
    public void SetTiempoImagen1(float tiempo)
    {
        tiempoImagen1 = tiempo; 
    }
    public void SetTiempoImagen2(float tiempo)
    { 
        tiempoImagen2 = tiempo;
    }
    public void SetTiempoImagen3(float tiempo)
    {
        tiempoImagen3 = tiempo;
    }
    public void SetTiempoImagen4(float tiempo) 
    {
        tiempoImagen4 = tiempo;
    }
}