using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange;
    [SerializeField] private Transform interactorSource;
    [SerializeField] private CameraEquipment cameraEquipment;

    private bool focusing = false;

    public void OnInteract(InputAction.CallbackContext cxt)
    {
        // Exit if the mouse is being held down
        if (!cxt.started) return;

        // Shoot a ray from the camera transform, to a maximum interaction range
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        Physics.Raycast(r, out RaycastHit hit, interactionRange);

        bool lookingAtInteractable = false;

        // Get the interactable object if the player is looking at an interactable object
        IInteractable interactableObj = null;
        if (hit.collider != null) lookingAtInteractable = hit.collider.gameObject.TryGetComponent(out interactableObj);

        // Interact or use object
        if (focusing) cameraEquipment.Use();
        else if (lookingAtInteractable) interactableObj.Interact();
    }

    public void OnFocus(InputAction.CallbackContext cxt)
    {
        focusing = cxt.ReadValue<float>() == 1f;

        if (focusing) cameraEquipment.Focus();
        else cameraEquipment.Unfocus();
    }
}
