using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour {

    public GameObject ragdoll;
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 7) {
            ragdoll.GetComponent<Ragdoll>().isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.layer == 7) {
            ragdoll.GetComponent<Ragdoll>().isGrounded = false;
        }
    }
}
