using System;
using UnityEditor.Experimental.GraphView;
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
    [SerializeField] private float graceAmount = 5f;
    [SerializeField] private float threshHold = 110f;
    [SerializeField] private float emptyRate = 5f;
    private int fillCount;
    private int emptyCount;
    private float currentTank = 0f;
    private bool isFilling = false;
    private bool emptyTank = false;
    //private bool finished = false;
    public bool tankSolved {  get; private set; }
    public Slider tankSlider;
    public OpenTank openTank;


   private void Start()
   {
        fillCount = 0;
        emptyCount = 0;
        tankSolved = false;
        currentTank = Random.Range(20, 70);
        tankSlider.minValue = 0;
        tankSlider.maxValue = maxTank + graceAmount;
        tankSlider.value = currentTank;
        Debug.Log("Current tank amount: " + currentTank);
   }
   
    public void Fillstart()
    {

        //isFilling = true;
        Debug.Log("Hallo");
        fillCount++;

    }


   private void Fillstop()
   {
        //if current tank amount is equal to max tank capacity, player can move on to next puzzle
        if (currentTank >= maxTank && currentTank <= maxTank + graceAmount)
        {
            tankSolved = true;
            Debug.Log("Tank is full! You can move on to the next puzzle.");
            // Win();
        }
        
   }

   public void Emptystart()
    {
        emptyCount++;
        emptyRate = emptyRate + Random.Range(0, 5);
        Debug.Log(emptyRate);
    }
   
   private void Empty()
   {
      
      if (emptyTank)
      {
        currentTank -= emptyRate * Time.deltaTime;
        tankSlider.value = currentTank;
         if (currentTank > 0)
         {
            Debug.Log(currentTank);
         }
         else if (currentTank <= 0)
         {
                currentTank = 0;
                Debug.Log(currentTank);
         }
      }
   }

    private void Fill()
    {
        //if (finished) return;  
        if (isFilling)
        {
            currentTank += fillRate * Time.deltaTime;
            tankSlider.value = currentTank;
            if (currentTank < threshHold)
            {
                Debug.Log(currentTank);
            }
            else if (currentTank >= threshHold)
            {
                currentTank = threshHold;
                Debug.Log(currentTank);
            }

            if (currentTank > maxTank + graceAmount)
            {
                tankSolved = false;
            }
        }
    }
    private void ButtonStop()
    { //stop filling up tank when button is pressed 2nd time
        if (fillCount % 2 == 0)
            isFilling = false;
        else isFilling = true;
        if (emptyCount % 2 == 0)
            emptyTank = false;
        else emptyTank = true;
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
    }

    //void Win()
    //{
    //    finished = true;
    //    currentTank = maxTank;
    //    // tankSlider.value = currentTank;
    //    Debug.Log("Yippie. Du hast es geschafft!");
    //}
    //void Lose()
    //{
    //    finished = true;
    //    isFilling = false;
    //    Debug.Log("Du Loser oder whatever");
    //}
    // private void Toggle()
    //{
    //    solved = !solved;
    //}
}


