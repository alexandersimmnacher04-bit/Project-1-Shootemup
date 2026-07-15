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
    private float currentTank = 0f;
    private bool isFilling = false;
    //private bool finished = false;
    public bool tankSolved {  get; private set; }
    // public Slider tankSlider;


    void Start()
    {
        tankSolved = false;
        currentTank = Random.Range(20, 70);
        //tankSlider.minValue = 0;
        //tankSlider.maxValue = maxTank + graceAmount;
        //tankSlider.value = currentTank;
        Debug.Log("Current tank amount: " + currentTank);
    }
    void OnMouseDown()
    {

            isFilling = true;
        Debug.Log("Hallo");

    }

    void OnMouseUp()
    {
        //stop filling up tank when mouse button is released
        //if current tank amount is equal to max tank capacity, player can move on to next puzzle
        if (currentTank >= maxTank && currentTank <= maxTank + graceAmount)
        {
            tankSolved = true;
            Debug.Log("Tank is full! You can move on to the next puzzle.");
            // Win();
        }
        isFilling = false;
    }

    void Update()
    { 
        //if (finished) return;  
        if (isFilling)
        {
            currentTank += fillRate * Time.deltaTime;
            //  tankSlider.value = currentTank;
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
                //Lose();
                tankSolved = false;
            }
            
                
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


