using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameState = false;
    public GameObject rocket;
    public GameObject task1;
    public GameObject task2;
    


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
  
}
