using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates and manages puzzle pieces in the inventory.
/// </summary>
public class PuzzlePieceGenerator : MonoBehaviour
{
    /// <summary>
    /// Prefab used to create puzzle pieces.
    /// </summary>
    public GameObject piecePrefab;

    /// <summary>
    /// Parent container for puzzle pieces.
    /// </summary>
    public Transform inventoryContent;

    /// <summary>
    /// Number of rows in the puzzle grid.
    /// </summary>
    public int rows;

    /// <summary>
    /// Number of columns in the puzzle grid.
    /// </summary>
    public int cols;

    /// <summary>
    /// Scale for puzzle pieces in the inventory.
    /// </summary>
    public Vector3 inventoryScale = new Vector3(1f, 1f, 1f);

    /// <summary>
    /// List to keep track of all generated pieces.
    /// </summary>
    private List<GameObject> pieces = new List<GameObject>();

    /// <summary>
    /// Clears all existing pieces from the inventory.
    /// </summary>
    public void ClearInventory()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        pieces.Clear();
    }

    /// <summary>
    /// Generates puzzle pieces in the inventory with assigned IDs and textures.
    /// </summary>
    /// <param name="puzzleImage">Source puzzle image to be sliced into pieces.</param>
    public void GeneratePiecesInScrollView(Texture2D puzzleImage)
    {
        ClearInventory();

        if (piecePrefab == null || inventoryContent == null || puzzleImage == null)
        {
            Debug.LogError("Prefab, InventoryContent, or PuzzleImage is not assigned");
            return;
        }

        int pieceWidth = puzzleImage.width / cols;
        int pieceHeight = puzzleImage.height / rows;

        int id = 0; // Logical ID for pieces, starts from 0.

        for (int row = rows - 1; row >= 0; row--)
        {
            for (int col = 0; col < cols; col++)
            {
                // Slice the texture for this piece.
                Texture2D pieceTexture = new Texture2D(pieceWidth, pieceHeight);
                pieceTexture.SetPixels(puzzleImage.GetPixels(col * pieceWidth, row * pieceHeight, pieceWidth, pieceHeight));
                pieceTexture.Apply();

                // Create a piece GameObject and set its properties.
                GameObject piece = Instantiate(piecePrefab, inventoryContent);
                piece.transform.localScale = inventoryScale;

                // Assign the texture to the piece.
                Renderer renderer = piece.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = new Material(renderer.sharedMaterial);
                    mat.mainTexture = pieceTexture;
                    renderer.material = mat;
                }

                // Assign ID and texture to the PuzzlePiece component.
                PuzzlePiece puzzlePiece = piece.GetComponent<PuzzlePiece>();
                if (puzzlePiece != null)
                {
                    puzzlePiece.pieceID = id;
                    puzzlePiece.pieceTexture = pieceTexture;
                }

                pieces.Add(piece);
                id++;
            }
        }

        Debug.Log($"Generation complete: {rows * cols} pieces created.");
        ShuffleInventory(); // Shuffle the visual order of pieces.

        // Initialize pagination if InventoryPaginator is present.
        InventoryPaginator paginator = GetComponent<InventoryPaginator>();
        if (paginator != null)
        {
            paginator.InitializePagination(pieces);
        }
    }

    /// <summary>
    /// Randomly shuffles the visual order of pieces in the inventory.
    /// </summary>
    private void ShuffleInventory()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            int randomIndex = Random.Range(0, pieces.Count);
            pieces[i].transform.SetSiblingIndex(randomIndex);
        }
    }
}
