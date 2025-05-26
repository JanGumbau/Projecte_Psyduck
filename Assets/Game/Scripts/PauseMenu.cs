using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public float scaleDuration = 0.5f;

    public static bool GameIsPaused = false;

    void Start()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            pauseMenu.transform.localScale = Vector3.one;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                StartCoroutine(ScaleOutAndResume(pauseMenu.transform));
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = true;
    }

    void Pause()
    {
        GameIsPaused = true;
        Cursor.visible = true;
        Time.timeScale = 0f;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            pauseMenu.transform.localScale = Vector3.zero;
            StartCoroutine(ScaleInPanel(pauseMenu.transform));
        }
    }

    private IEnumerator ScaleInPanel(Transform panelTransform)
    {
        float elapsed = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (elapsed < scaleDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / scaleDuration);
            panelTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        panelTransform.localScale = endScale;
    }

    private IEnumerator ScaleOutAndResume(Transform panelTransform)
    {
        float elapsed = 0f;
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.zero;

        while (elapsed < scaleDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / scaleDuration);
            panelTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        panelTransform.localScale = endScale;

        Resume();
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame() called");
        Application.Quit();
    }

    public void RestartGame()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("MenuScene");
    }
}
