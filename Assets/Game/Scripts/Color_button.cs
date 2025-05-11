using UnityEngine;
using UnityEngine.UI;

public class Color_button : MonoBehaviour
{
    private Button button;
    private Color originalColor;
    private Color pressedColor = Color.black;
    private bool isOriginalColor = true;

    void Start()
    {
        button = GetComponent<Button>();
        originalColor = button.image.color; 

        button.onClick.AddListener(ToggleColor);
    }

    void ToggleColor()
    {
        if (isOriginalColor)
        {
            button.image.color = pressedColor;
        }
        else
        {
            button.image.color = originalColor;
        }

        isOriginalColor = !isOriginalColor;
    }
}