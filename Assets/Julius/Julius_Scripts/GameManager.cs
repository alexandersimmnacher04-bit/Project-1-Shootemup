using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public bool gameState { get; private set; } = false;
    [Header("GameObjects")]
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private GameObject rocket;
    [SerializeField] private FillUpTank tank;
    [SerializeField] private AblageZone ablageZone;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject Win;
    [SerializeField] private GameObject Lose;
    [SerializeField] private GameObject buttonClose;
    [SerializeField] private trigger_animation_crawlerfährtaushangar animationmanager;
    [SerializeField] private GameObject buttonUi;
    [SerializeField] private Play_Sound_Display play_Sound_Display;
    [SerializeField] private GameObject Lampe1;
    [SerializeField] private GameObject Lampe2;
    [SerializeField] private GameObject Lampe3;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject jobInst;
    [SerializeField] private PlaySoundButton buttonSound;
    private float Timer = 47;
    private bool setTimer = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        gameState = true;
        rocket.SetActive(true);
        jobInst.SetActive(true);
        firstPersonController.ToggleCursor();
        firstPersonController.ToggleMovement();
       

    }

    // Update is called once per frame
    private void Update()
    {
        if (!gameState)
        {
            return;
        }
        if (setTimer)
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                

                if (tank.tankSolved == true && ablageZone.raetselSolved == true)
                {
                    Win.SetActive(true);
                    animationmanager.turnredlightoff();
                    animationmanager.turngreenlighton();
                    Lampe1.SetActive(true);
                    Lampe2.SetActive(true);
                    Lampe3.SetActive(true);
                    
                    
                }
                else
                {
                    Lose.SetActive(true);
                   
                }

                
                gameState = false;
                Canvas.SetActive(true);
                buttonUi.SetActive(true);
                restartButton.SetActive(false);

            }
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Endgame()
    {
        play_Sound_Display.PlayDisplaySound();
        setTimer = true;
        if (tank.tankSolved == true && ablageZone.raetselSolved)
        {
            Timer = 44f;
            animationmanager.videosuccessanimationstart();
        }
        else
        {
           
            animationmanager.videofailanimationstart();

        }
        
       


    }
    public void OpenTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseMenu()
    {
        firstPersonController.ToggleCanvas();
        buttonClose.SetActive(false);
        firstPersonController.ToggleMovement();
        firstPersonController.ToggleCursor();
        
    }
    public void CloseJobInst()
    {
        buttonSound.PlaySound();
        jobInst.SetActive(false);
        firstPersonController.ToggleCursor();
        firstPersonController.ToggleMovement();

    }
}
