using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public EnemyManager enemyManager;
    public GameObject panelUI;
    private bool hasActivated = false;
    public float scaleDuration = 0.5f;

    void Start()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(false);
            Cursor.visible = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!hasActivated && other.CompareTag("Player") && enemyManager && enemyManager.AreAllEnemiesDestroyed())
        {
            hasActivated = true;

            crono cronometro = FindObjectOfType<crono>();
            if (cronometro != null)
            {
                cronometro.StopCrono();
            }

            if (panelUI != null)
            {
                panelUI.SetActive(true); // Activamos primero para que se muestre
                panelUI.transform.localScale = Vector3.zero; // Empezamos desde cero
                Cursor.visible = true;
                StartCoroutine(ScaleInPanel(panelUI.transform));
            }
        }
    }

    private IEnumerator ScaleInPanel(Transform panelTransform)
    {
        float elapsed = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / scaleDuration);
            panelTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        panelTransform.localScale = endScale;
    }
}