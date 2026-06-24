using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Gamemanager : MonoBehaviour
{
    [SerializeField] private bool gameState = false;
    private GameObject rocket; 

    
     private void Start()
    {
        gameState = true;
        rocket.SetActive(true);
    }

    // Update is called once per frame
   private void Update()
    { if (gameState == false)
            return;
        
    }

}
