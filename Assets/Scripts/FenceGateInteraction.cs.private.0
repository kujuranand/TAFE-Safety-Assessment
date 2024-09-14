using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FenceGateInteraction : MonoBehaviour
{
    public GameObject InteractionText;
    public Animator animator;

    public AudioSource fenceGateOpenAudio;
    public AudioSource fenceGateCloseAudio;

    private bool playerInsideTrigger = false;
    private bool gateOpen = false;

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
            InteractionText.SetActive(false);
        }
    }

    private void OnEKeyPressed()
    {
        InteractionText.SetActive(false); // Hide Interaction Text
        if (gateOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        animator.SetTrigger("doorOpen"); // Set door open trigger
        PlayDoorAudio(fenceGateOpenAudio); // Play door opening audio
        gateOpen = true;
    }

    private void CloseDoor()
    {
        animator.SetTrigger("doorClose"); // Set door close trigger
        PlayDoorAudio(fenceGateCloseAudio); // Play door closing audio
        gateOpen = false;
    }

    // Play the corresponding audio clip based on the provided AudioSource
    private void PlayDoorAudio(AudioSource audioSource)
    {
        audioSource.Play();
    }
}