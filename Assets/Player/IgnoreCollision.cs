using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{

    Rigidbody rb;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        Physics2D.IgnoreLayerCollision(6,6);
        Physics2D.IgnoreLayerCollision(6,9);
    }

}
