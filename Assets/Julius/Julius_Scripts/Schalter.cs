using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Schalter : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject Absicherung;
    [SerializeField] private FirstPersonController firstPersonController;
    private float Timer;
    private bool buttonOn = true;
    private bool setTimer;
   

    private void OnMouseDown()
    { if (buttonOn == true)
        {
            Debug.Log("click");
            buttonOn = false;
            AreyouSure();
           
        }
        else return;
    }

    private void AreyouSure()
    {
        firstPersonController.ToggleMovement();
        firstPersonController.ToggleCursor();
        Absicherung.SetActive(true);
    }
     
    public void Yes()
    {
        Absicherung.SetActive(false);
        setTimer = true;
        Timer = 5f;
      
    }

    public void No()
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
