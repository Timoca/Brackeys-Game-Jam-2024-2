using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float wobbleIntensity = 0.1f; // Hoe sterk de wiebel is
    public float wobbleSpeed = 1.0f;     // Hoe snel de wiebel is

    private Vector3 originalPosition; // De oorspronkelijke positie van de camera
    private Quaternion originalRotation; // De oorspronkelijke rotatie van de camera
    private float wobbleTime = 0;
    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        wobbleTime += Time.deltaTime * wobbleSpeed;

        float wobbleZPos = Mathf.Sin(wobbleTime) * wobbleIntensity;
        float wobbleZRot = Mathf.Sin(wobbleTime * 1.5f) * wobbleIntensity;

        // Pas de Z-positie van de camera aan (alleen Z-as)
        transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z + wobbleZPos);

        // Pas de Z-rotatie van de camera aan
        transform.localRotation = originalRotation * Quaternion.Euler(0, 0, wobbleZRot);
    }
}
