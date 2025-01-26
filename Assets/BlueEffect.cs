using UnityEngine;

public class BlueEffect : MonoBehaviour
{

    public float forceMultiplier;
    public Collider2D collider;
    void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.tag == "Enemy") {
            Debug.Log("sucking");
            Vector2 enemyPosition = (Vector2) other.gameObject.transform.position;
            Vector2 bluePosition = (Vector2) gameObject.transform.position;
            Vector2 forceDirection = new Vector2(enemyPosition.x - bluePosition.x, enemyPosition.y - bluePosition.y);

            Debug.Log(forceDirection);
            

            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-1 * forceDirection * forceMultiplier, ForceMode2D.Impulse);

        }
    }

}
