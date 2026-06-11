using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Jump(InputAction.CallbackContext cntxt)
    {
        if (cntxt.started)
        {
            rb.AddForce(new Vector3(0, 100, 0));
        }
    }

    public void OnMovement(InputAction.CallbackContext cntxt)
    {
        moveInput = cntxt.ReadValue<Vector2>();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector3(moveInput.x, rb.linearVelocity.y,moveInput.y);
    }
}
