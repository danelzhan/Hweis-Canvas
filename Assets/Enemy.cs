using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth;
    public int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void looseHealth(int healthLost) {
        health -= healthLost;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }


}
