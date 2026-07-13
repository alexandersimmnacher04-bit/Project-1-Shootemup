using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Gewährt zugriff auf das Unit Input System.
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

//public: Die KLasse ist überall im Project zu finden. 
//class: Es wird eine neue Klasse definiert
//PlayerInputHandler: Der Name der Klasse
//MonoBehavior: Dadurch kann das Script an Objecte hängen, die Unity Engine kann spezielle Methoden direkt aufrufen, die Klasse kann mit der Unity Engne interagiern. 
public class PlayerInputHandler : MonoBehaviour
{
    //Header: Im Unity Inspector erscheinten eine Überschrift über der Variable. (Dient zur Lesbarkeit/Ordnung)
    //[SerializeField]: Die Variable bleibt Privat, trotzdem wird sie im Inspector angezeigt. Unity speichert den Wert der Variable in der Szene.
    //private: Die Variable nicht im Project nicht zu finden.
    //InputActionAsset: Ist der name des Tpy der Variable für Unity
    //playerControls: Ist der Name der Variable
    [Header("Imput Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    //string: Der Typ ist ein string also ein Text.
    //actionMapName: Ist der Name der Variable.
    //"Player": ist der Standartwert welcher zum Start gesetzt wird.
    [Header("Action Map Name Referance")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string interact = "interact";

    //InputAction: Ist ein Object aus dem Unity Input System, das eine einzelne Eingabe repräsentiert.
    //movementAction: Speichert die Movement-Action aus dem Asset
    //rotationAction: Speichert die Rotation-Action
    //ect.
    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction interactAction;

    //Property: Ist quasi wie eine Variable mit eingebauter Kontrolle.
    //Vector2: Sind die Variablen auf dem Vector2 also X = links/rechts Y = vor/zurück. Oder X = Maus horizontal = Maus vertikal.
    //bool: Eine Variable mit zwei zustanden "true" und "false"
    //get: andere Scrips dürfen den Wert lesen.
    //private set: nur dieses Script darf den Wert setzen
    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool InteractTriggered { get; private set; }

    //Awake(): Ist eine Unity-Lebenszklusmethode. Wird einmal aufgerufen sobald das Script geladen wird, bevor Start() ausgeführt wird.
    //FindActionMap(actionMapName: Holt die Action Map "Player" aus dem Asset.
    //FindAction(): Holt die Actions Movement, Rotation ect. aus der Action Map.
    //SubscribeActionValuesToInputEvents(): Verbindet die Actions mit den Events (performed, canceled / Klasse private void SubscribeActionValuesToInputEvents() (L.68-81)).
    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);
        
        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        interactAction = mapReference.FindAction(interact);

        SubscribeActionValuesToInputEvents();
    }

    //performed: Die InputAction wird mit den passenden Events verbunden wenn die Eingabe aktiv wird.
    //canceled: Doe InputAction wird mit den passenden Event verbunden wenn die Eingabe endet. Dadurch werden die Properties MovementInput und Jump Triggered ect. automatisch aktualisiert)
    //inputInfo.ReadValue<Vector2>(): Liest die Variablen des Vector2 der aktuellen Bewegung. (wird in MovementInput gespeichert)
    //Vector2.zero: MovementInput wird auf (0,0) gesetzt.
    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;

        interactAction.performed += inputInfo => InteractTriggered = true;
        interactAction.canceled += inputInfo => InteractTriggered = false;
    }

    //OnEnable(): Ist auch eine Unity-Lebenszyklusmethode, diese wird auf gerufen wenn das Game Object, Script oder Szene aktiviert wird.
    //.Enable(): aktiviert die Action Map.
    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    //OnDisable(): Ist ebenfalls eine Unity-Lebenszyklusmethode, diese wird auf gerufen wenn das Game Object, Script oder Szene deaktiviert wird. (Gegenstück zu OnEnable())
    //.Disable(): deaktiviert die Action Map.
    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}
