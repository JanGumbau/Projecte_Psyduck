using UnityEngine;
using UnityEngine.UI;

public class cambio_imagen : MonoBehaviour
{
    public Image imagenUI;
    public Sprite stars_0;
    public Sprite stars_1;
    public Sprite stars_2;
    public Sprite stars_3;
    public float tiempoCambio = 5f;

    private float cronometro = 0f;
    private bool yaCambiada = false;

    void Update()
    {
        cronometro += Time.deltaTime;

        if (!yaCambiada && cronometro >= tiempoCambio)
        {
            imagenUI.sprite = stars_0;
            yaCambiada = true;
        }
    }
}