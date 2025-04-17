using UnityEngine;

/// <summary>
/// Manages the initialization and configuration of each game level.
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Spawn point for the player at the beginning of the level.
    /// </summary>
    public Transform playerSpawnPoint;

    /// <summary>
    /// Reference to the GridManager for generating the puzzle grid.
    /// </summary>
    public GridManager gridManager;

    /// <summary>
    /// Reference to the PuzzlePieceGenerator for generating puzzle pieces.
    /// </summary>
    public PuzzlePieceGenerator pieceGenerator;

    /// <summary>
    /// Reference to the SphereManager for updating the puzzle sphere's texture.
    /// </summary>
    public SphereManager sphereManager;

    /// <summary>
    /// Called when the level starts. Positions the player and initializes the level.
    /// </summary>
    private void Start()
    {
        PositionPlayer();
        InitializeLevel();
    }

    /// <summary>
    /// Places the player at the defined spawn point.
    /// </summary>
    private void PositionPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;
            player.transform.rotation = playerSpawnPoint.rotation;
            Debug.Log("Player positioned at the spawn point.");
        }
        else
        {
            Debug.LogError("Player or spawn point is missing!");
        }
    }

    /// <summary>
    /// Initializes the current level by generating the grid, puzzle pieces, and sphere texture.
    /// </summary>
    private void InitializeLevel()
    {
        string currentLevel = PlayerPrefs.GetString("CurrentLevel", "Puzzle1");
        Debug.Log($"Initializing level: {currentLevel}");

        // Load the puzzle image for the current level.
        Texture2D puzzleImage = LoadPuzzleImage(currentLevel);
        if (puzzleImage == null)
        {
            Debug.LogError($"Puzzle image not found for level: {currentLevel}");
            return;
        }

        // Generate the grid and puzzle pieces.
        gridManager.GenerateGrid();
        pieceGenerator.GeneratePiecesInScrollView(puzzleImage);

        // Update the sphere texture for the level.
        if (sphereManager != null)
        {
            sphereManager.UpdateSphereTexture(currentLevel);
        }
        else
        {
            Debug.LogError("SphereManager is not assigned in LevelManager.");
        }

        Debug.Log($"Grid, puzzle pieces, and sphere texture generated for level: {currentLevel}.");
    }

    /// <summary>
    /// Loads the puzzle image for the specified level.
    /// </summary>
    /// <param name="levelName">The name of the level.</param>
    /// <returns>The loaded texture, or null if not found.</returns>
    private Texture2D LoadPuzzleImage(string levelName)
    {
        string path = $"PuzzleSprites/{levelName}";
        return Resources.Load<Texture2D>(path);
    }
}
