using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class crono : MonoBehaviour
{
    private TextMeshProUGUI textoCrono;
    [SerializeField] private float tiempo = 0;

    private int tiempoMinuto = 0;
    private int tiempoDecimasSegundo = 0;
    private int tiemposegundos = 0;

    void Start()
    {
        textoCrono = GameObject.Find("TextoCronoUI").GetComponent<TextMeshProUGUI>();
        if (textoCrono == null)
        {
            Debug.LogError("No se encontró el objeto TextoCronoUI con TextMeshProUGUI");
        }
    }

    void Cronometro()
    {
        tiempo += Time.deltaTime;

        tiempoMinuto = Mathf.FloorToInt(tiempo / 60);
        tiemposegundos = Mathf.FloorToInt(tiempo % 60);
        tiempoDecimasSegundo = Mathf.FloorToInt((tiempo % 1) * 100);

        textoCrono.text = string.Format("{0:00}:{1:00}:{2:00}", tiempoMinuto, tiemposegundos, tiempoDecimasSegundo);
    }

    void Update()
    {
        if (textoCrono != null)
        {
            Cronometro();
        }
    }
}
