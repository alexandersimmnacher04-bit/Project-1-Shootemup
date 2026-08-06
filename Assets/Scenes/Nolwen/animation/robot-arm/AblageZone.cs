using UnityEngine;

public class AblageZone : MonoBehaviour
{
    public Transform snapPunkt;

    public bool raeselSolved = false;

    public void RegistriereAblage(GameManager abgelegtesObjekt)
    {
        raeselSolved = true;
    }
}
