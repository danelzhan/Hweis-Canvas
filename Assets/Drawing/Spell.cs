using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spell : MonoBehaviour
{

    public Transform player;

    // infinity
    public GameObject infinity;
    public float infinityDuration;
    bool infinityCasting;
    float infinityTimeHolder;

    // blue
    public GameObject blue;
    public Transform firePoint;

    // red
    public GameObject red;
    

    GameObject spell;


    public void spellCast(GameObject[] allLines) {

        List<Vector2> allPoints;
        allPoints = new List<Vector2>();

        foreach (GameObject line in allLines) {
            foreach(Vector2 point in line.GetComponent<Line>().getPoints()) {
                allPoints.Add(point);
            }
        }

        string spellName = SpellRecognition.getSpell(allPoints);

        if (spellName == "infinity") {
            castInfinity();
        } else if (spellName == "blue") {
            castBlue();
        } else if (spellName == "red") {
            castRed();
        }

    }

    void castInfinity() {
        spell = Instantiate(infinity, player);
        infinityTimeHolder = Time.time + infinityDuration;
        infinityCasting = true;
    }

    void castBlue() {
        Instantiate(blue, firePoint.position, firePoint.rotation);
    }

    void castRed() {
        Instantiate(red, firePoint.position, firePoint.rotation);
    }


    public void spellTrain(GameObject[] allLines) {
        
        List<Vector2> allPoints;
        allPoints = new List<Vector2>();

        foreach (GameObject line in allLines) {
            foreach(Vector2 point in line.GetComponent<Line>().getPoints()) {
                allPoints.Add(point);
            }
        }

        SpellRecognition.trainSpell(allPoints);

    }

    void FixedUpdate() {
        if (infinityCasting && Time.time > infinityTimeHolder) {
            spell.SetActive(false);
            infinityCasting = false;
        }
    }

}
