using UnityEngine;

[System.Serializable]
public class Item {

    public int itemID;
    public string name;
    public string type;
    
}

[System.Serializable]
public class MeleeWeapon : Item {
    public int damage;

    public MeleeWeapon(int id, string name, int damage, string type) {
        this.itemID = id;
        this.name = name;
        this.damage = damage;
        this.type = type;
    }

}

[System.Serializable]
public class RangedWeapon : Item {
    public int ammunitionCount;
    public int damage;

    public RangedWeapon(int id, string name, int ammunitionCount, int damage, string type) {
        this.itemID = id;
        this.name = name;
        this.ammunitionCount = ammunitionCount;
        this.damage = damage;
        this.type = type;
    }

}

