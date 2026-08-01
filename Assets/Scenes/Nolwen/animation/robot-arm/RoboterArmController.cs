
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    [SerializeField] private float kollisionsPruefRadius = 0.15f;

    [Header("Referenzen")]
    [SerializeField] private PlayerInputHandler input;
    [SerializeField] private CameraManager cameraManager;

    private int currentGroupIndex = 0;
    private SchienenModus currentSchienenModus = SchienenModus.HangarSchiene;
    private float[] aktuelleWinkel;

    private void Start()
    {
        currentGroupIndex = 0;
        aktuelleWinkel = new float[gelenke.Length];
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

        if (other.CompareTag("HangarSchiene"))
            currentSchienenModus = SchienenModus.HangarSchiene;

        if (other.CompareTag("Junction"))
            currentSchienenModus = SchienenModus.Junction;

        if (other.CompareTag("LagerSchiene"))
            currentSchienenModus = SchienenModus.LagerSchiene;
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
        foreach (int gelenkIndex in gruppen[currentGroupIndex].gelenkIndizes)
        {
            HandleMovementForGelenk(gelenkIndex);
        }
    }
    private void HandleMovementForGelenk(int gelenkIndex)
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
            switch (achse)
            {
                case GelenkAchse.NeigenX:

                    if (input.MoveForward)
                        RotateWithLimit(gelenk, Vector3.right, 1f, gelenkIndex);

                    if (input.MoveBackward)
                        RotateWithLimit(gelenk, Vector3.right, -1f, gelenkIndex);
                    break;

                case GelenkAchse.DrehenY:
                    
                    if (input.MoveRight)
                        RotateWithoutLimit(gelenk, Vector3.up, 1f);

                    if(input.MoveLeft)
                        RotateWithoutLimit(gelenk, Vector3.up, -1f);
                    break;

                case GelenkAchse.KippenZ:

                    if (input.MoveRight)
                        RotateWithLimit(gelenk, Vector3.forward, 1f, gelenkIndex);

                    if (input.MoveLeft)
                        RotateWithLimit(gelenk, Vector3.forward, -1f, gelenkIndex);
                    break;

            }
       }
    }

    private void RotateWithoutLimit(Transform gelenk, Vector3 axis, float direction)
    {
        if (IstWandImWeg(gelenk))
            return;

        gelenk.Rotate(axis, direction * speed * Time.deltaTime, Space.Self);
    }

    private void RotateWithLimit(Transform gelenk, Vector3 axis, float direction, int gelenkIndex)
    {
        if (IstWandImWeg(gelenk))
            return;

        float delta = direction * speed * Time.deltaTime;
        float zielWinkel = Mathf.Clamp(aktuelleWinkel[gelenkIndex] + delta, minWinkel, maxWinkel);
        float tatsaechlicheAenderung = zielWinkel - aktuelleWinkel[gelenkIndex];

        if (Mathf.Abs(tatsaechlicheAenderung) > 0.001f)
        {
            gelenk.Rotate(axis, tatsaechlicheAenderung, Space.Self);
            aktuelleWinkel[gelenkIndex] = zielWinkel;
        }
    }

    private bool IstWandImWeg(Transform gelenk)
    {
        return Physics.CheckSphere(gelenk.position, kollisionsPruefRadius, hindernisLayer);
    }

    //private float NormalizeAngle(float angle)
    //{
    //    if (angle > 180f) angle -= 360f;
    //    return angle;
    //}

    //private bool CheckCollision(Transform gelenk, Vector3 direction, float distance = 0.2f)
    //{
    //    int mask = ~LayerMask.GetMask("RoboterArm");
    //    RaycastHit hit;
    //    return Physics.Raycast(gelenk.position, direction, out hit, distance, mask);
    //}

    //private void RotateWithCollisionCheck(Transform gelenk, Vector3 axis, float speed)
    //{
    //    Vector3 direction = gelenk.TransformDirection(axis);

    //    bool blocked = CheckCollision(gelenk, direction);

    //    if (!blocked)
    //    {
    //        gelenk.Rotate(axis * speed * Time.deltaTime);
    //    }
    //}

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

        Vector3 zielPosition = transform.position;

        switch (currentSchienenModus)
        {
            case SchienenModus.HangarSchiene:
                zielPosition.z += moveZ * schienenSpeed * Time.deltaTime;
                break;

            case SchienenModus.Junction:
                zielPosition.z += moveZ * schienenSpeed * Time.deltaTime;
                zielPosition.x += moveX * schienenSpeed * Time.deltaTime;
                break;

            case SchienenModus.LagerSchiene:
                zielPosition.x += moveX * schienenSpeed * Time.deltaTime;
                break;
        }

        if(!Physics.CheckSphere(zielPosition, kollisionsPruefRadius, hindernisLayer))
        transform.position = zielPosition;
    }
}
