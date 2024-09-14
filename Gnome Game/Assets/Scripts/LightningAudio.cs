using UnityEngine;

public class LightningAudio : MonoBehaviour
{
    [SerializeField] private AudioClip lightningSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLightningSound()
    {
        audioSource.PlayOneShot(lightningSound);
    }
}
