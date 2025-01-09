using UnityEngine;

public class Blue : Projectile
{
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("collided");
        if (other.gameObject.tag == "Enemy") {
            
            other.gameObject.GetComponent<Enemy>().looseHealth(5);
            Destroy(gameObject);
        }
    }
}
