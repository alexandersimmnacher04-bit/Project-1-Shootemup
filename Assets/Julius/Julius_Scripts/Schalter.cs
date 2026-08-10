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
    public OpenTank openTank;
    private float Timer;
    private bool buttonOn = true;
    private bool setTimer;
    public PlaySoundButton PlaySoundButton;
    public Play_Sound_Display Play_Sound_Display;


    private void Click()
    {
        Ray ray = new Ray(Playercamera.transform.position, Playercamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, openTank.interactDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if (buttonOn == true)
                {
                    PlaySoundButton.PlaySound();
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
        setTimer = true;
        Timer = 5f;
        Play_Sound_Display.PlayDisplaySound();
        //firstPersonController.ToggleMovement();
        //firstPersonController.ToggleCursor();

    }

    public void OnButtonNo()
    {
        buttonOn = true;
        Absicherung.SetActive(false);
        firstPersonController.ToggleMovement();
        firstPersonController.ToggleCursor();
    }
    private void Update()
    {
        if (setTimer)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
                gameManager.Endgame();
        }
        
    }

    //private void Checkgame()
    //{
    //    if (tank.tankSolved == true)
    //    {

    //        gameManager.Wingame();
    //    }
    //   else gameManager.Losegame();
    //}
}
