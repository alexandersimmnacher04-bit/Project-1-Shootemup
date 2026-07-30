using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class OpenTank : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject TankCanvas;
    private bool Taskactive;
    public FirstPersonController firstPersonController;
    public void Interact()
    {
        toggletask();
        firstPersonController.ToggleMovement();
        firstPersonController.ToggleCursor();
    }

    private void toggletask()
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
