using UnityEngine;
using UnityEngine.UI;

public class efecto_button : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound; 
    [SerializeField] private AudioSource audioSource; 
    [SerializeField][Range(0f, 1f)] private float volume = 1f; 

    private Button button;

    private void Awake()
    {
        
        button = GetComponent<Button>();

       
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

           
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    private void OnEnable()
    {
       
        button.onClick.AddListener(PlayClickSound);
    }

    private void OnDisable()
    {
        
        button.onClick.RemoveListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
       
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound, volume);
        }
        else
        {
            Debug.LogWarning("Falta asignar AudioClip o AudioSource en el Inspector", this);
        }
    }
}
