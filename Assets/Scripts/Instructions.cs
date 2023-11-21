using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    public GameObject InstructionsText;

    private bool instructionsDisplayed = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !instructionsDisplayed)
        {
            //Debug.Log("Entered Trigger");
            if (InstructionsText != null)
                {
                    InstructionsText.SetActive(true);
                    Debug.Log("Show Instructions.");
                    instructionsDisplayed = true;
                }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (InstructionsText != null)
                {
                    InstructionsText.SetActive(false); // Hide Interaction Text
                    Debug.Log("Hide Instructions.");
                }
    }
}
