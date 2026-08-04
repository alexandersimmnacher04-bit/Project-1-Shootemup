
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RoboterArmController : MonoBehaviour
{
    public enum GelenkAchse
    {
        DrehenY,
        NeigenX,
        KippenZ,
        SchieneZ
    }

    private enum SchienenModus
    {
        HangarSchiene,
        Junction,
        LagerSchiene,
    }

    [System.Serializable]
    public class GelenkGruppe
    {
        public int[] gelenkIndizes;
        public float[] richtungen;
        public bool steuerungMitLinksRechts = false;
    }

    [System.Serializable]
    public class GelenkList
    {
        public GelenkAchse[] achsen;
    }

    [Header("Gelenke des Roboters")]
    [SerializeField] private Transform[] gelenke;
    [SerializeField] private GelenkList[] gelenkAchsen;
    [SerializeField] private GelenkGruppe[] gruppen;

    [Header("Rotation Speeds / Movement Speeds")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float schienenSpeed = 2f;

    [Header("Winkellimits für alle Gelenke")]
    [SerializeField] private float minWinkel = -80f;
    [SerializeField] private float maxWinkel = 80f;

    [Header("Kollisionsschutz")]
    [SerializeField] private LayerMask hindernisLayer;
    [SerializeField] private float kollisionsMargin;

    [Header("Referenzen")]
    [SerializeField] private PlayerInputHandler input;
    [SerializeField] private CameraManager cameraManager;

    [Header("Debug Status")]
    [SerializeField] private SchienenModus currentSchienenModus = SchienenModus.HangarSchiene;

    private int currentGroupIndex = 0;
    private float[] aktuelleWinkel;

    private Collider[] armCollider;

    private int junctionCount = 0;
    private int lagerCount = 0;
    private int hangarCount = 0;
   
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Junction"))
            junctionCount++;
        else if (other.CompareTag("LagerSchiene"))
            lagerCount++;
        else if (other.CompareTag("HangarSchiene"))
            hangarCount++;

        EvaluateSchienenModus();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Junction"))
            junctionCount = Mathf.Max(0, junctionCount - 1);
        else if (other.CompareTag("LagerSchiene")) 
            lagerCount = Mathf.Max(0, lagerCount - 1);
        else if (other.CompareTag("HangarSchiene")) 
            hangarCount = Mathf.Max(0, hangarCount -1);

        EvaluateSchienenModus();
    }

    private void EvaluateSchienenModus()
    {
        if (junctionCount > 0)
        {
            currentSchienenModus = SchienenModus.Junction;
        }
        else if (lagerCount > 0)
        {
            currentSchienenModus = SchienenModus.LagerSchiene;
        }
        else if (hangarCount > 0)
        {
            currentSchienenModus = SchienenModus.HangarSchiene;
        }
    }
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
    }
    
    private void HandleGroupCycling()
    {
        if (input.NextGroupTriggered)
        {
            currentGroupIndex++;

            if (currentGroupIndex >= gruppen.Length)
                currentGroupIndex = 0;
        }
    }

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
                    // ...und hier nutzt du sie jetzt auch!
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

    private void HandleSchienenBewegung()
    {
        float moveZ = 0f;
        float moveX = 0f;

        if (input.MoveForward)
            moveZ = 1f;

        if (input.MoveBackward)
            moveZ = -1f;

        if (input.MoveRight)
            moveX = 1f;

        if (input.MoveLeft)
            moveX = -1f;

        Vector3 altePos = transform.position;
        Vector3 neuePos = altePos;

        switch (currentSchienenModus)
        {
            case SchienenModus.HangarSchiene:
                neuePos.z += moveZ * schienenSpeed * Time.deltaTime;
                break;

            case SchienenModus.Junction:
                neuePos.z += moveZ * schienenSpeed * Time.deltaTime;
                neuePos.x += moveX * schienenSpeed * Time.deltaTime;
                break;

            case SchienenModus.LagerSchiene:
                neuePos.x += moveX * schienenSpeed * Time.deltaTime;
                break;
        }

        transform.position = neuePos;

        if (IstKollisionVorhanden())
        {
            transform.position = altePos;
            Physics.SyncTransforms();
        }
    }

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

    //    private void OnDrawGizmos()
    //    {
    //        if (armCollider == null) return;

    //        Gizmos.color = Color.red;
    //        foreach (Collider col in armCollider)
    //        {
    //            if (col is BoxCollider box && !box.isTrigger)
    //            {
    //                Vector3 center = box.transform.TransformPoint(box.center);

    //                Vector3 trueScale = new Vector3(
    //                    box.transform.TransformVector(Vector3.right).magnitude,
    //                    box.transform.TransformVector(Vector3.up).magnitude,
    //                    box.transform.TransformVector(Vector3.forward).magnitude
    //                );

    //                Vector3 halfExtents = Vector3.Scale(box.size * 0.5f, trueScale);
    //                halfExtents -= Vector3.one * kollisionsMargin;
    //                halfExtents = Vector3.Max(halfExtents, Vector3.one * 0.001f);

    //                // Zeichnet exakt die Test-Box aus Physics.CheckBox
    //                Matrix4x4 matrix = Matrix4x4.TRS(center, box.transform.rotation, Vector3.one);
    //                Gizmos.matrix = matrix;
    //                Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
    //            }
    //        }
    //    }
}
