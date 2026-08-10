using JetBrains.Annotations;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera robo_cam01;
    [SerializeField] private Camera robo_cam02;
    [SerializeField] private GameObject camfilter;

    public bool IncamMode { get; private set; } = false;
    // Hier wird gestzt auf welcher Kamera der Spieler gerde zugreift.
    public void SwitchToPlayer()
    {
        IncamMode = false;
        playerCamera.gameObject.SetActive(true);
        robo_cam01.gameObject.SetActive(false);
        robo_cam02.gameObject.SetActive(false);
        camfilter.SetActive(false);
        
    }
    public void SwitchTo01()
    {
        IncamMode = true;
        playerCamera.gameObject.SetActive(false);
        robo_cam01.gameObject.SetActive(true);
        robo_cam02.gameObject.SetActive(false);
        camfilter.SetActive(true);
    }
    public void SwitchTo02()
    {
        IncamMode = true;
        playerCamera.gameObject.SetActive(false);
        robo_cam01.gameObject.SetActive(false);
        robo_cam02.gameObject.SetActive(true);
        camfilter.SetActive(true);

    }
}
