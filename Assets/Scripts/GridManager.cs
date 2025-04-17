using UnityEngine;

/// <summary>
/// Manages the creation, clearing, and validation of the puzzle grid.
/// </summary>
public class GridManager : MonoBehaviour
{
    public GameObject slotPrefab;    // Prefab for each slot
    public int rows;                 // Number of rows
    public int cols;                 // Number of columns
    public float gridWidth = 10f;    // Total grid width
    public float gridHeight = 10f;   // Total grid height
    public float spacingPercentage = 0.1f; // Spacing percentage between slots (relative to slot size)
    public Transform gridPosition;   // Origin point of the grid

    /// <summary>
    /// Generates a rectangular grid with a fixed overall size.
    /// </summary>
    public void GenerateGrid()
    {
        ClearGrid();

        if (gridPosition == null)
        {
            return;
        }

        Debug.Log($"Generating grid with {rows}x{cols} slots.");

        // Calculate slot size based on grid dimensions
        float slotWidth = gridWidth / cols;
        float slotHeight = gridHeight / rows;
        float slotSize = Mathf.Min(slotWidth, slotHeight);

        // Calculate spacing between slots
        float spacing = slotSize * spacingPercentage;

        // Calculate actual grid dimensions considering slot size and spacing
        float actualGridWidth = cols * (slotSize + spacing) - spacing;
        float actualGridHeight = rows * (slotSize + spacing) - spacing;

        // Offset to center the grid
        Vector3 gridOffset = new Vector3(-actualGridWidth / 2 + slotSize / 2, actualGridHeight / 2 - slotSize / 2, 0);

        int slotID = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float xPos = gridOffset.x + col * (slotSize + spacing);
                float yPos = gridOffset.y - row * (slotSize + spacing);

                // Position relative to the grid origin
                Vector3 slotPosition = new Vector3(xPos, yPos, 0) + gridPosition.position;

                GameObject slotObj = Instantiate(slotPrefab, slotPosition, Quaternion.identity, transform);

                if (slotObj != null)
                {
                    Debug.Log($"Slot created: {slotObj.name} at position {slotPosition}");

                    // Adjust slot size
                    slotObj.transform.localScale = new Vector3(slotSize, slotSize, 1);

                    // Assign slot ID
                    PuzzleSlot slotScript = slotObj.GetComponent<PuzzleSlot>();
                    if (slotScript != null)
                    {
                        slotScript.slotID = slotID;
                    }

                    slotObj.name = $"Slot_{slotID}";
                    slotID++;
                }
                else
                {
                    Debug.LogError($"Error creating slot at position {slotPosition}");
                }
            }
        }

        Debug.Log($"Grid successfully generated: {rows}x{cols} slots.");
    }

    /// <summary>
    /// Clears all existing slots from the grid.
    /// </summary>
    public void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Checks if all slots in the grid are occupied.
    /// </summary>
    /// <returns>True if all slots are occupied, otherwise false.</returns>
    public bool AreAllSlotsOccupied()
    {
        PuzzleSlot[] slots = GetComponentsInChildren<PuzzleSlot>();
        foreach (PuzzleSlot slot in slots)
        {
            if (!slot.isOccupied)
            {
                return false;
            }
        }
        return true;
    }
}
