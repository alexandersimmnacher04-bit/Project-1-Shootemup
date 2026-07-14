using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitchZone : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private PlayerInputHandler inputHandler;
    
    private bool playerInside = false;
    //neu
    private bool RoboterInside = false;
    //neu
    //Hier wird geschaut ob der Player in der TriggerBox ist oder nicht.
    private void Update()
    {
        if (!playerInside) return;

        if (inputHandler.InteractTriggered)
        {
            HandleInteract();
        }
        if(RoboterInside && cameraManager.IncamMode)
        {
            cameraManager.SwitchTo02();
        }
    }
    // Wenn der Spieler Interacted dann wird das Movement Blockiert. Wenn der Spieler nicht mehr interacted dann hört es auf.
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
    // Wenn der Player in der TriggerBox ist dan wird PlayerInside auf true gestzt.
    // Der Tag "Player" wird hier wieder verglichen.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
        if (other.CompareTag("RoboterArm"))
        {
            RoboterInside = true;
            if (cameraManager.IncamMode)
            {
                cameraManager.SwitchTo02();
            }
        }
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
        if (other.CompareTag("RoboterArm"))
        {
            RoboterInside = false;
            if (cameraManager.IncamMode)
            {
                cameraManager.SwitchTo01();
            }
        }
    }
        
}
