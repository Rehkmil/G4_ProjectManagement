using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class WrongDropZone : MonoBehaviour
{
    [Header("Punishment")]
    public int penalty = 1;
    public AudioClip wrongSound;
    [Range(0f, 1f)]
    public float wrongSoundVolume = 1f;
    [Tooltip("Optional. Leave empty to use the SFX group from the GameManager.")]
    public AudioMixerGroup sfxGroup;

    [Header("Wrong Indicator")]
    public GameObject wrongIndicatorPrefab;
    public float indicatorDuration = 2f;
    public Vector3 indicatorOffset = new Vector3(0f, 1f, 0f);

    [Header("Detection")]
    public string itemTag = "Item";

    // Crates already punished while inside this zone
    private HashSet<GameObject> punished = new HashSet<GameObject>();

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(itemTag)) return;
        if (punished.Contains(other.gameObject)) return;

        // Still being carried by the claw (parented to it), so ignore
        if (other.transform.parent != null) return;

        if (GameManager.instance == null || !GameManager.instance.isGameActive) return;

        punished.Add(other.gameObject);

        PlayWrongSound();

        GameManager.instance.AddScore(-penalty);

        // Show the "wrong" indicator where the crate was
        if (wrongIndicatorPrefab != null)
        {
            GameObject indicator = Instantiate(wrongIndicatorPrefab, other.transform.position + indicatorOffset, Quaternion.identity);
            Destroy(indicator, indicatorDuration);
        }

        // Remove the crate from the GameManager lists, then make it disappear
        GameManager.instance.activeItems.Remove(other.gameObject);
        GameManager.instance.correctlyGrabbedItems.Remove(other.gameObject);
        Destroy(other.gameObject);
    }

    private void PlayWrongSound()
    {
        if (wrongSound == null) return;

        // Use this script's group if set, otherwise the GameManager's SFX group
        AudioMixerGroup group = sfxGroup != null ? sfxGroup : GameManager.instance.sfxGroup;

        // Temporary audio source routed through the mixer so SFX mute/volume applies
        GameObject soundObj = new GameObject("WrongSound");
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = wrongSound;
        source.volume = wrongSoundVolume;
        source.spatialBlend = 0f;
        source.outputAudioMixerGroup = group;
        source.Play();

        Destroy(soundObj, wrongSound.length + 0.1f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        punished.Remove(other.gameObject);
    }
}