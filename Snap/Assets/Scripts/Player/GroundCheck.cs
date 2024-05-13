using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;

        playerController.SetGrounded(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;

        playerController.SetGrounded(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;

        playerController.SetGrounded(false);
    }
}
