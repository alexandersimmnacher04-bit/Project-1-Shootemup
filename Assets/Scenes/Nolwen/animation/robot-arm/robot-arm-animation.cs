using JetBrains.Annotations;
using System;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class robot_arm_animation : MonoBehaviour
{


    public GameObject robo_cam01;
    public GameObject robo_cam02;
    public bool robo_cam_change = true;

        void OnTriggerEnter(Collider other)
    {
            if (Input.GetKey(KeyCode.F))
            {
                 print("F Taste gedrückt");

                if (other.gameObject.CompareTag("player"))
                {
                print("Ja es hat funktioniert");
                Debug.Log("jkhb");


                // Kamera zugriff über jeweilige Taste | Dann Spieler Input blockieren |
                // neuer Input für den Roboter Arm....

                //..
                robo_cam01.SetActive(robo_cam_change);

                // Je nach Input animation ausführen

            }
        }

    }
}
//    }
//}




