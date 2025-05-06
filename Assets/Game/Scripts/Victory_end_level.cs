using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Victory_end_level : MonoBehaviour
{
    [SerializeField] private GameObject Menuwin;
    private Portal portal;

    private void Start()
    {
        portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<Portal>();
        portal.TocaJugadorPortal += ActivarMenu;
        Menuwin.SetActive(false);
    }

    private void ActivarMenu(object sender, EventArgs e)
    {
        Menuwin.SetActive(true);
    }

    private void OnDestroy()
    {
        // Limpia la suscripción para evitar errores si se destruye el objeto
        if (portal != null)
            portal.TocaJugadorPortal -= ActivarMenu;
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuInicial(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void SiguienteScene()
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual + 1);
    }
}