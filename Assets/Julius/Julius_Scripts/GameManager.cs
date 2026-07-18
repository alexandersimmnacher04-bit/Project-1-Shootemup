using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameState{ get; private set; } = false;
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private GameObject rocket;
    [SerializeField] private FillUpTank tank;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject Win;
    [SerializeField] private GameObject Lose;

    //[SerializeField] private GameObject task1;
    //[SerializeField] private GameObject task2;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    { 
        gameState = true;
        rocket.SetActive(true);
        //task1.SetActive(true);
        //task2.SetActive(true);
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!gameState)
        {
            return;
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Endgame()
    { if (tank.tankSolved == true)
        {
            Debug.Log("Du hast gewonnen congrats");
            Win.SetActive(true);
        }
        else
        {
            Debug.Log("Game Over");
            Lose.SetActive(true);
        }  
        gameState = false;
        Canvas.SetActive(true);

        
        


    }
    public void OpenMenu()
    {
        SceneManager.LoadScene(0);
    }

   

}
