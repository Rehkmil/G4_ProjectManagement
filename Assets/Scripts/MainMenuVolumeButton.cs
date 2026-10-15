
using UnityEngine;
using UnityEngine.UI;

public class MainMenuVolumeButton : MonoBehaviour
{
    [Header("Volume Button")]
    public Image buttonIcon;
    public Sprite volumeOnSprite;
    public Sprite volumeOffSprite;

    private bool isMuted = false;

    private void Start()
    {
        // Start with sound enabled.
        AudioListener.pause = false;
        UpdateIcon();
    }

    public void ToggleVolume()
    {
        isMuted = !isMuted;

        // Mute or unmute all normal game audio.
        AudioListener.pause = isMuted;

        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (buttonIcon != null)
        {
            buttonIcon.sprite = isMuted
                ? volumeOffSprite
                : volumeOnSprite;
        }
    }
}
