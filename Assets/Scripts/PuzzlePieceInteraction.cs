using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Handles VR interactions with puzzle pieces
public class PuzzlePieceInteractionVR : MonoBehaviour
{
    private Vector3 originalScale; // Original size of the piece in the inventory
    private Transform originalParent; // Original parent of the piece
    private Rigidbody rb; // Rigidbody for physics handling
    private Transform handTransform; // Reference to the hand holding the piece
    private PuzzleSlot currentTargetSlot; // Currently targeted slot

    private void Start()
    {
        // Save the initial scale and parent of the piece
        originalScale = transform.localScale;
        originalParent = transform.parent;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (handTransform != null)
        {
            PerformRaycast(); // Keep checking for slots while holding the piece
        }
    }

    /// <summary>
    /// Called when the piece is grabbed by the player.
    /// </summary>
    /// <param name="args">Interaction event arguments.</param>
    public void OnGrabbed(SelectEnterEventArgs args)
    {

        handTransform = args.interactorObject.transform;

        // Scale the piece to its real-world size
        transform.localScale = Vector3.one;

        // Detach the piece from the inventory
        transform.SetParent(null);

        // Temporarily disable physics for smooth interaction
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    /// <summary>
    /// Called when the piece is released by the player.
    /// </summary>
    /// <param name="args">Interaction event arguments.</param>
    public void OnReleased(SelectExitEventArgs args)
    {
        // Check if the piece is over a valid slot
        if (currentTargetSlot != null)
        {
            Debug.Log($"Trying to place piece in slot {currentTargetSlot.slotID}");

            PuzzlePiece puzzlePiece = GetComponent<PuzzlePiece>();
            if (puzzlePiece == null)
            {
                ReturnToInventory();
                return;
            }

            // Try to place the piece in the slot
            if (currentTargetSlot.TryPlacePiece(puzzlePiece))
            {
                Debug.Log($"Piece {name} locked in slot {currentTargetSlot.slotID}");

                gameObject.SetActive(false); // Disable the piece after placement
                currentTargetSlot.ResetSlotColor(); // Reset slot highlight
                currentTargetSlot = null;
                return;
            }
        }

        // Return the piece to the inventory if placement failed
        ReturnToInventory();
    }

    /// <summary>
    /// Returns the piece to its original position in the inventory.
    /// </summary>
    private void ReturnToInventory()
    {
        Debug.Log($"Returning {name} to inventory");

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Collider pieceCollider = GetComponent<Collider>();
        if (pieceCollider != null)
        {
            pieceCollider.enabled = false; // Temporarily disable collider
        }

        // Reset parent, position, rotation, and scale
        transform.SetParent(originalParent, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = originalScale;

        if (currentTargetSlot != null)
        {
            currentTargetSlot.ResetSlotColor(); // Reset the targeted slot if applicable
            currentTargetSlot = null;
        }

        Debug.Log($"Piece {name} has been reset in the inventory");

        if (pieceCollider != null)
        {
            pieceCollider.enabled = true; // Re-enable collider
        }
    }

    /// <summary>
    /// Performs a raycast from the hand to detect valid slots.
    /// </summary>
    private void PerformRaycast()
    {
        LayerMask layerMask = LayerMask.GetMask("PuzzleSlot"); // Target only the PuzzleSlot layer
        Ray ray = new Ray(handTransform.position, handTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 5f, layerMask))
        {
            PuzzleSlot slot = hit.collider.GetComponent<PuzzleSlot>();
            if (slot != null)
            {
                // Highlight the new slot and reset the previous one
                if (slot != currentTargetSlot)
                {
                    currentTargetSlot?.ResetSlotColor();
                    currentTargetSlot = slot;
                    currentTargetSlot.HighlightSlot();
                }
            }
        }
        else
        {
            // Reset the previous slot if no slot is targeted
            currentTargetSlot?.ResetSlotColor();
            currentTargetSlot = null;
        }
    }
}
