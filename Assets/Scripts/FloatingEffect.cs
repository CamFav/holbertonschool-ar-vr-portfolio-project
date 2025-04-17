using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [Header("Floating Settings")]
    public float amplitude = 10f; // Vertical movement distance
    public float frequency = 1f;  // Movement speed

    private Vector3 initialPosition;

    /// <summary>
    /// Stores the initial position.
    /// </summary>
    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    /// <summary>
    /// Updates the position to create a floating effect.
    /// </summary>
    private void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude; // Calculate vertical offset
        transform.localPosition = initialPosition + new Vector3(0f, offsetY, 0f); // Apply offset
    }
}
