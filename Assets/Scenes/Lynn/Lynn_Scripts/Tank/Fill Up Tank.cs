using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

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
    [SerializeField] private float graceAmount = 5f;
    [SerializeField] private float threshHold = 120f;
    [SerializeField] private float emptyRate = 10f;
    private float currentTank = 0f;
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
   [SerializeField] public GameObject redLight;
   [SerializeField] public GameObject greenLight;
   [SerializeField] public GameObject yellowLight;


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
    }

    private void Empty()
    {

        if (emptyTank)
        {
            fillButton.SetActive(false);
            currentTank -= emptyRate * Time.deltaTime;
            tankSlider.value = currentTank;

            if (currentTank <= 0)
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

            if (currentTank > maxTank + graceAmount)
            {
                tankSolved = false;
               
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

        if (fillCount == 1 || emptyCount == 1)
        {
            errorTimer -= 1f* Time.deltaTime;
            if (errorTimer <= 0)
            {
                errorTimer = 0;
                ToggleError();
                sliderImage.SetActive(false);
            }

        }
        LampChangeColor();

        if (fillCount % 2 == 0 && openTank.Taskactive)
        {
            playSoundTankFill.PlayFillFuelSound();
        }
    }

   private void ToggleError()
    { 
        errorOn = !errorOn;

        if (errorOn) 
        
        {
            error.SetActive(true); 
        }
        else 
        { 
            error.SetActive(false); 
        }
    }

    private void LampChangeColor()
    {
        if (isFilling || emptyTank)
        {

            if (currentTank < maxTank)
            {
                greenLight.SetActive(false);
                yellowLight.SetActive(true);
                redLight.SetActive(false);
            }
            else if (currentTank >= maxTank && currentTank <= maxTank + graceAmount)
            {
                greenLight.SetActive(true);
                yellowLight.SetActive(false);
                redLight.SetActive(false);
            }
            else if (currentTank <= 110f && currentTank > maxTank + graceAmount)
            {
                greenLight.SetActive(false);
                yellowLight.SetActive(true);
                redLight.SetActive(false);

            }
            else if (currentTank >= 110f)
            {
                greenLight.SetActive(false);
                yellowLight.SetActive(false);
                redLight.SetActive(true);


            }
            else
            {
                greenLight.SetActive(false);
                yellowLight.SetActive(false);
                redLight.SetActive(false);

            }
        }
        else 
        { 
            greenLight.SetActive(false); 
            yellowLight.SetActive(false); 
            redLight.SetActive(false); 
        }
       
    }
    
}


