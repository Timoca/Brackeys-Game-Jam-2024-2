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
    private GameTimer _gameTimer;
    private CameraMovement _cameraMovement;
    private int _faseLength;
    private float _elapsedTime = 0f;
    void Start()
    {
        _gameTimer = FindAnyObjectByType<GameTimer>();
        _cameraMovement = FindAnyObjectByType<CameraMovement>();

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
                //StartCoroutine(SmoothWobbleTransition(0.3f, 1.2f));
                IncreaseWind(5);
                break;
            case 2:
                Debug.Log("Fase 3: Thunderstorm");
                //StartCoroutine(SmoothWobbleTransition(0.5f, 1.6f));
                IncreaseWind(10);
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

    private void IncreaseWind(int simulationSpeed)
    {
        var mainBack = _windParticlesBack.main;
        mainBack.simulationSpeed = simulationSpeed;

        var mainFront = _windParticlesFront.main;
        mainFront.simulationSpeed = simulationSpeed;
    }
}
