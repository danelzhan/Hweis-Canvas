using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    public Player player;
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent<ItemObject>(out ItemObject touchedItem)) {
            other.gameObject.SetActive(false); 
            player.hand.GetComponent<SpriteRenderer>().sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;

            for (int i = 0; i < player.inventoryLength; i++) {
                if (player.inventory[i] == null) {
                    player.inventory[i] = other.gameObject.GetComponent<ItemObject>().getItem();
                    break;
                } // if
            } // for

            for (int i = 0; i < player.inventoryLength; i++) {
                if (player.inventory[i] != null) {
                    Debug.Log(player.inventory[i].name);
                }
            }
  
        }
    }

//     private void OnCollisionExit2D(Collision2D other) {

//     }
}
