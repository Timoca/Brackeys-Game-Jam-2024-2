using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeatherSystem : MonoBehaviour
{
    [SerializeField] float _minBackgroundDimmerAlpha = 0.5f;
    [SerializeField] float _maxBackgroundDimmerAlpha = 1f;
    [SerializeField] ParticleSystem _windParticlesBack;
    [SerializeField] ParticleSystem _windParticlesFront;
    [SerializeField] RawImage _backgroundDimmer;
    [SerializeField] Light _pointLight;
    [SerializeField] Light _lightningLight;
    private GameTimer _gameTimer;
    private CameraMovement _cameraMovement;
    private CollectibleSpawner _collectibleSpawner;
    private int _faseLength;
    private float _elapsedTime = 0f;
    void Start()
    {
        _gameTimer = FindAnyObjectByType<GameTimer>();
        _cameraMovement = FindAnyObjectByType<CameraMovement>();
        _collectibleSpawner = GetComponent<CollectibleSpawner>();

        _faseLength = _gameTimer.lengthOfGame / 3;
        StartCoroutine(StartStormyWeather());
    }

    void Update()
    {
        IncreaseDarkness();

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
            yield return new WaitForSeconds(_faseLength);
        }
    }

    private void IncreaseStorm(int fase)
    {
        // Increase storm intensity
        switch (fase)
        {
            case 0:
                Debug.Log("Fase 1: Light rain");
                break;
            case 1:
                Debug.Log("Fase 2: Heavy rain");
                StartCoroutine(SmoothWobbleTransition(2f, 2f));
                StartCoroutine(IncreaseWindOverTime(10));
                StartCoroutine(IncreaseDropRate(0.5f, 2f));
                StartCoroutine(LightningEffect(1f));
                break;
            case 2:
                Debug.Log("Fase 3: Thunderstorm");
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

        // Huidige waarden opslaan
        float startIntensity = _cameraMovement.wobbleIntensity;
        float startSpeed = _cameraMovement.wobbleSpeed;

        float elapsedTime = 0;

        // Overgang uitvoeren
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            // Lineair interpoleren tussen de huidige en doelwaarden
            _cameraMovement.wobbleIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / transitionDuration);
            _cameraMovement.wobbleSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / transitionDuration);

            yield return null; // Wacht op de volgende frame
        }

        // Zorg dat we exact de doelwaarden instellen aan het einde van de overgang
        _cameraMovement.wobbleIntensity = targetIntensity;
        _cameraMovement.wobbleSpeed = targetSpeed;
    }


    private IEnumerator IncreaseWindOverTime(int targetSpeed)
    {
        float duration = _gameTimer.lengthOfGame / 3;

        // Huidige windsnelheid opslaan
        float startSpeedFront = _windParticlesFront.main.simulationSpeed;
        float startSpeedBack = _windParticlesBack.main.simulationSpeed;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / duration;  // Tussen 0 en 1 gedurende de fase

            // Interpoleer de wind snelheid
            var mainBack = _windParticlesBack.main;
            mainBack.simulationSpeed = Mathf.Lerp(startSpeedBack, targetSpeed, lerpFactor);

            var mainFront = _windParticlesFront.main;
            mainFront.simulationSpeed = Mathf.Lerp(startSpeedFront, targetSpeed, lerpFactor);

            yield return null;  // Wacht op de volgende frame
        }

        // Zorg dat de snelheid precies op de doelwaarde eindigt
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

        // Zorg dat het licht uit staat voordat de coroutine start
        _lightningLight.enabled = false;

        while (elapsedTime < duration)
        {
            // Wacht een willekeurige tijd voordat de volgende flits plaatsvindt
            float waitTime = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(waitTime);

            // Simuleer de bliksemflits door het licht kort in te schakelen
            float flashDuration = Random.Range(0.1f, 0.3f); // Duur van de flits

            _lightningLight.enabled = true;  // Zet het licht aan

            yield return new WaitForSeconds(flashDuration);  // Wacht de duur van de flits

            _lightningLight.enabled = false;  // Zet het licht weer uit

            elapsedTime += waitTime + flashDuration;
        }

        // Zorg dat het licht uit staat als de coroutine is afgelopen
        _lightningLight.enabled = false;
    }

}
