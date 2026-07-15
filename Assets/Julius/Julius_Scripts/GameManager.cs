using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameState = false;
    [SerializeField] private GameObject rocket;
    [SerializeField] private GameObject task1;
    [SerializeField] private GameObject task2;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        gameState = true;
        rocket.SetActive(true);
        task1.SetActive(true);
        task2.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameState)
        {
            return;
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Wingame()
    {
        Debug.Log("Du hast gewonnen congrats");
    }

    public void Losegame()
    {
        Debug.Log("Game Over");
    }

}
