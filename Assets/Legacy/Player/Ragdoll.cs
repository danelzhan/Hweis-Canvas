using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ragdoll : MonoBehaviour
{
    
    public Muscle[] muscles;

    public Rigidbody2D rbLeft;
    public Rigidbody2D rbRight;
    private float moveDelayPointer;
    public float moveDelay;
    public Vector2 moveDirection;
    public float movementForceMultiplier = 7;
    public float secondStepMultiplier = 0;
    public float jumpMultiplier;
    private Vector2 jump;
    public InputAction moveAction;
    public InputAction jumpAction;
    public bool isGrounded = true;

    void Start()
    {
        
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        jump = new Vector2(0, jumpMultiplier);
    }

    // muscle work
    void FixedUpdate()
    {

        foreach (Muscle muscle in muscles) {
            muscle.activateMuscle();
        }

        // movement left-right
        moveDirection = moveAction.ReadValue<Vector2>();
        if (moveDirection.x < 0) {
            while (Time.time > moveDelayPointer) {
                moveLeft1();
                Invoke("moveLeft2", 0.0855f);
                moveDelayPointer = Time.time + moveDelay;
            }
        } else if (moveDirection.x > 0) {
            while (Time.time > moveDelayPointer) {
                moveRight1();
                Invoke("moveRight2", 0.0855f);
                moveDelayPointer = Time.time + moveDelay;
            }
        }

        // jump
        if (jumpAction.IsPressed() && isGrounded) {
            rbLeft.AddForce(jump, ForceMode2D.Impulse);
            rbRight.AddForce(jump, ForceMode2D.Impulse);
        }

    }

    // walking helpers
    private void moveLeft1() {
        rbLeft.AddForce(moveDirection * movementForceMultiplier, ForceMode2D.Impulse);
        rbRight.AddForce(moveDirection * -secondStepMultiplier * movementForceMultiplier, ForceMode2D.Impulse);
    }

    public void moveLeft2() {
        rbRight.AddForce(moveDirection * movementForceMultiplier * 2.5f, ForceMode2D.Impulse);
        rbLeft.AddForce(moveDirection * -secondStepMultiplier * movementForceMultiplier, ForceMode2D.Impulse);
    }

    private void moveRight1() {
        rbRight.AddForce(moveDirection * movementForceMultiplier, ForceMode2D.Impulse);
        rbLeft.AddForce(moveDirection * -secondStepMultiplier * movementForceMultiplier, ForceMode2D.Impulse);
    }

    public void moveRight2() {
        rbLeft.AddForce(moveDirection * movementForceMultiplier * 2.5f, ForceMode2D.Impulse);
        rbRight.AddForce(moveDirection * -secondStepMultiplier * movementForceMultiplier, ForceMode2D.Impulse);

    }
}

