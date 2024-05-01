using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    public void PlayMusic() 
    {
        audioSource.Play();
    }
    public void StopMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
