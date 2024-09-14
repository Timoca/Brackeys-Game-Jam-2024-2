using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioClip[] footstepSounds;
    public float footstepInterval = 0.5f;
    public float minPitch = 0.8f; // Minimum pitch
    public float maxPitch = 1.2f; // Maximum pitch

    private AudioSource _audioSource;

    private PlayerMovement _playerMovement;

    private Rigidbody rb;
    private float timeSinceLastStep = 0f;
    private int currentFootstepIndex = 0;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
        rb = _playerMovement.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player is moving and play footstep sounds
        if (rb.linearVelocity.magnitude > 0.1)
        {
            timeSinceLastStep += Time.deltaTime;

            // If the time since the last footstep is greater than the interval, play a sound
            if (timeSinceLastStep >= footstepInterval)
            {
                if (_playerMovement._isGrounded)
                {
                    PlayFootstep();
                    timeSinceLastStep = 0f; // Reset the timer
                }
            }
        }
    }
    private void PlayFootstep()
    {
        float currentVolume = _audioSource.volume;
        _audioSource.pitch = Random.Range(minPitch, maxPitch);
        _audioSource.volume = 0.1f;

        _audioSource.PlayOneShot(footstepSounds[currentFootstepIndex]);

        currentFootstepIndex = (currentFootstepIndex + 1) % footstepSounds.Length;
        _audioSource.volume = currentVolume;
    }
}
