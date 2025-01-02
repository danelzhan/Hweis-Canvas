using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LineGenerator : MonoBehaviour
{

    public GameObject linePrefab;
    public InputAction castAction;
    public InputAction trainAction;
    public Transform spell;
    
    Line activeLine;

    bool isCasting;

    void Start()
    {
        castAction = InputSystem.actions.FindAction("Cast");
        trainAction = InputSystem.actions.FindAction("Train");
        SpellRecognition.Start();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && castAction.ReadValue<float>() == 1) {
            GameObject newLine = Instantiate(linePrefab, spell);
            activeLine = newLine.GetComponent<Line>();
            isCasting = true;
        }

        if (Input.GetButtonUp("Fire1")) {
            activeLine = null;
        }

        if (castAction.ReadValue<float>() == 0 && isCasting) {
            GameObject[] allLines = GameObject.FindGameObjectsWithTag("Line");
            spell.gameObject.GetComponent<Spell>().spellCast(allLines); 
            foreach (GameObject line in allLines) {
                Destroy(line);
            }
            isCasting = false;
        }

        // training
        if (trainAction.ReadValue<float>() == 1 && isCasting) {
            GameObject[] allLines = GameObject.FindGameObjectsWithTag("Line");
            spell.gameObject.GetComponent<Spell>().spellTrain(allLines);
            isCasting = false;
            foreach (GameObject line in allLines) {
                Destroy(line);
            }
        }



        if (activeLine != null) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.updateLine(mousePosition);
        }

    }
}
