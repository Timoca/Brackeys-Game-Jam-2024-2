using UnityEngine;

public class RainAudio : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayRainSound()
    {
        _audioSource.Play();
    }
}
