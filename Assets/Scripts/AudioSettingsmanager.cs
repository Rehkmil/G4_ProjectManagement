
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsmanager : MonoBehaviour
{
    public static AudioSettingsmanager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Music Button")]
    public Image musicButtonImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    [Header("SFX Button")]
    public Image sfxButtonImage;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;

    private const string MusicKey = "MusicEnabled";
    private const string SFXKey = "SFXEnabled";

    private bool musicEnabled = true;
    private bool sfxEnabled = true;

    public bool MusicEnabled => musicEnabled;
    public bool SFXEnabled => sfxEnabled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicEnabled = PlayerPrefs.GetInt(MusicKey, 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt(SFXKey, 1) == 1;

        ApplySettings();
    }

    private void Start()
    {
        UpdateButtonImages();
    }

    public void ToggleMusic()
    {
        SetMusicEnabled(!musicEnabled);
    }

    public void ToggleSFX()
    {
        SetSFXEnabled(!sfxEnabled);
    }

    public void SetMusicEnabled(bool enabled)
    {
        musicEnabled = enabled;
        PlayerPrefs.SetInt(MusicKey, enabled ? 1 : 0);

        ApplySettings();
        UpdateButtonImages();
    }

    public void SetSFXEnabled(bool enabled)
    {
        sfxEnabled = enabled;
        PlayerPrefs.SetInt(SFXKey, enabled ? 1 : 0);

        ApplySettings();
        UpdateButtonImages();
    }

    private void UpdateButtonImages()
    {
        if (musicButtonImage != null)
        {
            musicButtonImage.sprite = musicEnabled
                ? musicOnSprite
                : musicOffSprite;
        }

        if (sfxButtonImage != null)
        {
            sfxButtonImage.sprite = sfxEnabled
                ? sfxOnSprite
                : sfxOffSprite;
        }
    }

    private void ApplySettings()
    {
        if (audioMixer == null)
            return;

        audioMixer.SetFloat(
            "MusicVolume",
            musicEnabled ? 0f : -80f
        );

        audioMixer.SetFloat(
            "SFXVolume",
            sfxEnabled ? 0f : -80f
        );
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
