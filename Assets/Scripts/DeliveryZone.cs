
using UnityEngine;
using System.Collections;

public class DeliveryZone : MonoBehaviour
{
    [Header("Indicators")]
    public GameObject checkPrefab;
    public GameObject xPrefab;

    [Header("Score Sounds")]
    public AudioClip correctScoreSound;
    public AudioClip wrongScoreSound;
    [Range(0f, 1f)]
    public float soundVolume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            bool isCorrectSequence =
                GameManager.instance.ValidateItem(collision.gameObject);

            if (isCorrectSequence)
            {
                GameManager.instance.AddScore(1);
                ShowIndicator(checkPrefab, collision.transform.position);
                PlayScoreSound(correctScoreSound);
            }
            else
            {
                ShowIndicator(xPrefab, collision.transform.position);
                PlayScoreSound(wrongScoreSound);
            }

            StartCoroutine(LingerAndDestroy(collision.gameObject));
        }
    }

    private void PlayScoreSound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, soundVolume);
        }
    }

    private void ShowIndicator(GameObject indicatorPrefab, Vector3 spawnPos)
    {
        if (indicatorPrefab == null) return;

        Vector3 indicatorPosition =
            spawnPos + new Vector3(0, 1f, 0);

        GameObject indicator = Instantiate(
            indicatorPrefab,
            indicatorPosition,
            Quaternion.identity
        );

        Destroy(indicator, 1f);
    }

    private IEnumerator LingerAndDestroy(GameObject item)
    {
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.simulated = false;
        }

        yield return new WaitForSeconds(2f);

        Destroy(item);
    }
}
