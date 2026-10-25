using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Image musicButtonImage;
    [SerializeField] private Sprite musicOnSprite, musicOffSprite;
    [SerializeField] private Image sfxButtonImage;
    [SerializeField] private Sprite sfxOnSprite, sfxOffSprite;

    private void Start()
    {
        var mgr = AudioSettingsManager.Instance;
        if (mgr != null)
        {
            mgr.Changed += Refresh;
            Refresh();
        }
    }

    private void OnDestroy()
    {
        if (AudioSettingsManager.Instance != null)
        {
            AudioSettingsManager.Instance.Changed -= Refresh;
        }
    }

    public void ToggleMusic() =>
        AudioSettingsManager.Instance.SetMusicEnabled(!AudioSettingsManager.Instance.MusicEnabled);

    public void ToggleSFX() =>
        AudioSettingsManager.Instance.SetSFXEnabled(!AudioSettingsManager.Instance.SFXEnabled);

    private void Refresh()
    {
        var mgr = AudioSettingsManager.Instance;
        musicButtonImage.sprite = mgr.MusicEnabled ? musicOnSprite : musicOffSprite;
        sfxButtonImage.sprite = mgr.SFXEnabled ? sfxOnSprite : sfxOffSprite;
    }
}