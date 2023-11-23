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

    public AudioSource doorOpenAudio;
    public AudioSource doorLockedAudio;
    public AudioSource doorCloseAudio;

    private bool playerInsideTrigger = false;

    private void Start()
    {
        // Ensure that the Animator component is assigned in the Unity Editor
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned to the script.");
        }

        // Ensure that AudioSource components are assigned in the Unity Editor
        if (doorOpenAudio == null || doorLockedAudio == null || doorCloseAudio == null)
        {
            Debug.LogError("One or more AudioSource components are not assigned to the script.");
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

                PlayDoorAudio(doorLockedAudio); // Play door locked audio

                if (areObjectsVisible)
                {
                    animator.SetTrigger("doorOpen"); // Set door open trigger
                    DoorLockedText.SetActive(false); // Hide Door Locked Text
                    Debug.Log("Open Door.");

                    PlayDoorAudio(doorOpenAudio); // Play door opening audio

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

            // Schedule playing door closing audio with a 1-second delay
            Invoke("PlayDoorClosingAudio", 0.75f);
        }

        else
        {
            Debug.LogError("Animator is null.");
        }
    }

    // Play the door closing audio
    private void PlayDoorClosingAudio()
    {
        PlayDoorAudio(doorCloseAudio); // Play door closing audio
    }

    // Play the corresponding audio clip based on the provided AudioSource
    private void PlayDoorAudio(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource is null.");
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
