using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioSource audioSource; // Inspector'dan atay�n
    public float fadeDuration = 1f; // Fade in/out s�resi

    void Awake()
    {
        // Singleton kontrol�
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // E�er sahnede sessiz veya duruyorsa m�zi�i ba�lat
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Yeni m�zik �al
    public void PlayMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip) return; // Ayn� m�zikse de�i�tirme
        StartCoroutine(FadeMusic(newClip));
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        // Fade out mevcut m�zik
        if (audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }
            audioSource.volume = 0;
            audioSource.Stop();
        }

        // Yeni m�zik set et ve �al
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 1;
    }

    public void SetVolume(float value)
    {
        // Scrollbar 0-100 aras� �al���yorsa
        // audioSource.volume = value / 100f;

        // Scrollbar 0-1 aras� �al���yorsa (default Unity davran���)
        audioSource.volume = value;
    }

}
