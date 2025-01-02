using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spell : MonoBehaviour
{

    public void spellCast(GameObject[] allLines) {

        List<Vector2> allPoints;
        allPoints = new List<Vector2>();

        foreach (GameObject line in allLines) {
            foreach(Vector2 point in line.GetComponent<Line>().getPoints()) {
                allPoints.Add(point);
            }
        }

        SpellRecognition.getSpell(allPoints);

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

}
