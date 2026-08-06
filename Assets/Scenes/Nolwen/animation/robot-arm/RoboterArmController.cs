
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RoboterArmController : MonoBehaviour
{
    #region InspectorFelder/enums/Daten
    // Enum: Ein Datentp mit festen Wort-Optionen
    // Legt hier fest welche Bewegungstp-Optionenen ein Gelenk im Inspectorhaben kann
    public enum GelenkAchse
    {
        DrehenY,
        NeigenX,
        KippenZ,
        SchieneZ
    }

    // gelenkIndizes: Welche Gelnk IDs gehören zu einer Gruppe
    // richtungen: Soll ein Gelenk in einer Gruppe umgedreht drehen
    // steuerungMitLinksRechts: Schaltet um ob eine gruppe Links/rechts oder oben/unten für die Steuerung nutzen soll.
    [System.Serializable]
    public class GelenkGruppe
    {
        public int[] gelenkIndizes;
        public float[] richtungen;
        public bool steuerungMitLinksRechts = false;
    }

    // Dient dazu das ein Gelenk im Inspector mehrere Achsen haben kann
    [System.Serializable]
    public class GelenkList
    {
        public GelenkAchse[] achsen;
    }

    // Speichert für alle Gelenke deren Transformkomponenten und AchsenKonfigurationen
    [Header("Gelenke des Roboters")]
    [SerializeField] private Transform[] gelenke;
    [SerializeField] private GelenkList[] gelenkAchsen;
    [SerializeField] private GelenkGruppe[] gruppen;

    //Bewegungsgeschwindogkeit der Achsen
    [Header("Rotation Speeds / Movement Speeds")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float schienenSpeed = 2f;

    //Winkelgrenzen der Gelenke
    [Header("Winkellimits für alle Gelenke")]
    [SerializeField] private float minWinkel = -80f;
    [SerializeField] private float maxWinkel = 80f;

    //Layer fürs Blockieren von Bewegungen
    //AbstandsPuffer 
    [Header("Kollisionsschutz")]
    [SerializeField] private LayerMask hindernisLayer;
    [SerializeField] private float kollisionsMargin;

    //Layer für die Schienenbegrezung
    //Collider des Roboterarms für die Schiene
    [Header("Schienen Begranzung")]
    [SerializeField] private LayerMask schienenbegrenzungsLayer;
    [SerializeField] private BoxCollider basisCollider;

    //PlayerImputHandler für die Steuerung
    //CameraManager für den Kamera Wechsel
    [Header("Referenzen")]
    [SerializeField] private PlayerInputHandler input;
    [SerializeField] private CameraManager cameraManager;
    
    private int currentGroupIndex = 0;
    private float[] aktuelleWinkel;
    private Collider[] armCollider;
    #endregion
    #region Lebenszklus
    private void Start()
    {
        currentGroupIndex = 0;
        aktuelleWinkel = new float[gelenke.Length];
        armCollider = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        if(!cameraManager.IncamMode)
        return;

        HandleDirectGroupSelection();
        HandleGroupCycling();
        HandleGroupMovement();
    }
    #endregion
    #region GruppenHandling
    //Wechsel zwischen den Gelenk Gruppen 
    private void HandleDirectGroupSelection()
    {
        if (input.SelectGroup1Triggered)
        {
            currentGroupIndex = 0;
        }
        if (input.SelectGroup2Triggered)
        {
            currentGroupIndex = 1;
        }
        if (input.SelectGroup3Triggered)
        {
            currentGroupIndex = 2;
        }
        if (input.SelectGroup4Triggered)
        {
            currentGroupIndex = 3;
        }
        if (input.SelectGroup5Triggered)
        {
            currentGroupIndex = 4;
        }
    }
    
    //Durchcyclen der Gelenkgruppen
    private void HandleGroupCycling()
    {
        if (input.NextGroupTriggered)
        {
            currentGroupIndex++;

            if (currentGroupIndex >= gruppen.Length)
                currentGroupIndex = 0;
        }
    }
    //holt die aktive Gruppe
    //Geht die gleneke der Gruppe druch und übergint Richtungsmodifakor und Steuerungstyp
    private void HandleGroupMovement()
    {
        GelenkGruppe gruppe = gruppen[currentGroupIndex];

        for (int i = 0; i < gruppe.gelenkIndizes.Length; i++)
        {
            int gelenkIndex = gruppe.gelenkIndizes[i];

            float richtung = 1f;

            if(gruppe.richtungen != null && i < gruppe.richtungen.Length)
            {
                richtung = gruppe.richtungen[i];
            }

            HandleMovementForGelenk(gelenkIndex, richtung, gruppe.steuerungMitLinksRechts);
        }
    }
    #endregion
    #region HandleMovementForGelenk
    //Index99 leitet an die Schienenlogik weiter
    //Prüft welche Tasten gedückt wurden / Sonderfall steuerungMitLinksRechts
    //Je nach Achse wird außerdem RoteteWithLimit  oder RotateWithoutLimit aufgerufen
    private void HandleMovementForGelenk(int gelenkIndex, float richtungsmodifikator, bool nutzeLinksRechts)
    {

        if (gelenkIndex == 99)
        {
            HandleSchienenBewegung();
            return;
        }

        if (gelenkIndex < 0 || gelenkIndex >= gelenke.Length)
            return;

        if (gelenkIndex >= gelenkAchsen.Length)
            return; 

        Transform gelenk = gelenke[gelenkIndex];

       foreach (var achse in gelenkAchsen[gelenkIndex].achsen)
       {

            bool movePlus = nutzeLinksRechts ? input.MoveRight : (achse == GelenkAchse.NeigenX ? input.MoveForward : input.MoveRight);
            bool moveMinus = nutzeLinksRechts ? input.MoveLeft : (achse == GelenkAchse.NeigenX ? input.MoveBackward : input.MoveLeft);

            switch (achse)
            {
                case GelenkAchse.NeigenX:
                    if (movePlus)
                        RotateWithLimit(gelenk, Vector3.right, 1f * richtungsmodifikator, gelenkIndex);

                    if (moveMinus)
                        RotateWithLimit(gelenk, Vector3.right, -1f * richtungsmodifikator, gelenkIndex);
                    break;

                case GelenkAchse.DrehenY:
                    if (movePlus)
                        RotateWithoutLimit(gelenk, Vector3.up, 1f * richtungsmodifikator);

                    if (moveMinus)
                        RotateWithoutLimit(gelenk, Vector3.up, -1f * richtungsmodifikator);
                    break;

                case GelenkAchse.KippenZ:
                    if (movePlus)
                        RotateWithLimit(gelenk, Vector3.forward, 1f * richtungsmodifikator, gelenkIndex);

                    if (moveMinus)
                        RotateWithLimit(gelenk, Vector3.forward, -1f * richtungsmodifikator, gelenkIndex);
                    break;
            }
        }
    }
    #endregion
    #region RotationsHandling
    //Dreht das Gelenk, prüft mit IstKollisionVorhanden ob der arm nach der Drehung in einem Hindernis stecken würde und setzt bei einem Hindernis den den wert im Frame um den selben wert wieder zurück. 
    private void RotateWithoutLimit(Transform gelenk, Vector3 axis, float direction)
    {
        float delta = direction * speed * Time.deltaTime;

        gelenk.Rotate(axis, delta, Space.Self);

       if (IstKollisionVorhanden())
        {
            gelenk.Rotate(axis , -delta, Space.Self);
            Physics.SyncTransforms();
        }
    }

    //Rotation mit Winkelbegrenzung, macht ebenfalls den KollisionCheck
    private void RotateWithLimit(Transform gelenk, Vector3 axis, float direction, int gelenkIndex)
    {
        
        float delta = direction * speed * Time.deltaTime;
        float zielWinkel = Mathf.Clamp(aktuelleWinkel[gelenkIndex] + delta, minWinkel, maxWinkel);
        float tatsaechlicheAenderung = zielWinkel - aktuelleWinkel[gelenkIndex];

        if (Mathf.Abs(tatsaechlicheAenderung) > 0.001f)
        {
            gelenk.Rotate(axis, tatsaechlicheAenderung, Space.Self);
            
            if (IstKollisionVorhanden())
            {
                gelenk.Rotate(axis, -tatsaechlicheAenderung, Space.Self);
                Physics.SyncTransforms();
            }
            else
            {
                aktuelleWinkel[gelenkIndex] = zielWinkel;
            }
        }
    }
    #endregion
    #region SchienenBewegung
    //Der Roboterarm wird nur auf der X undY Achse bewegt
    //Hier wird wieder auf Kollision überprüft
    private void HandleSchienenBewegung()
    {
        float moveZ = 0f;
        float moveX = 0f;

        if (input.MoveForward) moveZ = 1f;
        if (input.MoveBackward) moveZ = -1f;
        if (input.MoveRight) moveX = 1f;
        if (input.MoveLeft) moveX = -1f;

        Vector3 altePos = transform.position;

        if (moveZ != 0f)
        {
            transform.position += new Vector3(0f, 0f, moveZ * schienenSpeed * Time.deltaTime);
            if (IstKollisionVorhanden() || IstSchienenBegrenzungErreicht())
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, altePos.z);
            }
        }

        Vector3 posNachZ = transform.position;
        if (moveX != 0f)
        {
            transform.position += new Vector3(moveX * schienenSpeed * Time.deltaTime, 0f, 0f);
            if (IstKollisionVorhanden() || IstSchienenBegrenzungErreicht())
            {
                transform.position = new Vector3(posNachZ.x, transform.position.y, transform.position.z);
            }
        }
    }
    #endregion
    #region IstKollisionVorhanden
    //Kollision wird berprüft
    //Physics.SyncTransform(): aktualisiert alle Objectpositionen im Physik System
    //Mit TransformVector() wird die größe der Box Collider Ermittelt
    //Phsics.CheckBox(): Macht eine PrüfBox wenn diese ein Objekt mit Hindernis Layer berührt gibt diese true zurück.
    private bool IstKollisionVorhanden()
    {
        Physics.SyncTransforms();

        foreach (Collider col in armCollider)
        {
            if (col == null || col.isTrigger) continue;

            if (col is BoxCollider box)
            {
                Vector3 center = box.transform.TransformPoint(box.center);

                Vector3 trueScale = new Vector3
                    (
                    box.transform.TransformVector(Vector3.right).magnitude,
                    box.transform.TransformVector(Vector3.up).magnitude,
                    box.transform.TransformVector(Vector3.forward).magnitude
                    );

                Vector3 halfExtents = Vector3.Scale(box.size * 0.5f, trueScale);

                halfExtents -= Vector3.one * kollisionsMargin;
                halfExtents = Vector3.Max(halfExtents, Vector3.one * 0.001f);

                if (Physics.CheckBox(center, halfExtents, col.transform.rotation, hindernisLayer, QueryTriggerInteraction.Ignore))
                {
                    return true;
                }
            }
        }

        return false;
    }
    #endregion
    #region IstSchienenBegrenzungErreicht
    //Hier passiert in etwa das gleiche wie bei IstKollisionVorhanden
    private bool IstSchienenBegrenzungErreicht()
    {
        if (basisCollider == null)
            return false;

        Physics.SyncTransforms();

        Vector3 center = basisCollider.transform.TransformPoint(basisCollider.center);

        Vector3 trueScale = new Vector3
        (
            basisCollider.transform.TransformVector(Vector3.right).magnitude,
            basisCollider.transform.TransformVector(Vector3.up).magnitude,
            basisCollider.transform.TransformVector(Vector3.forward).magnitude
        );

        Vector3 halfExtents = Vector3.Scale(basisCollider.size * 0.5f, trueScale);

        return Physics.CheckBox(center, halfExtents, basisCollider.transform.rotation, schienenbegrenzungsLayer, QueryTriggerInteraction.Ignore);
    }
    #endregion
}
