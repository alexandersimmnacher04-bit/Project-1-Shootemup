using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Schalter : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject Absicherung;
    [SerializeField] private FirstPersonController firstPersonController;
    private float Timer = 5;
    private bool buttonOn = true;
   

    private void OnMouseDown()
    { if (buttonOn == true)
            AreyouSure();
        else return;
    }

    private void AreyouSure()
    {
        firstPersonController.BlockMovement(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Absicherung.SetActive(true);
    }
     
    public void Yes()
    {
        buttonOn = false;
        Absicherung.SetActive(false); 
        while (Timer > 0)
        {
            Debug.Log("tick");
            Timer -= Time.deltaTime;
        }
        if (Timer <= 0) 
            gameManager.Endgame();
      
    }

    public void No()
    {
        firstPersonController.BlockMovement(false);
        Absicherung.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
