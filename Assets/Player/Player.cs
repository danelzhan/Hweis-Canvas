using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Player : MonoBehaviour
{
    
    // avatar
    public GameObject ragdoll;

    // information
    public int playerID;
    public GameObject hand;
    public Item[] inventory;
    public int inventoryLength;
    public int health;
    public InputAction castAction;

    void Start()
    {

        // inventory
        inventory = new Item[inventoryLength];

        castAction = InputSystem.actions.FindAction("Cast");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
            
        if (inventory[0] != null && inventory[0].type == "ranged") {
            if (Input.GetButtonDown("Fire1") && castAction.ReadValue<float>() == 0) {
                //shoot();
                Debug.Log("shoot");
            }
        } // if
    }
}
