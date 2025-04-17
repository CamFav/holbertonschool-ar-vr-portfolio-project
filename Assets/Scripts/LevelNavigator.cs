using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles level navigation, display, and interactions in the menu.
/// </summary>
public class LevelNavigator : MonoBehaviour
{
    [Header("UI Elements")]
    public Image currentLevelDisplay;       // Image displaying the current level.
    public Button currentLevelButton;      // Button to select the current level.
    public Button leftArrowButton;         // Button to navigate to the previous level.
    public Button rightArrowButton;        // Button to navigate to the next level.
    public TextMeshProUGUI levelTitleText; // Text displaying the level title.

    [Header("Star Elements")]
    public Image[] starImages;             // Array of images representing the star ratings.
    public Sprite filledStar;              // Sprite for filled stars.
    public Sprite emptyStar;               // Sprite for empty stars.

    [Header("Level Data")]
    public string[] levelNames;            // Names of the levels ("Puzzle1", "Puzzle2").
    private int currentLevelIndex = 0;     // Index of the currently displayed level.

    [Header("Paths")]
    public string spriteFolder = "PuzzleSprites";  // Path to load level sprites.

    public MenuManager menuManager;        // Reference to the MenuManager for loading levels.

    /// <summary>
    /// Initializes the level navigator and sets up the UI.
    /// </summary>
    private void Start()
    {
        if (levelNames == null || levelNames.Length == 0)
        {
            levelNames = new string[] { "Puzzle1", "Puzzle2", "Puzzle3" }; // Default level names.
            Debug.Log("Level names initialized in LevelNavigator.");
        }

        PlayerPrefsArrayUtils.SetStringArray("LevelNames", levelNames);

        if (PlayerPrefs.GetInt($"{levelNames[0]}Unlocked", 0) == 0)
        {
            PlayerPrefs.SetInt($"{levelNames[0]}Unlocked", 1);
        }

        UpdateLevelUnlocks();
        PlayerPrefs.Save();

        foreach (string levelName in levelNames)
        {
            Debug.Log($"Level: {levelName}, Unlocked: {PlayerPrefs.GetInt($"{levelName}Unlocked", 0)}, Completed: {PlayerPrefs.GetInt($"{levelName}Completed", 0)}");
        }

        UpdateLevelDisplay();

        leftArrowButton.onClick.RemoveAllListeners();
        leftArrowButton.onClick.AddListener(ShowPreviousLevel);

        rightArrowButton.onClick.RemoveAllListeners();
        rightArrowButton.onClick.AddListener(ShowNextLevel);

        currentLevelButton.onClick.RemoveAllListeners();
        currentLevelButton.onClick.AddListener(SelectLevel);
    }

    /// <summary>
    /// Displays the next level in the menu.
    /// </summary>
    public void ShowNextLevel()
    {
        if (currentLevelIndex < levelNames.Length - 1)
        {
            currentLevelIndex++;
        }
        UpdateLevelDisplay();
    }

    /// <summary>
    /// Displays the previous level in the menu.
    /// </summary>
    public void ShowPreviousLevel()
    {
        if (currentLevelIndex > 0)
        {
            currentLevelIndex--;
        }
        UpdateLevelDisplay();
    }

    /// <summary>
    /// Updates the UI to display the current level and its details.
    /// </summary>
    private void UpdateLevelDisplay()
    {
        string levelName = levelNames[currentLevelIndex];
        Debug.Log($"Updating display: {levelName}");

        string spritePath = $"{spriteFolder}/{levelName}";
        Sprite levelSprite = Resources.Load<Sprite>(spritePath);

        if (levelSprite != null)
        {
            currentLevelDisplay.sprite = levelSprite;
        }
        else
        {
            Debug.LogError($"Sprite not found: {spritePath}");
        }

        if (levelTitleText != null)
        {
            levelTitleText.text = $"Level {currentLevelIndex + 1}";
        }

        bool isCompleted = PlayerPrefs.GetInt($"{levelName}Completed", 0) == 1;
        bool isUnlocked = currentLevelIndex == 0 || PlayerPrefs.GetInt($"{levelName}Unlocked", 0) == 1;

        currentLevelButton.interactable = isUnlocked;

        if (isCompleted)
        {
            currentLevelDisplay.color = Color.green;
        }
        else if (!isUnlocked)
        {
            currentLevelDisplay.color = Color.gray;
        }
        else
        {
            currentLevelDisplay.color = Color.white;
        }

        UpdateStars(levelName);

        leftArrowButton.gameObject.SetActive(currentLevelIndex > 0);
        rightArrowButton.gameObject.SetActive(currentLevelIndex < levelNames.Length - 1);
    }

    /// <summary>
    /// Updates the star rating display for the current level.
    /// </summary>
    private void UpdateStars(string levelName)
    {
        int starsEarned = PlayerPrefs.GetInt($"{levelName}_Stars", 0);

        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = i < starsEarned ? filledStar : emptyStar;
        }
    }

    /// <summary>
    /// Loads the currently selected level.
    /// </summary>
    private void SelectLevel()
    {
        string selectedLevel = levelNames[currentLevelIndex];
        Debug.Log($"Loading level: {selectedLevel}");
        menuManager.LoadLevel(selectedLevel);
    }

    /// <summary>
    /// Unlocks levels based on completion status of previous levels.
    /// </summary>
    private void UpdateLevelUnlocks()
    {
        string[] levels = PlayerPrefsArrayUtils.GetStringArray("LevelNames");

        for (int i = 0; i < levels.Length; i++)
        {
            string levelName = levels[i];

            if (i == 0 || PlayerPrefs.GetInt($"{levels[i - 1]}Completed", 0) == 1)
            {
                PlayerPrefs.SetInt($"{levelName}Unlocked", 1);
            }
        }

        PlayerPrefs.Save();
        Debug.Log("Level unlocks updated");
    }
}
