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
    public static string[] spellNames = {"blue", "blue", "blue", "red", "red", "red", "infinity", "infinity", "infinity", "blue", "blue", "blue"};


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

        List<Vector2> scaledVectors = new List<Vector2>();
        scaledVectors = getScaledVectors(allPoints, interval);  


        string existingSpellData = "";
        StreamReader reader = new StreamReader(spellFilePath);

        while (!reader.EndOfStream) {
            existingSpellData += reader.ReadLine();
            existingSpellData += "\n";
        }

        reader.Close();

        saveSpellJson(spellFilePath, scaledVectors, existingSpellData);

    }

    public static string getSpell (List<Vector2> allPoints) {  
        int interval = getInterval(allPoints.Count);
        List<Vector2> scaledVectors;
        scaledVectors = getScaledVectors(allPoints, interval);

        // Vector2[,] testSpells;
        // testSpells = new Vector2[numSpells, maxInputs];

        // for(int i = 0; i < numSpells; i++) {
        //     for (int j = 0; j < maxInputs; j++) {
        //         Debug.Log(testSpells[i][j]);
        //     }
        // }

        float[] standardDeviations = getStandardDeviations(scaledVectors, allSpells);

        int holderIndex = 0;
        for(int spellIndex = 1; spellIndex < spellNames.Length; spellIndex++) {
            if (standardDeviations[holderIndex] > standardDeviations[spellIndex]) {
                holderIndex = spellIndex;
            }
        }

        Debug.Log(spellNames[holderIndex] + " std: " + standardDeviations[holderIndex]);

        if (standardDeviations[holderIndex] > 0.45f) {
            Debug.Log("spell not recognized");
            return "spell not recognized";
        }


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

    static List<Vector2> getScaledVectors (List<Vector2> allPoints, int interval) {

        

        List<Vector2> scaledVectors;
        scaledVectors = new List<Vector2>();
        Vector2 distanceVector;
        distanceVector = new Vector2();

        List<Vector2> distanceVectors;
        distanceVectors = new List<Vector2>();


        // get distance vectors
        for (int index = 0; index < allPoints.Count - interval; index += interval) {    
            Vector2 currentPoint = allPoints[index];
            Vector2 nextPoint = allPoints[index + interval];
            distanceVector = (new Vector2(nextPoint.x - currentPoint.x, nextPoint.y - currentPoint.y));
            distanceVectors.Add(distanceVector);
        }


        // find scale multiplier
        int greatestDistanceVectorIndex = 0;
        float holder = 0;
        for (int index = 0; index < distanceVectors.Count; index++) {
            if (distanceVectors[index].magnitude > holder) {
                greatestDistanceVectorIndex = index;
            }
        }

        float scaleMultiplier = 1 / distanceVectors[greatestDistanceVectorIndex].magnitude;

        // scale vectors to same size
        for (int index = 0; index < distanceVectors.Count; index++) {
            scaledVectors.Add(distanceVectors[index] * scaleMultiplier);

        }

        //if (scaledVectors.Count != 10) {Debug.Log("# of vectors: " + scaledVectors.Count);}

        return scaledVectors;

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

    static float[] getStandardDeviations(List<Vector2> inputscaledVectors, Vector2[,] spellscaledVectors) {
        float[] standardDeviations = new float[spellscaledVectors.GetLength(0)];
        float holder;
        for (int spellIndex = 0; spellIndex < spellscaledVectors.GetLength(0); spellIndex++) {

            holder = 0;
            for (int vectorIndex = 0; vectorIndex < Mathf.Min(spellscaledVectors.GetLength(1), inputscaledVectors.Count); vectorIndex++) {
                holder += Mathf.Pow(inputscaledVectors[vectorIndex].x - spellscaledVectors[spellIndex, vectorIndex].x, 2);
                holder += Mathf.Pow(inputscaledVectors[vectorIndex].y - spellscaledVectors[spellIndex, vectorIndex].y, 2);
            }
            standardDeviations[spellIndex] = Mathf.Sqrt(holder/(standardDeviations.Length));

        }

        return standardDeviations;

    }

    static void saveSpellJson(string jsonFilePath, List<Vector2> scaledVectors, string existingSpellData) {

        SpellData spellData;
        spellData = new SpellData(scaledVectors, "name");

        if (existingSpellData != "") {
            System.IO.File.WriteAllText(jsonFilePath, existingSpellData + spellData.toString());
        } else {
            System.IO.File.WriteAllText(jsonFilePath, spellData.toString());
        }


    }

}

public class SpellData {
    
    List<Vector2> scaledVectors;
    string name;

    public SpellData(List<Vector2> scaledVectors, string name) {
        this.scaledVectors = scaledVectors;
        this.name = name;
    }

    public string toString() {
        string output = "";
        for (int i = 0; i < scaledVectors.Count; i++) {

            if (i != scaledVectors.Count - 1) {
                output += "(" + scaledVectors[i].x + "," + scaledVectors[i].y + "), ";
            } else {
                output += "(" + scaledVectors[i].x + "," + scaledVectors[i].y + ")";
            }

        }

        Debug.Log(output);

        return output;
    }

}

public class NeuralNetwork {

    public int numLayerNeurons = 16;
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
 
        int gradientDimension = (int) ((results.Length * (Mathf.Pow(numLayerNeurons, 2)+ numLayerNeurons + 1)));




        return null;

    }

    float getPartialWeightFinal (int index, Neuron[] expectedResults) {
        
        Neuron neuron = results[index];
        float partial;

        partial = 2 * ( neuron.getValue() - expectedResults[index] ) * 
                        sigmoidDerivative(neuron) *
                        neuron.getRawSum();

        return partial;
            
    }

    float getPartialWeightLayer2 (int index, Neuron[] expectedResults) {

        Neuron neuron = stage2Neurons[index];
        float partial = 0;

        for (int i = 0; i < results.Length; i++) {
            partial += 2 * ( results[i].getValue() ) * sigmoidDerivative( results[i] ) * results[i].getWeight *
            sigmoidDerivative(neuron) * neuron.getRawSum;
        }

        return partial;

    }

    float getPartialWeightLayer1 (int index, Neuron[] expectedResults) {

        Neuron neuron = stage2Neurons[index];
        float partial = 0;

        for (int i = 0; i < results.Length; i++) {

            for (int j = 0; j < results.Length; j++) {



            }

        }

        return partial;
        
    }

    float sigmoidDerivative (Neuron neuron) {
        return ( Mathf.Exp( - ( neuron.getBias() + neuron.getWeightedSum() )) / 
                Mathf.Pow( 1 + Mathf.Exp( - ( neuron.getBias() + neuron.getWeightedSum() ) ) , 2 ) );
    }

    float getCost (Neuron[] results, Neuron[] expectedResults) {
        
        float cost = 0;

        for (int i = 0; i < results.Length; i++ ) {
            cost += (Mathf.Pow(results[i].getValue() - expectedResults[i].getValue(), 2));
        }

        return cost;

    }

}

public class Neuron {

    float[] weights;
    float bias;
    float value;
    float weightedSum;
    float rawSum;
    float randomLimit = 5.0f;
    float[] inputs;

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

        this.inputs = inputs;

        weightedSum = 0;
        rawSum = 0;

        for (int index = 0; index < inputs.Length; index++) {
            weightedSum += this.inputs[index] * weights[index];
            rawSum += this.inputs[index];
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

    public float getValue() {
        return value;
    }

    public float getWeightedSum() {
        return weightedSum;
    }

    public float getRawSum() {
        return rawSum;
    }

    public float getBias() {
        return bias;
    }

    public float getWeight() {
        return weight;
    }

}
