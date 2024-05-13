using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    public void OnLook(InputAction.CallbackContext cxt)
    {
        // Get mouse input
        Vector2 lookVector = cxt.ReadValue<Vector2>();

        // Apply sway multiplier to look vector components
        float mouseX = lookVector.x * swayMultiplier;
        float mouseY = lookVector.y * swayMultiplier;

        // Get rotation quaternions
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // Rotate the item holder
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
