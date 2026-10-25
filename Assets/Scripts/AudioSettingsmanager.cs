using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-1)]
public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance { get; private set; }

    // This forces Unity to clear the static variable every time you enter Play Mode 
    // when Domain Reload is disabled.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        Instance = null;
    }

    [SerializeField] private AudioMixer audioMixer;

    private const string MusicKey = "MusicEnabled";
    private const string SFXKey = "SFXEnabled";
    private const string MusicParam = "MusicVolume";
    private const string SFXParam = "SFXVolume";

    public bool MusicEnabled { get; private set; }
    public bool SFXEnabled { get; private set; }

    public event Action Changed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MusicEnabled = PlayerPrefs.GetInt(MusicKey, 1) == 1;
        SFXEnabled = PlayerPrefs.GetInt(SFXKey, 1) == 1;
    }

    private IEnumerator Start()
    {
        yield return null; 
        ApplySettings();
    }

    public void SetMusicEnabled(bool value)
    {
        MusicEnabled = value;
        PlayerPrefs.SetInt(MusicKey, value ? 1 : 0);
        ApplySettings();
    }

    public void SetSFXEnabled(bool value)
    {
        SFXEnabled = value;
        PlayerPrefs.SetInt(SFXKey, value ? 1 : 0);
        ApplySettings();
    }

    private void ApplySettings()
    {
        // Trigger the UI to update immediately, even if the mixer is missing.
        Changed?.Invoke();

        if (audioMixer == null)
        {
            Debug.LogWarning("AudioMixer is not assigned in the AudioSettingsManager! Audio won't change.");
            return;
        }

        if (!audioMixer.SetFloat(MusicParam, MusicEnabled ? 0f : -80f))
            Debug.LogWarning($"Exposed parameter '{MusicParam}' not found on mixer.");
        
        if (!audioMixer.SetFloat(SFXParam, SFXEnabled ? 0f : -80f))
            Debug.LogWarning($"Exposed parameter '{SFXParam}' not found on mixer.");
    }
}