using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueProjectile : Projectile
{
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("collided");
        if (other.gameObject.tag == "Enemy") {
            
            other.gameObject.GetComponent<Enemy>().looseHealth(5);
            Destroy(gameObject);
        }
    }
}
