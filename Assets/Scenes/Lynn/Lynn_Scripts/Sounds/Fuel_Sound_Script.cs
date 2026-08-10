using Unity.VisualScripting;
using UnityEngine;

// Funktion, die im Update() aufgerufen wird, um den Sound abzuspielen, wenn isFilling true ist
public class Fuel_Sound_Script : MonoBehaviour
{
    public Play_Sound_Tank_Fill playSoundTankFill;
    public bool isFilling = false;

    private void Update()
    {
        if (isFilling)
        {
            playSoundTankFill.PlayFillFuelSound();
        }
    }
}