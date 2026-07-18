using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class FirstPersonController : MonoBehaviour
{
    //walkSpeed: Geschwindigkeit beim Laufen (Standartwert hier 3.0f).
    //sprintMultiplier: Faktor um die die Geschwindigkeit beim Sprinten erhöht wird, hier mit dem Faktor 2.0f (3.0f * 2.0f = 6.0f).
    [Header("Movment Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    //jumpForce: Wie stark der SC beim Springen nach oben geschossen wird, hier 5.0f.
    //gravityMultiplier: Wie Stark ist die Schwerkraft (Normale Unity-Schwerkraft 1.0f, >1.0f schneller Fallen, <1.0f langsamer fallen).
    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    //mouseSensitivity: Wie stark die Mausbewegungen die Kamera drehen. (Höherer Wert = schnellere Drehung).
    //upDownLookRange: Ist die Maximale vertikale Drehung (80° bedeutet 80° nach oben und 80° nach unten).
    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;

    //characterController: Ist Unitys CharacterController-Komponente ist Verantwortlich für Bewegung, Kollision, Schwerkraft.
    //mainCamera: Die Kamera aus den der Spieler sieht. Wird für die Rotation benutzt.
    //playerInputHandler: Das andere Unity Script für die SC bewegung, liefert MovementInput, RotationsInput, JumpTriggered, SprintTriggered.
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler playerInputHandler;

    //currentMovement: Speichert die Bewegung in den Vector3 Koordinaten X,Y,Z. Wird  später an characterController.Move() übergeben.
    //verticalRotation: Speichert die vertikale Kamerarotation. Wird benutzt um die Kamera hoch/runter zu drehen.
    private Vector3 currentMovement;
    private float verticalRotation;

    private bool movementBlocked = false;
    private bool cursorOn;

    public void BlockMovement(bool blocked)
    {
       movementBlocked = blocked;
    }
    public void ToggleMovement()
    {
        movementBlocked = !movementBlocked;
               
    }
    public void ToggleCursor()
    {
        cursorOn = !cursorOn;
        if (cursorOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    //Wenn Sprint gedrückt wird, wird die Geschwindigkeit, der walkSpeed * sprintMultiplier. (Geschwindigkeit = LaufGeschwindigkeit * SprintMulitiplizierer)
    //Wenn Sprint nicht gedrückt wird die Geschwindigkeit, der walkSpeed * 1. (Geschwindigkeit = LaufGeschwindigkeit * 1).
    //?: Oder
    //CurrentSpeed: Ist die automatisch berechnete Lauf bzw. Sprintgeschwindigkeit.
    private float CurrentSpeed => walkSpeed * (playerInputHandler.SprintTriggered ? sprintMultiplier : 1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Cursor.lockState = CursorLockMode.Locked: Der Mauszeiger wird in der Bildschrimmitte fixiert
    //Cursor.visible = false: Der Mauszeiger wird unsichtbar.
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame.
    //HandleMovement(): Ruft die Methode HandleMovement, jeden Frame.
    //HandleRotation(): Ruft die Methode HandleRotation, jeden Frame.
    void Update()
    {
        if (movementBlocked)
            return;

        HandleMovement();
        HandleRotation();
    }

    //MovementInput ist ein Vector2 aus dem InputHandler, dieser wird in einem Vector3 umgewandelt um disen in die inputDirection umzuwandeln.
    //Die inputDirection wird in worldDirection umgewandelt, um den SC Blickrichtung (der Kamera) bewegung nach vorne ect. zu bewegen
    //inputDirection: Wandelt WASD in lokale 3D-Richtung um
    //TransformDirection: Dreht diese Richtung in die Weltroation des Spielers.
    //normalized: Macht die Richtung einheitlich (keine Diagonal-Boosts enstehen).
    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }

    //CharacterController.isGrounded: Ist True wenn der Controller also der SC auf dem Boden ist. (Verhindert Springen wenn der SC in der Luft ist).
    //currentMovement.y = -0.5f: Der Negative Wert zieht den Sc laicht auf den Boden sonst würde dieser nicht zufällig auf dem Boden bleiben.
    //if(playyerInputHandler.JumpTriggered): Wenn der Spieler Springt, dann wird die vertikale Geschwindigkeit auf JumpForce gesetzt.
    //else: Wenn der Spieler nicht am Boden ist wird wird die Schwerkraft angewendet. (Geschwindigkeit auf der y Achse += -9.81 * 1)
    //Physics.gravity.y: Ist normalerweise -9.81. Also die Gravitation auf der Erde.
    //Time.deltaTime: Damit es unabhägig von der framerate wird.
    private void HandleJumping()
    {
        if (characterController.isGrounded)
        {

            currentMovement.y = -0.5f;

            if(playerInputHandler.JumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    //worldDirection: Ist ein Vector3 aus der Methode CalculateWorldDirection.
    //currentMovement.x: Setzt die horizontale Bewegung, links/rechts.
    //currentMovement.y: Setzt die Vorwärts und Rückwärtbewegung.
    //HandleJumping(): Ruft die Methode HandleJumping auf.
    //Move(): Wendet die Bewegung an (mit Kollision).
    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    //float rotationAmount: Der Wert, um den der Spieler gedreht werden soll. Kommt aus der Input-Verarbeitung von RotationInput.x * mouseSensitivity * Time.deltaTime.
    //transform.Rotate(0, rotationAmount, 0):dreht das Object um die angegebenden Achse, in dem fall nur um die Y-Achse (horizontale Drehung).
    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }

    //Math.Clamp(value, min, max): Berechnet die Rotation. Das .Clamp ist für das setzen der Grenzen.
    //verticalRotation - rotationAmount: Die Kamera Rotation.
    //-upDownLookRange, upDownLookRangen: Sind die Min und Max Grenzen hier -80° und 80°.
    //Quanternion.Euler(): Unity Rotationen werden intern als Quaterions gespeichert. Hier werden sie durch Euler-Winkel gesetzt.
    //(verticalRotaion, 0, 0): Die Kamera wird nur auf der x-Achse gedreht, Y und Z sind 0 damit die Kamer nicht seitlich kippt.
    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    //float mouseXRotation/mouseYRotation: die horizontale Mausbewegung wird mit mouseSensitivity multipliziert, damit man einstellen kann wie schnell sich der Spieler dreht.
    //ApplyHorizontalRotation: Dreht den Spieler um die Y-Achse. Dadurch dreht sich die Blickrichtung.
    //ApplyVerticalRotation: Dreht die Kamera um die X-Achse.
    private void HandleRotation()
    {
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }
}
