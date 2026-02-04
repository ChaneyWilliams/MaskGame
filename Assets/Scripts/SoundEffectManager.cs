using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;
    static AudioSource audioSource;
    static SoundEffectLibrary library;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            library = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
    public static void Play(string audioName)
    {
        AudioClip audioClip = library.GetRandomClip(audioName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        //else { Debug.Log("null"); }
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
