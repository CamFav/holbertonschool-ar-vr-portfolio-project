using UnityEngine;

/// <summary>
/// Puzzle piece with unique properties and functionality.
/// </summary>
public class PuzzlePiece : MonoBehaviour
{
    /// <summary>
    /// Unique ID for the puzzle piece.
    /// </summary>
    public int pieceID;

    /// <summary>
    /// Indicates whether the puzzle piece is locked in place.
    /// </summary>
    public bool isLocked = false;

    /// <summary>
    /// Texture associated with the puzzle piece.
    /// </summary>
    public Texture2D pieceTexture;

    /// <summary>
    /// Locks the puzzle piece and disables interaction.
    /// </summary>
    public void LockPiece()
    {
        isLocked = true;

        // Disable interaction once locked.
        GetComponent<Collider>().enabled = false;

        Debug.Log($"Piece {pieceID} locked!");
    }

    /// <summary>
    /// Retrieves the texture of the puzzle piece.
    /// </summary>
    /// <returns>Texture associated with the piece.</returns>
    public Texture2D GetTexture()
    {
        return pieceTexture;
    }
}
