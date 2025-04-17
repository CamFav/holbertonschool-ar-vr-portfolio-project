using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;         // Main menu panel
    public GameObject levelSelectionPanel;  // Level selection panel
    public GameObject settingsPanel;        // Settings panel
    public SphereManager sphereManager;     // Sphere texture manager

    private void Start()
    {
        InitializeLevels(); // Initialize levels at startup
    }

    /// <summary>
    /// Displays the level selection menu.
    /// </summary>
    public void ShowLevelSelection()
    {
        settingsPanel.SetActive(false);
        levelSelectionPanel.SetActive(true);
    }

    /// <summary>
    /// Displays the settings menu.
    /// </summary>
    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
        levelSelectionPanel.SetActive(false);
    }

    /// <summary>
    /// Loads the selected level.
    /// </summary>
    /// <param name="levelName">The name of the level to load.</param>
    public void LoadLevel(string levelName)
    {
        // Save the selected level
        PlayerPrefs.SetString("CurrentLevel", levelName);
        PlayerPrefs.Save();

        // Load the corresponding scene
        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// Initializes levels and sets default unlock states.
    /// </summary>
    public void InitializeLevels()
    {
        string[] levels = { "Puzzle1", "Puzzle2", "Puzzle3" }; // List of levels
        PlayerPrefsArrayUtils.SetStringArray("LevelNames", levels);

        // Unlock the first level by default
        if (PlayerPrefs.GetInt("Puzzle1Unlocked", 0) == 0)
        {
            PlayerPrefs.SetInt("Puzzle1Unlocked", 1);
        }

        // Lock other levels by default
        for (int i = 1; i < levels.Length; i++)
        {
            if (!PlayerPrefs.HasKey($"{levels[i]}Unlocked"))
            {
                PlayerPrefs.SetInt($"{levels[i]}Unlocked", 0);
            }
        }

        PlayerPrefs.Save();
        // Debug.Log("Levels initialized");
    }

    /// <summary>
    /// Marks a level as completed and unlocks the next one.
    /// </summary>
    /// <param name="levelName">The name of the completed level.</param>
    public void CompleteLevel(string levelName)
    {
        // Debug.Log($"Level completed: {levelName}");

        // Mark the level as completed
        PlayerPrefs.SetInt($"{levelName}Completed", 1);

        // Unlock the next level
        string[] levels = PlayerPrefsArrayUtils.GetStringArray("LevelNames");
        int levelIndex = System.Array.IndexOf(levels, levelName);

        if (levelIndex >= 0 && levelIndex < levels.Length - 1)
        {
            string nextLevel = levels[levelIndex + 1];
            PlayerPrefs.SetInt($"{nextLevel}Unlocked", 1);
        }

        PlayerPrefs.Save();
        Debug.Log("Next level unlocked");
    }
}
