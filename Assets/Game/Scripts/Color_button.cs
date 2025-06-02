using UnityEngine;
using UnityEngine.UI;

public class SpriteButtonToggle : MonoBehaviour
{
    private Button button;
    private bool isOriginalSprite = true;

    public Sprite originalSprite;   // Imagen original
    public Sprite pressedSprite;    // Imagen al presionar

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleSprite);
    }

    void ToggleSprite()
    {
        if (isOriginalSprite)
        {
            button.image.sprite = pressedSprite;
        }
        else
        {
            button.image.sprite = originalSprite;
        }

        isOriginalSprite = !isOriginalSprite;
    }
}
