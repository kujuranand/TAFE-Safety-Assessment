using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ObjectInteractionSystem : MonoBehaviour
{
    public GameObject objectToHide; // clothing
    public GameObject objectToShow; // PPE
    
    public AudioSource pickUpAudioSource;

    public string customInteractionText = "Custom interaction Text"; // Set custom text
    public LogDisplay logDisplay; // Reference to the LogDisplay script

    private bool playerInsideTrigger = false;

    private void Start()
    {
        CheckGameObject(objectToShow, "PPE");
    }

    private void CheckGameObject(GameObject gameObjectToCheck, string gameObjectName)
    {
        if (gameObjectToCheck != null)
        {
            pickUpAudioSource = gameObjectToCheck.GetComponent<AudioSource>();

            if (pickUpAudioSource != null)
            {
                Debug.Log("AudioSource found on the " + gameObjectName + " GameObject.");
            }
            else
            {
                Debug.LogError("AudioSource component not found on the " + gameObjectName + " GameObject.");
            }
        }
        else
        {
            Debug.LogError(gameObjectName + " GameObject not assigned.");
        }
    }

    private void Update()
    {
        // Check for key press in the Update method
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OnEKeyPressed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            
            

            if (customInteractionText != null)
                {
                // Show log message on the display
                logDisplay.ShowLog(customInteractionText);
                Debug.Log(customInteractionText);
                }
            else
            {
                Debug.LogError("Interaction text not assigned");
            }
        }
        else
        {
            Debug.LogError("Check - Player tag");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;

            if (customInteractionText != null)
            {
                logDisplay.HideLog();
            }
        }
    }

    private void OnEKeyPressed()
    {
        Invoke("putOnPPE", 0.8f); // Delay 1 second
        
        Invoke("DestroyObject", 0.8f); // Delay destroy object by 1 second
    }

    private void putOnPPE()
    {
        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
            Debug.Log("Hide clothing");
        }
        else
        {
            Debug.LogError("Clothing object not assigned");
        }

        if (objectToShow != null)
        {
            objectToShow.SetActive(true);

            if (pickUpAudioSource != null)
            {
                pickUpAudioSource.Play(); // Play the audio
            }

            if (customInteractionText != null)
            {
                logDisplay.HideLog();
            }
        }
    }

    private void DestroyObject()
    {
        // Destroy the object
        Destroy(gameObject);
    }    
}
