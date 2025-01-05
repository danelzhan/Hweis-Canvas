using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocityMultiplier;
    public float jumpMultiplier;
    
    bool movementInput;
    public InputAction jumpAction;
    bool grounded;
    float previousDirection;


    void Start()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
        previousDirection = 1.0f;
    }

    void Update() {


    }

    // Update is called once per frame
    void FixedUpdate() {
        float xInput = Input.GetAxis("Horizontal");
 
        if (xInput != 0f && xInput != previousDirection) {
            transform.Rotate(0f, 180f, 0f);
            previousDirection = xInput;
        }

        Vector2 velocity = rb.linearVelocity;
        velocity.x = xInput * velocityMultiplier;
        if (jumpAction.IsPressed() && grounded) {
            velocity.y += jumpMultiplier;
        }
        rb.linearVelocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 7) {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.layer == 7) {
            grounded = false;
        }
    }

}