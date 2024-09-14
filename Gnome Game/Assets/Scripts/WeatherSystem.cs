using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeatherSystem : MonoBehaviour
{
    [SerializeField] float _minBackgroundDimmerAlpha = 0.5f;
    [SerializeField] float _maxBackgroundDimmerAlpha = 1f;
    [SerializeField] ParticleSystem _windParticlesBack;
    [SerializeField] ParticleSystem _windParticlesFront;
    [SerializeField] GameObject _rainParticlesParent;
    [SerializeField] RawImage _backgroundDimmer;
    [SerializeField] Light _pointLight;
    [SerializeField] Light _lightningLight;
    private GameTimer _gameTimer;
    private CameraMovement _cameraMovement;
    private CollectibleSpawner _collectibleSpawner;
    private LighningAudio _lightningAudio;
    private RainAudio _rainAudio;
    private int _phaseLength;
    private bool _lightningStarted = false;
    private float _elapsedTime = 0f;
    void Start()
    {
        _gameTimer = FindAnyObjectByType<GameTimer>();
        _cameraMovement = FindAnyObjectByType<CameraMovement>();
        _collectibleSpawner = GetComponent<CollectibleSpawner>();
        _lightningAudio = FindAnyObjectByType<LighningAudio>();
        _rainAudio = FindAnyObjectByType<RainAudio>();

        _phaseLength = _gameTimer.lengthOfGame / 3;
        StartCoroutine(StartStormyWeather());
    }

    void Update()
    {
        IncreaseDarkness();

        if (_gameTimer.gameEnded && !_lightningStarted)
        {
            StartCoroutine(LightningEffect(3f));
            MakeItRain();
            _lightningStarted = true;
        }
    }

    private void MakeItRain()
    {
        _rainAudio.PlayRainSound();
        _rainParticlesParent.SetActive(true);
    }

    private void IncreaseDarkness()
    {
        // Increase the elapsed time
        _elapsedTime += Time.deltaTime;

        // Calculate the interpolation factor (should be between 0 and 1)
        float lerpFactor = _elapsedTime / _gameTimer.lengthOfGame;

        // Make sure the interpolation factor is not greater than 1
        lerpFactor = Mathf.Clamp01(lerpFactor);

        // Get the current color
        Color color = _backgroundDimmer.color;

        // Adjust the alpha with the correct lerp factor
        color.a = Mathf.Lerp(_minBackgroundDimmerAlpha, _maxBackgroundDimmerAlpha, lerpFactor);

        // Adjust the intensity of the light
        _pointLight.intensity = Mathf.Lerp(5000f, 2000f, lerpFactor);

        // Set the modified color back
        _backgroundDimmer.color = color;
    }

    private IEnumerator StartStormyWeather()
    {
        for (int i = 0; i < 3; i++)
        {
            IncreaseStorm(i);
            yield return new WaitForSeconds(_phaseLength);
        }
    }

    private void IncreaseStorm(int phase)
    {
        // Increase storm intensity
        switch (phase)
        {
            case 0:
                Debug.Log("Phase 1: Light rain");
                break;
            case 1:
                Debug.Log("Phase 2: Heavy rain");
                StartCoroutine(SmoothWobbleTransition(2f, 2f));
                StartCoroutine(IncreaseWindOverTime(10));
                StartCoroutine(IncreaseDropRate(0.5f, 2f));
                StartCoroutine(LightningEffect(1f));
                break;
            case 2:
                Debug.Log("Phase 3: Thunderstorm");
                StartCoroutine(SmoothWobbleTransition(3f, 2f));
                StartCoroutine(IncreaseWindOverTime(20));
                StartCoroutine(IncreaseDropRate(0.1f, 2f));
                StartCoroutine(LightningEffect(2f));
                break;
        }
    }

    private IEnumerator SmoothWobbleTransition(float targetIntensity, float targetSpeed)
    {
        float transitionDuration = 5.0f;

        // Save current values
        float startIntensity = _cameraMovement.wobbleIntensity;
        float startSpeed = _cameraMovement.wobbleSpeed;

        float elapsedTime = 0;

        // Perform transition
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            // Linearly interpolate between current and target values
            _cameraMovement.wobbleIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / transitionDuration);
            _cameraMovement.wobbleSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / transitionDuration);

            yield return null; // Wait for the next frame
        }

        // Set exact target values at the end of the transition
        _cameraMovement.wobbleIntensity = targetIntensity;
        _cameraMovement.wobbleSpeed = targetSpeed;
    }


    private IEnumerator IncreaseWindOverTime(int targetSpeed)
    {
        float duration = _gameTimer.lengthOfGame / 3;

        // Save current wind speed
        float startSpeedFront = _windParticlesFront.main.simulationSpeed;
        float startSpeedBack = _windParticlesBack.main.simulationSpeed;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / duration;  // Between 0 and 1 during the phase

            // Interpolate the wind speed
            var mainBack = _windParticlesBack.main;
            mainBack.simulationSpeed = Mathf.Lerp(startSpeedBack, targetSpeed, lerpFactor);

            var mainFront = _windParticlesFront.main;
            mainFront.simulationSpeed = Mathf.Lerp(startSpeedFront, targetSpeed, lerpFactor);

            yield return null;  // Wait for the next frame
        }

        // Set the speed exactly to the target value
        var finalMainBack = _windParticlesBack.main;
        finalMainBack.simulationSpeed = targetSpeed;

        var finalMainFront = _windParticlesFront.main;
        finalMainFront.simulationSpeed = targetSpeed;
    }

    private IEnumerator IncreaseDropRate(float temporaryDropRate, float duration)
    {
        float currentDropRate = _collectibleSpawner.spawnInterval;
        _collectibleSpawner.spawnInterval = temporaryDropRate;
        yield return new WaitForSeconds(duration);
        _collectibleSpawner.spawnInterval = currentDropRate;
    }

    private IEnumerator LightningEffect(float duration)
    {
        float elapsedTime = 0f;

        // Make sure the light is off before the coroutine starts
        _lightningLight.enabled = false;

        while (elapsedTime < duration)
        {
            // Wait for a random time before the next flash occurs
            float waitTime = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(waitTime);

            // Simulate the lightning flash by briefly turning on the light
            float flashDuration = Random.Range(0.1f, 0.3f); // Duration of the flash

            _lightningLight.enabled = true;  // Turn on the light
            _lightningAudio.PlayLightningSound();  // Play the sound

            yield return new WaitForSeconds(flashDuration);  // Wait for the flash duration

            _lightningLight.enabled = false;  // Turn off the light

            elapsedTime += waitTime + flashDuration;
        }

        // Make sure the light is off when the coroutine is finished
        _lightningLight.enabled = false;
    }
}
