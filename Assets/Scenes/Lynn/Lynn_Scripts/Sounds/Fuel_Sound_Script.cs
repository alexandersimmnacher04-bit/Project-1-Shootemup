using UnityEngine;

public class Fuel_Sound_Script : MonoBehaviour
{
    public Play_Sound_Tank_Fill playSoundTankFill;
    public bool isFilling = false;

    private void Start()
    {
        while (isFilling == true)
        {
            playSoundTankFill.PlayFillFuelSound();
        }
    }
}
