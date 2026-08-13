using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;


//making puzzle where player fills up tank when holding mouse button down
//maxtank capacity int = 100, current tank amount random between 0 and 100
//when tank filled to correct amount, player can move on to next puzzle
//if current tank amount is equal to max tank capacity, player can move on to next puzzle
//make tank fill up over time, not instantly with OnMouseUp 
//make UI slider to show current tank amount
//player can overfill tank, cannot move onto next puzzle until tank is filled to correct amount



public class FillUpTank : MonoBehaviour
{
    [SerializeField] private float fillRate = 5f;
    [SerializeField] private float maxTank = 100f;
    [SerializeField] private float graceAmount = 15f;
    [SerializeField] private float threshHold = 130f;
    [SerializeField] private float emptyRate = 10f;
    private float currentTank = 1f;
    private int fillCount;
    private int emptyCount;
    public bool isFilling = false;
    private bool emptyTank = false;
    private bool errorOn = false;
   [SerializeField] private float errorTimer = 3f;
    public bool tankSolved { get; private set; }
    [Header("Objects and Scripts")]
    public Slider tankSlider;
    public OpenTank openTank;
    public GameObject emptyButton;
    public GameObject fillButton;
    public Play_Sound_Tank_Fill playSoundTankFill;
    public GameObject error;
    public GameObject sliderImage;
   [SerializeField] private GameObject redLight;
   [SerializeField] private GameObject greenLight;
   [SerializeField] private GameObject yellowLight;
    [SerializeField] private GameObject orangeLight;
    public Error_Sound sounderror;

    private void Start()
    {
        fillCount = 0;
        emptyCount = 0;
        errorTimer = 3f;
        tankSolved = false;
        currentTank = Random.Range(20, 50);
        tankSlider.minValue = 0;
        tankSlider.maxValue = maxTank + graceAmount;
        tankSlider.value = currentTank;
        Debug.Log("Current tank amount: " + currentTank);

    }

    public void Fillstart()
    {


        fillCount++;
        Debug.Log(fillCount);
       

    }


    private void Fillstop()
    {
        //if current tank amount is equal to max tank capacity, player can move on to next puzzle
        if (currentTank >= maxTank && currentTank <= maxTank + graceAmount)
        {
            tankSolved = true;
            
        }
        else tankSolved = false;

    }

    public void Emptystart()
    {
        emptyCount++;
        emptyRate = emptyRate + Random.Range(0, 5);
        Debug.Log(emptyRate);
        Debug.Log(tankSolved);
        if (openTank.Taskactive)
        {
            sliderImage.SetActive(false);
        }
    }

    private void Empty()
    {

        if (emptyTank)
        {
            fillButton.SetActive(false);
            currentTank -= emptyRate * Time.deltaTime;
            tankSlider.value = currentTank;

            if (currentTank <= 0 && openTank.Taskactive)
            {
                currentTank = 0;
                
            }
        }
        else fillButton.SetActive(true);
    }

    private void Fill()
    {
         
        if (isFilling)
        {
           
            emptyButton.SetActive(false);
            currentTank += fillRate * Time.deltaTime;
            tankSlider.value = currentTank;
             if (currentTank >= threshHold)
             {
                currentTank = threshHold;
             }

           
        }
        else emptyButton.SetActive(true);
    }
    private void ButtonStop()
    { //stop filling up tank when button is pressed 2nd time
        if (fillCount % 2 == 0) 
        {
          isFilling = false; 
        }
         else
        { 
            isFilling = true;
           

        }

        if (emptyCount % 2 == 0)
        {
            emptyTank = false;
            
            
        }
        else 
        {
            emptyTank = true; 
            sliderImage.SetActive(true);
        }
    }
    private void Update()
    {

        Fill();
        Empty();
        ButtonStop();
        Fillstop();
        if (!openTank.Taskactive)
        {
            fillCount = 0;
            emptyCount = 0;
        }

        
        LampChangeColor();

       if (fillCount % 2 != 0 && openTank.Taskactive)
       {
            GetComponent<AudioSource>().enabled = true;
       }
       else 
            GetComponent<AudioSource>().enabled = false;
        if (currentTank == 0 || currentTank == threshHold)
        {
           sounderror.GetComponent<AudioSource>().enabled = true;
            error.SetActive(true);
        }
        else
        {
            sounderror.GetComponent<AudioSource>().enabled = false;
            error.SetActive(false);
        }
    }

  

    private void LampChangeColor()
    {
        if (isFilling || emptyTank)
        {
            if (currentTank < 50f)
            {
                greenLight.SetActive(false);
                yellowLight.SetActive(false);
                redLight.SetActive(false);
                orangeLight.SetActive(true);
            }
            else if (currentTank > 50f && currentTank < maxTank)
            {
                greenLight.SetActive(false);
                yellowLight.SetActive(true);
                redLight.SetActive(false);
                orangeLight.SetActive(false);
            }
            else if (currentTank >= maxTank && currentTank <= maxTank + graceAmount)
            {
                greenLight.SetActive(true);
                yellowLight.SetActive(false);
                redLight.SetActive(false);
                orangeLight.SetActive(false);
            }
            else if (currentTank < threshHold && currentTank > maxTank + graceAmount)
            {
                greenLight.SetActive(false);
                yellowLight.SetActive(false);
                redLight.SetActive(false);
                orangeLight.SetActive(true);
            }
            else if (currentTank >= threshHold || currentTank >= 0)
            {
                greenLight.SetActive(false);
                yellowLight.SetActive(false);
                redLight.SetActive(true);
                orangeLight.SetActive(false);
            }
            else
            {
                greenLight.SetActive(false);
                yellowLight.SetActive(false);
                redLight.SetActive(false);
                orangeLight.SetActive(false);
            }
        }
        else 
        { 
            greenLight.SetActive(false); 
            yellowLight.SetActive(false); 
            redLight.SetActive(false);
            orangeLight.SetActive(false);
        }
       
    }
    
}


