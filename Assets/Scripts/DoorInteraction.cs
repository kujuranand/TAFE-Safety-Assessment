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
    private bool doorOpen = false;

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
            InteractionText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            InteractionText.SetActive(false); // Hide Interaction Text
            DoorLockedText.SetActive(false); // Hide Door Locked Text
        }
    }

    private void OnEKeyPressed()
    {
        // Check if all three PPEs are active
        bool areObjectsVisible = HardHat != null && HardHat.activeSelf && 
                                 HiVisShirt != null && HiVisShirt.activeSelf && 
                                 SafetyBoots != null && SafetyBoots.activeSelf;

        InteractionText.SetActive(false); // Hide Interaction Text
        DoorLockedText.SetActive(true); // Show Door Locked Text
        PlayDoorAudio(doorLockedAudio); // Play door locked audio

        if (areObjectsVisible)
        {
            if (doorOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        animator.SetTrigger("doorOpen"); // Set door open trigger
        DoorLockedText.SetActive(false); // Hide Door Locked Text
        PlayDoorAudio(doorOpenAudio); // Play door opening audio
        doorOpen = true;
    }

    // Method to close the door
    private void CloseDoor()
    {
        animator.SetTrigger("doorClose"); // Set door close trigger
        PlayDoorAudio(doorCloseAudio);
        doorOpen = false;
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
}
