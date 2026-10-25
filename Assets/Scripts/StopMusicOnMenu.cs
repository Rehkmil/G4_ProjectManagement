using UnityEngine;
using UnityEngine.SceneManagement;

public class StopMusicOnMenu : MonoBehaviour
{
    public string menuSceneName = "MainMenu";
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (source == null) return;

        if (scene.name == menuSceneName)
            source.Stop();
        else if (!source.isPlaying)
            source.Play();
    }
}