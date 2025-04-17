using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// Reference to the player's XR Origin object.
    /// </summary>
    public GameObject playerXR;

    private void Start()
    {
        // Find the PlayerSpawn object in the scene.
        GameObject playerSpawn = GameObject.Find("PlayerSpawn");

        if (playerSpawn == null)
        {
            Debug.LogError("PlayerSpawn not found in the scene");
            return;
        }

        if (playerXR == null)
        {
            Debug.LogError("No XR Origin assigned to PlayerSpawner");
            return;
        }

        // Position the XR Origin at the PlayerSpawn location.
        playerXR.transform.position = playerSpawn.transform.position;
        playerXR.transform.rotation = playerSpawn.transform.rotation;

        Debug.Log("XR Origin positioned at PlayerSpawn");
    }
}
