using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Represents a slot for placing puzzle pieces.
/// Handles interactions, visual feedback, and puzzle logic.
/// </summary>
public class PuzzleSlot : MonoBehaviour
{
    [Header("Slot Properties")]
    public int slotID; // Unique ID for the slot
    public bool isOccupied = false; // Indicates if a piece is placed in the slot

    private Renderer slotRenderer; // Renderer for visual updates
    private Color originalColor;   // Original color of the slot

    private void Awake()
    {
        // Initialize the renderer and save the original color
        slotRenderer = GetComponent<Renderer>();
        if (slotRenderer != null)
        {
            originalColor = slotRenderer.material.color;
        }
    }

    /// <summary>
    /// Called when a hover interaction enters the slot.
    /// </summary>
    /// <param name="args">Hover enter event arguments.</param>
    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        HighlightSlot();
    }

    /// <summary>
    /// Called when a hover interaction exits the slot.
    /// </summary>
    /// <param name="args">Hover exit event arguments.</param>
    public void OnHoverExited(HoverExitEventArgs args)
    {
        ResetSlotColor();
    }

    /// <summary>
    /// Highlights the slot to provide visual feedback.
    /// </summary>
    public void HighlightSlot()
    {
        Debug.Log($"Slot {slotID} highlighted");
        if (slotRenderer != null)
        {
            slotRenderer.material.color = Color.yellow;
        }
    }

    /// <summary>
    /// Resets the slot to its original color.
    /// </summary>
    public void ResetSlotColor()
    {
        if (slotRenderer != null)
        {
            slotRenderer.material.color = originalColor;
        }
    }


    /// <summary>
    /// Attempts to place a puzzle piece in the slot.
    /// </summary>
    /// <param name="piece">The puzzle piece to place.</param>
    /// <returns>True if the placement is successful; otherwise, false.</returns>
    public bool TryPlacePiece(PuzzlePiece piece)
    {
        if (isOccupied)
        {
            return false;
        }

        if (piece.pieceID == slotID) // Verify if the piece matches the slot
        {
            Debug.Log($"Piece {piece.pieceID} placed in slot {slotID}.");

            // Apply the piece texture to the slot
            SetTexture(piece.GetTexture());

            // Mark the slot as occupied
            isOccupied = true;

            // Start the timer if not already running
            LevelTimer levelTimer = FindObjectOfType<LevelTimer>();
            if (levelTimer != null && !levelTimer.isTimerRunning)
            {
                levelTimer.StartTimer();
            }

            // Check if the puzzle is completed
            GameManager.Instance.CheckPuzzleCompletion();

            return true;
        }
        else
        {
            Debug.Log($"Piece {piece.pieceID} does not match slot {slotID}.");
            return false;
        }
    }

    /// <summary>
    /// Sets a texture on the slot.
    /// </summary>
    /// <param name="texture">The texture to apply.</param>
    public void SetTexture(Texture2D texture)
    {
        if (slotRenderer != null && texture != null)
        {
            slotRenderer.material.mainTexture = texture;
            Debug.Log($"Texture applied to slot {slotID}.");
        }
        else
        {
            Debug.LogError($"Missing texture for slot {slotID}.");
        }
    }
}
