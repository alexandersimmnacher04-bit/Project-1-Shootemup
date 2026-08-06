using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class OpenTank : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject TankCanvas;
    public bool Taskactive { get; private set; }
    public float interactDistance = 5f;
    public Camera Playercamera;
    public FirstPersonController firstPersonController;
    public void Interact()
    {
        Ray ray = new Ray(Playercamera.transform.position, Playercamera.transform.forward);

        if (Physics.Raycast(ray,out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                toggletask();
                firstPersonController.ToggleMovement();
                firstPersonController.ToggleCursor();
            }
        }
        
    }

    public void toggletask()
    {
        Taskactive = !Taskactive;
       

    }
    private void Update()
    {
        if (Taskactive) {
            TankCanvas.SetActive(true);
        }
        else TankCanvas.SetActive(false);
    }
}
