using System;
using UnityEngine;

//making puzzle where player fills up tank when holding mouse button down
//maxtank capacity int = 100, current tank amount random between 0 and 100
//when tank filled to correct amount, player can move on to next puzzle

public class FillUpTank : MonoBehaviour
{

    int maxTank = 100;
    int currentTank = 20;
    bool isTankFull = false;

    void Start()
    {

    }

    void OnMouseDown()
    {
        //fill up tank when mouse button is held down
        //increase current tank amount by 1 every frame
        //if current tank amount is equal to max tank capacity, stop filling up tank
        //if current tank amount is equal to max tank capacity, player can move on to next puzzle
        
        while (currentTank < maxTank)
        {
            currentTank++;
            Debug.Log(currentTank);
        }


        isTankFull = true;
    }


    void Update()
    {

    }
}