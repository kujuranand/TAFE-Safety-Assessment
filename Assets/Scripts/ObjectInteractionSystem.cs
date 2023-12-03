using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteractionSystem : MonoBehaviour
{
    public GameObject objectToHide; // clothing
    public GameObject objectToShow; // PPE
    
    public GameObject InteractionText;
    
    public AudioSource pickUpAudioSource;

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

            if (InteractionText != null)
                {
                    InteractionText.SetActive(true);
                    Debug.Log("Show - Press E");
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

            if (InteractionText != null)
                {
                    InteractionText.SetActive(false); // Hide Interaction Text
                    Debug.Log("Hide - Press E");
                }
        }
    }

    private void OnEKeyPressed()
    {
        putOnPPE();
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

            if (InteractionText != null)
            {
                InteractionText.SetActive(false); // Hide Interaction Text
                Debug.Log("Show PPE");
            }
        }
    }    
}
