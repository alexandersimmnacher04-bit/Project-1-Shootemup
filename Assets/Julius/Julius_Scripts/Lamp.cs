using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{
    public FillUpTank fillUpTank;
    private Image imageColor;
    private void Awake()
    {
       imageColor = GetComponentInChildren<Image>();
    }
    public void Colorchange()
    {
       imageColor.color= Color.red; 
    }
}

