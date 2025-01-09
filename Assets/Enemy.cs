using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth;
    public int health;
    public GameObject healthBar;
    public float healthBarMaxWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void looseHealth(int healthLost) {
        health -= healthLost;

        float newWidth = ((float) health / (float) maxHealth) * healthBarMaxWidth;
        Debug.Log("width: " + newWidth);
        healthBar.transform.localScale = new Vector2 (newWidth, 0.05f);

        if (health <= 0) {
            Destroy(gameObject);
        }
    }


}
