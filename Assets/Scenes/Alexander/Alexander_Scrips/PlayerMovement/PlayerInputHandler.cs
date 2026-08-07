using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Gewährt zugriff auf das Unit Input System.
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
public class PlayerInputHandler : MonoBehaviour
{
    private InteractionZone currentZone;
    [SerializeField] FirstPersonController firstPersonController;

    public void SetCurrentZone(InteractionZone zone) => currentZone = zone;
    //InputActionAsset: Ist der name des Tpy der Variable für Unity
    //playerControls: Ist der Name der Variable
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;
    [SerializeField] private InputActionAsset robotControls;

    //string: Der Typ ist ein string also ein Text.
    //actionMapName: Ist der Name der Variable.
    //"Player": ist der Standartwert welcher zum Start gesetzt wird.
    [Header("Action Map Name Referance")]
    [SerializeField] private string playeractionMapName = "Player";
    [SerializeField] private string robotactionMapName = "RoboterArm";

    [Header("Player Actions")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string interactsecondary = "Interactsecondary";
    [SerializeField] private string escape = "Escape";

    [Header("Roboter Actionen")]
    [SerializeField] private string moveforward = "moveforward";
    [SerializeField] private string movebackward = "movebackward";
    [SerializeField] private string moveright = "moveright";
    [SerializeField] private string moveleft = "moveleft";

    [SerializeField] private string selectgroup1 = "selectgroup1";
    [SerializeField] private string selectgroup2 = "selectgroup2";
    [SerializeField] private string selectgroup3 = "selectgroup3";
    [SerializeField] private string selectgroup4 = "selectgroup4";
    [SerializeField] private string selectgroup5 = "selectgroup5";
    [SerializeField] private string nextgroup = "nextgroup";


    //InputAction: Ist ein Object aus dem Unity Input System, das eine einzelne Eingabe repräsentiert.
    //movementAction: Speichert die Movement-Action aus dem Asset
    //rotationAction: Speichert die Rotation-Action
    //ect.
    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction interactAction;
    private InputAction interactsecondaryAction;
    private InputAction escapeAction;

    private InputAction moveforwardAction;
    private InputAction movebackwardAction;
    private InputAction moverightAction;
    private InputAction moveleftAction;

    private InputAction selectgroup1Action;
    private InputAction selectgroup2Action;
    private InputAction selectgroup3Action;
    private InputAction selectgroup4Action;
    private InputAction selectgroup5Action;
    private InputAction nextgroupAction;

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
    public bool InteractSecondaryTriggered { get; private set; }
    public bool EscapeTriggered { get; private set; }


    public bool MoveForward => moveforwardAction.IsPressed();
    public bool MoveBackward => movebackwardAction.IsPressed();
    public bool MoveRight => moverightAction.IsPressed();
    public bool MoveLeft => moveleftAction.IsPressed();

    public bool SelectGroup1Triggered { get; private set; }
    public bool SelectGroup2Triggered { get; private set; }
    public bool SelectGroup3Triggered { get; private set; }
    public bool SelectGroup4Triggered { get; private set; }
    public bool SelectGroup5Triggered { get; private set; }
    public bool NextGroupTriggered { get; private set; }



    //Awake(): Ist eine Unity-Lebenszklusmethode. Wird einmal aufgerufen sobald das Script geladen wird, bevor Start() ausgeführt wird.
    //FindActionMap(actionMapName: Holt die Action Map "Player" aus dem Asset.
    //FindAction(): Holt die Actions Movement, Rotation ect. aus der Action Map.
    //SubscribeActionValuesToInputEvents(): Verbindet die Actions mit den Events (performed, canceled / Klasse private void SubscribeActionValuesToInputEvents() (L.68-81)).
    private void Awake()
    {
        InputActionMap playermapReference = playerControls.FindActionMap(playeractionMapName);
        InputActionMap robotmapReference = robotControls.FindActionMap(robotactionMapName);

        movementAction = playermapReference.FindAction(movement);
        rotationAction = playermapReference.FindAction(rotation);
        jumpAction = playermapReference.FindAction(jump);
        sprintAction = playermapReference.FindAction(sprint);
        interactAction = playermapReference.FindAction(interact);
        interactsecondaryAction = playermapReference.FindAction(interactsecondary);
        escapeAction = playermapReference.FindAction(escape);

        moveforwardAction = robotmapReference.FindAction(moveforward);
        movebackwardAction = robotmapReference.FindAction(movebackward);
        moverightAction = robotmapReference.FindAction(moveright);
        moveleftAction = robotmapReference.FindAction(moveleft);

        selectgroup1Action = robotmapReference.FindAction(selectgroup1);
        selectgroup2Action = robotmapReference.FindAction(selectgroup2);
        selectgroup3Action = robotmapReference.FindAction(selectgroup3);
        selectgroup4Action = robotmapReference.FindAction(selectgroup4);
        selectgroup5Action = robotmapReference.FindAction(selectgroup5);
        nextgroupAction = robotmapReference.FindAction(nextgroup);

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

        interactsecondaryAction.performed += inputInfo => InteractSecondaryTriggered = true;
        interactsecondaryAction.canceled += inputInfo => InteractSecondaryTriggered = false;

        escapeAction.performed += inputInfo => EscapeTriggered = true;
        escapeAction.canceled += inputInfo => EscapeTriggered = false;


        selectgroup1Action.performed += inputInfo => SelectGroup1Triggered = true;
        selectgroup1Action.canceled += inputInfo => SelectGroup1Triggered = false;

        selectgroup2Action.performed += inputInfo => SelectGroup2Triggered = true;
        selectgroup2Action.canceled += infoInfo => SelectGroup2Triggered = false;

        selectgroup3Action.performed += inputInfo => SelectGroup3Triggered = true;
        selectgroup3Action.canceled += inputInfo => SelectGroup3Triggered = false;

        selectgroup4Action.performed += inputInfo => SelectGroup4Triggered = true;
        selectgroup4Action.canceled += inputInfo => SelectGroup4Triggered = false;

        selectgroup5Action.performed += inputInfo => SelectGroup5Triggered = true;
        selectgroup5Action.canceled += inputInfo => SelectGroup5Triggered = false;

        nextgroupAction.performed += inputInfo => NextGroupTriggered = true;
        nextgroupAction.canceled += inputInfo => NextGroupTriggered = false;
    }
    private void Start()
    {
        EnablePlayerInput();
    }
    //Late Update muss gemacht werden weil bei Update jeder frame getrakt wird uns der Knopf mehere Frames gehalten wird das sorgt für ständiges hin und her springen bei der Kamera.
    private void LateUpdate()
    {

        if (InteractTriggered && currentZone != null)
        {
            currentZone.TriggerPrimary();
        }
        if (InteractSecondaryTriggered && currentZone != null)
        {
            currentZone.TriggerSecondary();
        }
        

        InteractTriggered = false;
        InteractSecondaryTriggered = false;

        SelectGroup1Triggered = false;
        SelectGroup2Triggered = false;  
        SelectGroup3Triggered = false;
        SelectGroup4Triggered = false;
        SelectGroup5Triggered = false;
        NextGroupTriggered = false;
    }

    public void SwitchToRobotMode()
    {
        EnableRobotInput();
    }

    public void SwitchToPlayerMode()
    {
        EnablePlayerInput();
    }
    //OnEnable(): Ist auch eine Unity-Lebenszyklusmethode, diese wird auf gerufen wenn das Game Object, Script oder Szene aktiviert wird.
    //.Enable(): aktiviert die Action Map.
    private void EnablePlayerInput()
    {
        playerControls.FindActionMap(playeractionMapName).Enable();
        robotControls.FindActionMap(robotactionMapName).Disable();
    }

    //OnDisable(): Ist ebenfalls eine Unity-Lebenszyklusmethode, diese wird auf gerufen wenn das Game Object, Script oder Szene deaktiviert wird. (Gegenstück zu OnEnable())
    //.Disable(): deaktiviert die Action Map.
    private void EnableRobotInput()
    {
        robotControls.FindActionMap(robotactionMapName).Enable();
    }
   
}
