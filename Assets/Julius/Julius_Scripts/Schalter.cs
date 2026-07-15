using Unity.VisualScripting;
using UnityEngine;

public class Schalter : MonoBehaviour
{
    private FillUpTank Tank;
    private GameManager gameManager;

    
    private void Checkgame()
    {
        if (Tank.tankSolved == true)
        {

            gameManager.Wingame();
        }
       else gameManager.Losegame();
    }
}
