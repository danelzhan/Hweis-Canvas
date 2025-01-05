using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ItemObject : MonoBehaviour
{
    
    private Item item;
    public int itemID;
    public string itemType;
    public string itemName;
    public int ammoCount;
    public int damage;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public InputAction castAction;

    void Start()
    {

        if (itemType == "meleeWeapon") {
            item = new MeleeWeapon(itemID, itemName, damage, "melee");
        } else if (itemType == "rangedWeapon") {
            item = new RangedWeapon(itemID, itemName, ammoCount, damage, "ranged");
        }
    }

    // Update is called once per frame

    public Item getItem() {
        return this.item;
    }

    private void shoot() {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("shot");
    }

}
