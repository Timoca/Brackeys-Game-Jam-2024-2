using UnityEngine;

public class LightningAudio : MonoBehaviour
{
    [SerializeField] private AudioClip lightningSound;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayLightningSound()
    {
        _audioSource.PlayOneShot(lightningSound);
    }
}
