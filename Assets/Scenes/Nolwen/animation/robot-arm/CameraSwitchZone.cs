using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitchZone : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private PlayerInputHandler inputHandler;
    
    private bool playerInside = false;

    private void Update()
    {
        if (!playerInside) return;

        if (inputHandler.InteractTriggered)
        {
            HandleInteract();
        }
    }

    private void HandleInteract()
    {
        if (!cameraManager.IncamMode)
        {
            cameraManager.SwitchTo01();
            playerController.BlockMovement(true);
        }
        else
        {
            cameraManager.SwitchToPlayer();
            playerController.BlockMovement(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }
    // Wenn der Player nicht mehr in der TriggerBox ist dann wird PlayerInsdide wieder auf false gesetzt.
    // Der Tag "Player" wird hier wieder verglichen.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            cameraManager.SwitchToPlayer();
            playerController.BlockMovement(false);
        }
    }
}
