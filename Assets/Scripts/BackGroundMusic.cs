using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGroundMusic : MonoBehaviour
{
    private static BackGroundMusic Instance;
    private AudioSource audioSource;

    public AudioClip menuMusic;
    public AudioClip gameMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        AudioClip targetClip;

        if (SceneManager.GetActiveScene().name == "MainMenu" || GameManager.instance.GameIsPaused)
            targetClip = menuMusic;
        else
            targetClip = gameMusic;

        if (audioSource.clip != targetClip)
        {
            audioSource.clip = targetClip;
            audioSource.Play();
        }
    }
}
