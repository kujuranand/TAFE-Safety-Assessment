using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    public GameObject InteractionText;
    public GameObject DoorLockedText;
    public Animator animator;
    public GameObject HardHat;
    public GameObject HiVisShirt;
    public GameObject SafetyBoots;

    private bool playerInsideTrigger = false;

    private void Start()
    {
        // Ensure that the Animator component is assigned in the Unity Editor
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned to the script.");
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

    private void OnEKeyPressed()
    {
        // Check if all three PPEs are active
        bool areObjectsVisible = HardHat != null && HardHat.activeSelf && 
                                 HiVisShirt != null && HiVisShirt.activeSelf && 
                                 SafetyBoots != null && SafetyBoots.activeSelf;

        if (animator != null)
            {
                InteractionText.SetActive(false); // Hide Interaction Text
                DoorLockedText.SetActive(true); // Show Door Locked Text

                if (areObjectsVisible)
                {
                    animator.SetTrigger("doorOpen"); // Set door open trigger
                    DoorLockedText.SetActive(false); // Hide Door Locked Text
                    Debug.Log("Open Door.");

                    Invoke("CloseDoor", 5f); // Schedule the doorClose Trigger after 5 seconds
                }
            }
        else
            {
                Debug.LogError("Animator is null.");
            }
    }

    // Method to close the door
    private void CloseDoor()
    {
        if (animator != null)
        {
            animator.SetTrigger("doorClose"); // Set door close trigger
            Debug.Log("Close Door.");
        }

        else
        {
            Debug.LogError("Animator is null.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             playerInsideTrigger = true;

            //Debug.Log("Entered Trigger");
            if (InteractionText != null)
                {
                    InteractionText.SetActive(!InteractionText.activeSelf);
                    Debug.Log("Show Press E");
                }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;

            //Debug.Log("Exited Trigger");
            if (InteractionText != null)
                {
                    InteractionText.SetActive(false); // Hide Interaction Text
                    DoorLockedText.SetActive(false); // Hide Door Locked Text
                    Debug.Log("Hide Press E");
                }
        }
    }
}
