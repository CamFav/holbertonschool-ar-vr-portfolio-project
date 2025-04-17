using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the overall game logic, level initialization, and progression.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    public GridManager gridManager;
    public PuzzlePieceGenerator pieceGenerator;

    [Header("Player")]
    public Transform playerSpawn; // Spawn point for the player.

    [Header("Puzzle")]
    private string currentLevel; // Current level identifier.
    public int rows = 3;         // Default number of rows.
    public int cols = 3;         // Default number of columns.
    public Texture2D defaultPuzzleImage; // Default puzzle texture.

    [Header("UI")]
    public Canvas levelCompleteCanvas; // Canvas displayed upon level completion.
    public GameObject returnToMenuButton; // Button for returning to the main menu.

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initializes the current level if not in the main menu.
    /// </summary>
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            currentLevel = PlayerPrefs.GetString("CurrentLevel", "Level1");
            InitializeLevel(currentLevel);
        }
    }

    /// <summary>
    /// Initializes the level with the specified parameters.
    /// </summary>
    /// <param name="selectedLevel">The name of the level to initialize.</param>
    public void InitializeLevel(string selectedLevel)
    {
        Debug.Log($"Initializing level: {selectedLevel}");

        Texture2D puzzleImage = LoadPuzzleImage(selectedLevel) ?? defaultPuzzleImage;
        DefineGridDimensions(selectedLevel);

        Debug.Log($"Grid dimensions: {rows}x{cols}");
        gridManager.rows = rows;
        gridManager.cols = cols;
        pieceGenerator.rows = rows;
        pieceGenerator.cols = cols;

        gridManager.GenerateGrid();
        pieceGenerator.GeneratePiecesInScrollView(puzzleImage);
    }

    /// <summary>
    /// Defines grid dimensions based on the level name.
    /// </summary>
    /// <param name="levelName">The name of the level.</param>
    private void DefineGridDimensions(string levelName)
    {
        switch (levelName)
        {
            case "Puzzle1":
                rows = 6;
                cols = 12;
                break;
            case "Puzzle2":
                rows = 3;
                cols = 3;
                break;
            case "Puzzle3":
                rows = 5;
                cols = 5;
                break;
            default:
                rows = 5;
                cols = 5;
                break;
        }

        Debug.Log($"Grid dimensions for {levelName}: {rows}x{cols}");
    }

    /// <summary>
    /// Loads the puzzle image for the given level.
    /// </summary>
    /// <param name="levelName">The name of the level.</param>
    /// <returns>The loaded texture, or null if not found.</returns>
    private Texture2D LoadPuzzleImage(string levelName)
    {
        string path = $"PuzzleSprites/{levelName}";
        return Resources.Load<Texture2D>(path);
    }

    /// <summary>
    /// Checks if the puzzle is complete and triggers completion logic.
    /// </summary>
    public void CheckPuzzleCompletion()
    {
        if (gridManager.AreAllSlotsOccupied())
        {
            Debug.Log("Puzzle complete");
            OnPuzzleCompleted();
        }
        else
        {
            Debug.Log("Puzzle incomplete.");
        }
    }

    /// <summary>
    /// Handles actions when the puzzle is completed.
    /// </summary>
    private void OnPuzzleCompleted()
    {
        Debug.Log("Puzzle completed");

        LevelTimer levelTimer = FindObjectOfType<LevelTimer>();
        if (levelTimer != null)
        {
            levelTimer.StopTimer();
        }

        SaveStars();
        PlayerPrefs.SetInt($"{currentLevel}Completed", 1);
        UnlockNextLevel();
        PlayerPrefs.Save();
        Debug.Log("Progress saved");
    }

    /// <summary>
    /// Returns to the main menu.
    /// </summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Spawn");
    }

    /// <summary>
    /// Unlocks the next level if available.
    /// </summary>
    private void UnlockNextLevel()
    {
        string[] levels = PlayerPrefsArrayUtils.GetStringArray("LevelNames");
        if (levels == null || levels.Length == 0)
        {
            Debug.LogError("Level list not found in PlayerPrefs");
            return;
        }

        int currentLevelIndex = System.Array.IndexOf(levels, currentLevel);
        if (currentLevelIndex == -1)
        {
            Debug.LogError($"Current level '{currentLevel}' not found in the list.");
            return;
        }

        if (currentLevelIndex < levels.Length - 1)
        {
            string nextLevel = levels[currentLevelIndex + 1];
            PlayerPrefs.SetInt($"{nextLevel}Unlocked", 1);
            PlayerPrefs.Save();
            Debug.Log($"Next level unlocked: {nextLevel}");
        }
    }

    /// <summary>
    /// Saves the stars earned for the current level.
    /// </summary>
    private void SaveStars()
    {
        LevelTimer levelTimer = FindObjectOfType<LevelTimer>();

        if (levelTimer != null)
        {
            int starsEarned = levelTimer.GetStarsEarned();
            PlayerPrefs.SetInt($"{currentLevel}_Stars", starsEarned);
            Debug.Log($"Stars saved for {currentLevel}: {starsEarned}");
        }
        else
        {
            Debug.LogWarning("LevelTimer not found");
        }
    }
}
