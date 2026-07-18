using Unity.Mathematics;
using UnityEngine;

public class RoboterArmController : MonoBehaviour
{
    [Header("Gelenke des Roboters")]
    [SerializeField] private Transform[] gelenke;
    [SerializeField] private GelenkList[] gelenkAchsen;
    [SerializeField] private GelenkGruppe[] gruppen;

    [System.Serializable]
    public class GelenkGruppe
    {
        public int[] gelenkIndized;
    }

    [System.Serializable]
    public class GelenkList
    {
        public GelenkAchse[] achsen;
    }

    [Header("Rotation Speeds")]
    [SerializeField] private float speed = 20f;

    private int currentgelenkIndex = 0;

    [SerializeField] private PlayerInputHandler input;
    [SerializeField] private CameraManager cameraManager;

    private void Update()
    {
        if (!cameraManager.IncamMode)
            return;

        HandleGelenkSelection();
        HandleGelenkMovement();
    }

    private void HandleGelenkSelection()
    {
        if (input.NextGelenk)
        {
            currentgelenkIndex++;

            if (currentgelenkIndex >= gelenke.Length)
                currentgelenkIndex = 0;
        }
    }
    private void HandleGelenkMovement()
    {
        Transform gelenk = gelenke[currentgelenkIndex];

       foreach (var achse in gelenkAchsen[currentgelenkIndex].achsen)
       {
            switch (achse)
            {
                case GelenkAchse.NeigenX:

                    if (input.MoveForward)
                        RotateWithCollisionCheck(gelenk, Vector3.right, speed);

                    if (input.MoveBackward)
                        RotateWithCollisionCheck(gelenk, Vector3.right * -1f, speed);
                    break;

                case GelenkAchse.DrehenY:
                    
                    if (input.MoveRight)
                        RotateWithCollisionCheck(gelenk, Vector3.up, speed);

                    if(input.MoveLeft)
                        RotateWithCollisionCheck(gelenk, Vector3.up * -1f, speed);
                    break;

                case GelenkAchse.KippenZ:

                    if (input.MoveRight)
                        RotateWithCollisionCheck(gelenk, Vector3.forward, speed);

                    if (input.MoveLeft)
                        RotateWithCollisionCheck(gelenk, Vector3.forward * -1f, speed);
                    break;



            }

       }
    }

    public enum GelenkAchse
    {
        DrehenY,
        NeigenX,
        KippenZ
    }
    
    private float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
    
    private bool CheckCollision (Transform gelenk, Vector3 direction, float distance = 0.2f)
    {
        RaycastHit hit;
        return Physics.Raycast(gelenk.position, direction, out hit, distance);
    }
    
    private void RotateWithCollisionCheck(Transform gelenk, Vector3 axis, float speed)
    {
        Vector3 direction = gelenk.TransformDirection(axis);

        bool blocked = CheckCollision(gelenk, direction);

        if (!blocked)
        {
            gelenk.Rotate(axis * speed * Time.deltaTime);
        }
    }

}
