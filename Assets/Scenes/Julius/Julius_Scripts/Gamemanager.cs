using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Gamemanager : MonoBehaviour
{
    public bool gameState = false;
    public GameObject rocket;

    
    void Start()
    {
        gameState = true;
        rocket.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    { if (gameState == false)
            return;
        
    }
}
