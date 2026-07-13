using UnityEngine;


// Game in which you complete multiple puzzles to start a rocket
//In this puzzle there are different sized screws that need to be placed in the correct holes. The player can pick up and rotate the screws to fit them into the holes.
//the screws need to be screwed to the correct amount.
//When the player had put all the screws in the correct holes and screwed them to the correct amount, the puzzle is completed and the player can move on to the next puzzle.
public class Script_Screw_Puzzle : MonoBehaviour
{
    
    int totalScrews = 5; // Total number of screws in the puzzle
    bool[] screwsPlaced; // Array to keep track of which screws have been placed correctly
    bool isPuzzleCompleted = false; // Flag to check if the puzzle is completed
    float[] screwTightness; // Array to keep track of the tightness of each screw


    // Update is called once per frame
    void Update()
    {
        
    }
}
