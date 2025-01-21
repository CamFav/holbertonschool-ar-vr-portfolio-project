using UnityEngine;

public class SphereRotator : MonoBehaviour
{
    /// <summary>
    /// Rotation speed in degrees per second.
    /// </summary>
    public float rotationSpeed = 10f;

    private void Update()
    {
        // Rotate the sphere around its Y-axis.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
