using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles pagination for an inventory system, allowing navigation through items by pages.
/// </summary>
public class InventoryPaginator : MonoBehaviour
{
    public Transform inventoryContent; // Parent object containing inventory items.
    public int itemsPerPage = 20;      // Number of items displayed per page.
    public Button nextPageButton;      // Button to navigate to the next page.
    public Button previousPageButton;  // Button to navigate to the previous page.

    private List<GameObject> inventoryItems = new List<GameObject>(); // List of inventory items.
    private int currentPage = 0;       // Index of the current page.

    private void Start()
    {
        // Assign button functions if the buttons are set.
        if (nextPageButton != null)
            nextPageButton.onClick.AddListener(NextPage);

        if (previousPageButton != null)
            previousPageButton.onClick.AddListener(PreviousPage);
    }

    /// <summary>
    /// Initializes the pagination system with the provided list of items.
    /// </summary>
    /// <param name="items">List of inventory items to paginate.</param>
    public void InitializePagination(List<GameObject> items)
    {
        inventoryItems = items;
        currentPage = 0;
        UpdatePage();
    }

    /// <summary>
    /// Updates the visibility of items for the current page.
    /// </summary>
    private void UpdatePage()
    {
        if (inventoryContent == null) return;

        // Calculate indices for the current page.
        int startIndex = currentPage * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, inventoryItems.Count);

        // Enable or disable items based on the current page.
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].SetActive(i >= startIndex && i < endIndex);
        }

        // Enable or disable navigation buttons based on the page index.
        if (previousPageButton != null)
            previousPageButton.interactable = currentPage > 0;

        if (nextPageButton != null)
            nextPageButton.interactable = endIndex < inventoryItems.Count;
    }

    /// <summary>
    /// Navigates to the next page if possible.
    /// </summary>
    public void NextPage()
    {
        if ((currentPage + 1) * itemsPerPage < inventoryItems.Count)
        {
            currentPage++;
            UpdatePage();
        }
    }

    /// <summary>
    /// Navigates to the previous page if possible.
    /// </summary>
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }
}
