using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the level timer and star rating based on elapsed time.
/// </summary>
public class LevelTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public TextMeshProUGUI timerText; // Timer display text.
    public Image[] starImages;       // Array of star icons.
    public Sprite filledStar;        // Sprite for filled stars.
    public Sprite emptyStar;         // Sprite for empty stars.

    [Header("Star Time Limits")]
    public float timeLimitFor3Stars = 300f; // Time limit for 3 stars (in seconds).
    public float timeLimitFor2Stars = 600f; // Time limit for 2 stars.
    public float timeLimitFor1Star = 900f;  // Time limit for 1 star.

    private float timer = 0f;        // Elapsed time in seconds.
    public bool isTimerRunning = false; // Indicates if the timer is active.
    private int starsEarned = 3;     // Number of stars earned (default is 3).

    private void Start()
    {
        // Initialize timer display.
        UpdateTimerDisplay(0f);
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime; // Increment timer.
            UpdateTimerDisplay(timer); // Update timer display.
            UpdateStarsDisplay(); // Update star icons dynamically.
        }
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    /// <summary>
    /// Updates the timer display in "MM:SS" format.
    /// </summary>
    /// <param name="currentTime">The current elapsed time.</param>
    private void UpdateTimerDisplay(float currentTime)
    {
        float minutes = Mathf.FloorToInt(currentTime / 60); // Convert seconds to minutes.
        float seconds = Mathf.FloorToInt(currentTime % 60); // Remaining seconds.
        timerText.text = $"{minutes:00}:{seconds:00}"; // Display format "MM:SS".
    }

    /// <summary>
    /// Updates the star icons based on the elapsed time.
    /// </summary>
    private void UpdateStarsDisplay()
    {
        // Determine the number of stars based on elapsed time.
        if (timer <= timeLimitFor3Stars)
        {
            starsEarned = 3;
        }
        else if (timer <= timeLimitFor2Stars)
        {
            starsEarned = 2;
        }
        else if (timer <= timeLimitFor1Star)
        {
            starsEarned = 1;
        }
        else
        {
            starsEarned = 0; // No stars if time exceeds limits.
        }

        // Update star icons.
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = i < starsEarned ? filledStar : emptyStar;
        }
    }

    /// <summary>
    /// Returns the number of stars earned at the end of the level.
    /// </summary>
    /// <returns>The number of stars earned.</returns>
    public int GetStarsEarned()
    {
        return starsEarned;
    }
}
