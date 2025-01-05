using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class SpellRecognition {

    public static string spellFilePath = "Assets/Drawing/spells.txt";

    public static int maxInputs = 10;
    public static int numSpells = 30;
    public static Vector2[,] allSpells;
    public static string[] spellNames = {"blue", "red", "infinity"};


    public static void Start() {
        allSpells = new Vector2[numSpells, maxInputs];

        parseSpells();

        // for (int spellIndex = 0; spellIndex < numSpells; spellIndex++) {
        //     for (int vectorIndex = 0; vectorIndex < maxInputs; vectorIndex++) {
        //         Debug.Log(allSpells[spellIndex, vectorIndex]);

        //     }
        // }

    }
    
    public static void parseSpells() {


        string existingSpellData = "";
        List<string[]> vectorStrings = new List<string[]>();

        StreamReader reader = new StreamReader(spellFilePath);
        
        while (!reader.EndOfStream) {
            string spellData = reader.ReadLine();
            vectorStrings.Add(spellData.Trim().Split(", "));
        }

        reader.Close();

        for (int spellIndex = 0; spellIndex < vectorStrings.Count; spellIndex++) {
            for (int vectorIndex = 0; vectorIndex < vectorStrings[spellIndex].Length; vectorIndex++) {
                string[] vectorString = new string[2];

                vectorString = vectorStrings[spellIndex][vectorIndex].Replace("(", "").Replace(")", "").Split(",");
                allSpells[spellIndex, vectorIndex] = new Vector2(float.Parse(vectorString[0]), float.Parse(vectorString[1]));

            }
        }

    }

    public static void trainSpell(List<Vector2> allPoints) {

        int interval = getInterval(allPoints.Count);

        List<Vector2> unitVectors = new List<Vector2>();
        unitVectors = getUnitVectors(allPoints, interval);  


        string existingSpellData = "";
        StreamReader reader = new StreamReader(spellFilePath);

        while (!reader.EndOfStream) {
            existingSpellData += reader.ReadLine();
            existingSpellData += "\n";
        }

        reader.Close();

        saveSpellJson(spellFilePath, unitVectors, existingSpellData);

    }

    public static string getSpell (List<Vector2> allPoints) {  
        int interval = getInterval(allPoints.Count);
        List<Vector2> unitVectors;
        unitVectors = getUnitVectors(allPoints, interval);

        // Vector2[,] testSpells;
        // testSpells = new Vector2[numSpells, maxInputs];

        // for(int i = 0; i < numSpells; i++) {
        //     for (int j = 0; j < maxInputs; j++) {
        //         Debug.Log(testSpells[i][j]);
        //     }
        // }

        float[] standardDeviations = getStandardDeviations(unitVectors, allSpells);

        int holderIndex = 0;
        for(int spellIndex = 1; spellIndex < spellNames.Length; spellIndex++) {
            if (standardDeviations[holderIndex] > standardDeviations[spellIndex]) {
                holderIndex = spellIndex;
            }
        }

        Debug.Log(spellNames[holderIndex]);
        return(spellNames[holderIndex]);

        //float[] cosValues = getCosValues(getDifferenceVectors(allPoints, interval));

        // float sum = 0f;
        // foreach(float cos in cosValues) {
        //     sum += cos;
        //     // Debug.Log(cos);
        // }
        // Debug.Log("sum" + sum);
        // Debug.Log("inputs" + cosValues.Length);
        
    }

    static int getInterval (int length) {
        return Mathf.Max((int)Mathf.Ceil((float)length / (float)maxInputs), 1);
    }

    static List<Vector2> getUnitVectors (List<Vector2> allPoints, int interval) {

        List<Vector2> unitVectors;
        unitVectors = new List<Vector2>();
        Vector2 distanceVector;
        distanceVector = new Vector2();


        for (int index = 0; index < allPoints.Count - interval; index += interval) {
            Vector2 currentPoint = allPoints[index];
            Vector2 nextPoint = allPoints[index + interval];
            distanceVector = (new Vector2(nextPoint.x - currentPoint.x, nextPoint.y - currentPoint.y));
            unitVectors.Add(distanceVector / distanceVector.magnitude);
        }

        //if (unitVectors.Count != 10) {Debug.Log("# of vectors: " + unitVectors.Count);}

        return unitVectors;

    }

    static float[] getCosValues (List<Vector2> distanceVectors) {

        float[] cosValues = new float[Mathf.Max(distanceVectors.Count - 1, 1)];

        if (distanceVectors.Count == 1) {
            cosValues[0] = 1;
            return cosValues;
        }

        for (int index = 1; index < distanceVectors.Count; index++) {
            float dotProduct = Vector2.Dot(distanceVectors[index - 1], distanceVectors[index]);
            cosValues[index - 1] = dotProduct / (distanceVectors[index - 1].magnitude * distanceVectors[index].magnitude);
        }

        return cosValues;

    }

    static float[] getStandardDeviations(List<Vector2> inputUnitVectors, Vector2[,] spellUnitVectors) {
        float[] standardDeviations = new float[spellUnitVectors.GetLength(1)];
        float holder;

        for (int spellIndex = 0; spellIndex < spellUnitVectors.GetLength(1); spellIndex++) {

            holder = 0;
            for (int vectorIndex = 0; vectorIndex < Mathf.Min(spellUnitVectors.GetLength(0), inputUnitVectors.Count); vectorIndex++) {
                holder += Mathf.Pow(inputUnitVectors[vectorIndex].x - spellUnitVectors[spellIndex, vectorIndex].x, 2);
                holder += Mathf.Pow(inputUnitVectors[vectorIndex].y - spellUnitVectors[spellIndex, vectorIndex].y, 2);
            }
            standardDeviations[spellIndex] = Mathf.Sqrt(holder/(standardDeviations.Length));

        }

        return standardDeviations;

    }

    static void saveSpellJson(string jsonFilePath, List<Vector2> unitVectors, string existingSpellData) {

        SpellData spellData;
        spellData = new SpellData(unitVectors, "name");

        if (existingSpellData != "") {
            System.IO.File.WriteAllText(jsonFilePath, existingSpellData + spellData.toString());
        } else {
            System.IO.File.WriteAllText(jsonFilePath, spellData.toString());
        }


    }

}

public class SpellData {
    
    List<Vector2> unitVectors;
    string name;

    public SpellData(List<Vector2> unitVectors, string name) {
        this.unitVectors = unitVectors;
        this.name = name;
    }

    public string toString() {
        string output = "";
        for (int i = 0; i < unitVectors.Count; i++) {

            if (i != unitVectors.Count - 1) {
                output += "(" + unitVectors[i].x + "," + unitVectors[i].y + "), ";
            } else {
                output += "(" + unitVectors[i].x + "," + unitVectors[i].y + ")";
            }

        }

        Debug.Log(output);

        return output;
    }

}

public class NeuralNetwork {

    public int numLayerNeurons = 32;
    int initialLength;
    int resultSize;
    float[] initialNeurons;
    Neuron[] stage1Neurons;
    Neuron[] stage2Neurons;
    Neuron[] results;

    public NeuralNetwork(float[] cosValues, int numSpells) {

        initialLength = cosValues.Length;
        resultSize = numSpells;
        initialNeurons = cosValues;
        stage1Neurons = new Neuron[numLayerNeurons];
        stage2Neurons = new Neuron[numLayerNeurons];
        results = new Neuron[numSpells];

        for (int index = 0; index < numLayerNeurons; index++) {
            stage1Neurons[index] = new Neuron(cosValues.Length);
            stage2Neurons[index] = new Neuron(numLayerNeurons);
        }
        
        for (int index = 0; index < numSpells; index++) {
            results[index] = new Neuron(numLayerNeurons);
        }

    }

    float[] getNegativeGradient (Neuron[] results, Neuron[] expectedResults) {
 
        int gradientDimension = (int) (initialLength * numLayerNeurons + Mathf.Pow(numLayerNeurons, 2) + resultSize * numLayerNeurons);

        return null;

    }

}

public class Neuron {

    float[] weights;
    float value;
    float bias;
    float randomLimit = 5.0f;

    public Neuron (int numInputs) {
        float[] weights = new float[numInputs];

        for (int index = 0; index < numInputs; index++) {
            weights[index] = Random.Range(-1 * randomLimit, randomLimit);
        }

        bias = Random.Range(-1 * randomLimit, randomLimit);

        this.weights = weights;
        this.value = 0;
    }

    public void activateNueron (float[] inputs) {

        float weightedSum = 0;

        for (int index = 0; index < inputs.Length; index++) {
            weightedSum += inputs[index] * weights[index];
        }
        weightedSum += this.bias;

        value = activationFunction(weightedSum);

    }

    float activationFunction (float sum) {
        return 1 / (1 + Mathf.Exp(sum * -1));
    }

    public void adjustWeight (int index, float newWeight) {
        this.weights[index] = newWeight;
    }

}
