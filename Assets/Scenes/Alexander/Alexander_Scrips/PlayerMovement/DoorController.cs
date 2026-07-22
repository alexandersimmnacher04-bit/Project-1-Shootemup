using UnityEngine;

public class DoorController : MonoBehaviour
{
    //openAngel gibt an wie weit die türen aufgehen sollen
    //speed wie schenell die Tür rotieren soll
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float speed = 3f;

    //Quanternion spechert die Rotation der Tür wenn sie offen bzw. zu ist.
    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    //closedRot ist die positin zu welcher die Tür zurück ghet wenn sie geschlossen ist
    //Quanternion.Euler() erszeugt eine Rotation aus dem Euler-Winkel
    private void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    //Wenn die Tür offen war schließt diese wieder
    //Falls die Tür sich gerade Dreht wird dies gestoppt
    //startet duie animation der Tür
    public void Interact()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor());
    }
    //Ist leer wird nicht verwendet aber vielleicht später mal
    public void InteractSecondary() { }

    //Guckt ob die Tür offen oder geschlossen ist und guckt dann ob die Tür zu oder offen gedreht werden soll
    //Mist den Winkel zwischen den Rotationen
    //Macht eine Samfte Rotation und stellt am ende den Endzustand her
    private System.Collections.IEnumerator RotateDoor()
    {

        Quaternion target = isOpen ? openRot : closedRot;

        while (Quaternion.Angle(transform.rotation, target) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * speed);
            yield return null;
        }

        transform.rotation = target;
    }
}

//Code muss nicht so komplzierts sein entwas verreinfachen wäre gut hier muss nur auf und zu gehen diese ganze scheiße drumherum ist unnötig
