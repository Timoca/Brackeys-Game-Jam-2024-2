using UnityEngine;

public class WheelbarrowAudio : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayCollectingSound()
    {
        _audioSource.PlayOneShot(collectSound);
    }
}
