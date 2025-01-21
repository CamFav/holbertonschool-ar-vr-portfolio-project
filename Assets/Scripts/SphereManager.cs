using UnityEngine;

public class SphereManager : MonoBehaviour
{
    public Renderer sphereRenderer; // Renderer for the sphere
    public string sphereTextureFolder = "SphereTextures";

    /// <summary>
    /// Updates the sphere's texture based on the selected puzzle.
    /// </summary>
    /// <param name="puzzleName">Selected puzzle name ("Puzzle1").</param>
    public void UpdateSphereTexture(string puzzleName)
    {
        // Derive the sphere texture name ("Sphere1" for "Puzzle1")
        string sphereTextureName = puzzleName.Replace("Puzzle", "Sphere");

        // Load the texture from the Resources folder
        Texture sphereTexture = Resources.Load<Texture>($"{sphereTextureFolder}/{sphereTextureName}");

        if (sphereTexture != null)
        {
            // Apply the texture to the sphere's renderer
            if (sphereRenderer != null)
            {
                sphereRenderer.material.mainTexture = sphereTexture;
            }
            else
            {
                Debug.LogError("Sphere renderer is not assigned");
            }
        }
    }
}
