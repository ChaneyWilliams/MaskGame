using UnityEngine;
using System.Collections;

public class AudioFadeIn : MonoBehaviour
{
    public AudioSource audioGameMusic;
    float startVolume = 1;
    bool fadedIn = false;
    void Update()
    {
        if(!fadedIn)
        {
            StartCoroutine(FadeIn(audioGameMusic, 5f));
            fadedIn = true;
        }
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        

        while (audioSource.volume < 0.8f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.volume = startVolume;
    }
    //void Start()
    //{
    //    
    //}
}