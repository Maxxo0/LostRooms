using UnityEngine;

public class HeadBob : MonoBehaviour
{

    [Header("Shake Settings")]
    [Tooltip("Max positional offset for shake.")]
    public float positionAmplitude = 0.05f;
    [Tooltip("Max rotational offset in degrees.")]
    public float rotationAmplitude = 1.5f;
    [Tooltip("Speed of the shake movement.")]
    public float frequency = 2f;

    [Tooltip("Referencia al Rigidbody del jugador.")]
    public Rigidbody playerRb;

    [Tooltip("�Est� el jugador en el suelo?")]
    public bool isGrounded;

    private Vector3 initialPos;
    private Quaternion initialRot;
    private float seed;

    void Start()
    {
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
        seed = Random.Range(0f, 100f);

        if (playerRb == null)
        {
            Debug.LogWarning("�No se asign� el Rigidbody del jugador al HeadBob!");
        }
    }

    void Update()
    {
        if (playerRb == null) return;

        float speed = playerRb.linearVelocity.magnitude;

        if (isGrounded && speed > 0.1f)
        {
            float t = Time.time * frequency;

            // Positional shake using Perlin Noise
            float x = (Mathf.PerlinNoise(seed, t) - 0.5f) * 2f;
            float y = (Mathf.PerlinNoise(seed + 1f, t) - 0.5f) * 2f;
            Vector3 posOffset = new Vector3(x, y, 0f) * positionAmplitude;

            // Rotational shake
            float rx = (Mathf.PerlinNoise(seed + 2f, t) - 0.5f) * 2f;
            float ry = (Mathf.PerlinNoise(seed + 3f, t) - 0.5f) * 2f;
            float rz = (Mathf.PerlinNoise(seed + 4f, t) - 0.5f) * 2f;
            Vector3 rotOffset = new Vector3(rx, ry, rz) * rotationAmplitude;

            transform.localPosition = initialPos + posOffset;
            transform.localRotation = Quaternion.Euler(initialRot.eulerAngles + rotOffset);
        }
        else
        {
            // Smooth reset when stopped
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPos, Time.deltaTime * frequency);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRot, Time.deltaTime * frequency);
        }
    }
}
