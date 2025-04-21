using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class crono : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoCrono;
    [SerializeField] private float tiempo = 0;
    

    

    private int tiempoMinuto = 0;
    private int tiempoDecimasSegundo = 0;
    private int tiemposegundos = 0;


    void Cronometro()
    {
       
        
            tiempo += Time.deltaTime;
     
        tiempoMinuto = Mathf.FloorToInt(tiempo / 60);
        tiemposegundos = Mathf.FloorToInt(tiempo % 60);
        tiempoDecimasSegundo = Mathf.FloorToInt((tiempo%1)*100);

        textoCrono.text = string.Format("{0:00}:{1:00}:{2:00}", tiempoMinuto, tiemposegundos, tiempoDecimasSegundo);
        if(tiempo<=5)
        {
            
           
           
        }
        if(tiempo>=7)
        {
            
            
        }
        if(tiempo>=10)
        {
         
           
        }
    }
    

 
    void Update()
    {
        Cronometro(); 
    }
}
