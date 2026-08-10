using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Schalter : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject Absicherung;
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private Camera Playercamera;
    [SerializeField] private trigger_animation_crawlerfährtaushangar Animationmanager;
    [SerializeField] private GameObject Console;
    public OpenTank openTank;
    private bool buttonOn = true;
    public bool animationOn { get; private set; } = false;
    public PlaySoundButton PlaySoundButton;
    public Play_Sound_Display Play_Sound_Display;
    public GameObject Lampe1;
    public GameObject Lampe2;
    public GameObject Lampe3;


    private void Click()
    {
        Ray ray = new Ray(Playercamera.transform.position, Playercamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, openTank.interactDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if (buttonOn == true)
                {
                    //PlaySoundButton.PlaySound();
                    Debug.Log("click");
                    buttonOn = false;
                    AreyouSure();

                }
                else return;
            }
        }
        
    }

    private void AreyouSure()
    {
        firstPersonController.ToggleMovement();
        firstPersonController.ToggleCursor();
        Absicherung.SetActive(true);
    }
     
    public void OnButtonYes()
    {
        Absicherung.SetActive(false);
        PlaySoundButton.PlaySound();
        animationOn = true;
        Animationmanager.closedoor01();
        Animationmanager.closedoor02();
        Animationmanager.turnredlighton();
        Animationmanager.turngreenlightoff();
        firstPersonController.ToggleMovement();
        firstPersonController.ToggleCursor();
        Console.GetComponent<BoxCollider>().enabled = false;
        Lampe1.SetActive(false);
        Lampe2.SetActive(false);
        Lampe3.SetActive(false);


    }

    public void OnButtonNo()
    {
        buttonOn = true;
        Absicherung.SetActive(false);
        firstPersonController.ToggleMovement();
        firstPersonController.ToggleCursor();
    }
    

    
}
