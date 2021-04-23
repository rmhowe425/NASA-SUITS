using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class geologyManager : MonoBehaviour
{
    public geoInstructions dialogue;
    public Text dialogueText;
    public string instruction;
    public bool geologyRunning;

    private ArrayList instructions;
    private int totalInstructions;
    private int instructionNumber;

    // Start is called before the first frame update
    void Start()
    {
        instructions = new ArrayList();
    }

    public void startInstructions(geoInstructions dialogue)
    {
        Debug.Log("geology is running");

        instructions.Clear();

        foreach (string instruction in dialogue.instructions)
        {
            instructions.Add(instruction);
        }

        totalInstructions = instructions.Count;
        Debug.Log("Total Instructions is " + totalInstructions);

        instructionNumber = -1;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
            Debug.Log("In next sentence function");

            // Increment count by 1
            instructionNumber++;

            Debug.Log("Total Instructions is: " + totalInstructions);

            // Check if count is greater than array size. If greater, decrement count.
            if (instructionNumber >= totalInstructions)
            {
                instructionNumber = instructionNumber - 1;
            }
            
            // script is for debugging current instruction number
            Debug.Log("In Next Sentence Method, Instruction is " + instructionNumber);

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];
            Debug.Log("Instruction ->" + instruction);

            // Display the instruction
            dialogueText.text = instruction;
     
    }

    public void DisplayPreviousSentence()
    {
    
            // Decrement count
            instructionNumber--;

            // Check if count drops below 0. If it does, set it back to 0.
            if (instructionNumber < 0)
            {
                instructionNumber = 0;
            }

            // script is for debugging current instruction number
            Debug.Log(instructionNumber);

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
     
    }
}
