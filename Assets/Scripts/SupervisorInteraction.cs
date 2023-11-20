using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SupervisorInteraction : MonoBehaviour
{
    public GameObject InteractionText;
    public Animator animator;

    private void Start()
    {
        // Ensure that the Animator component is assigned in the Unity Editor
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned to the script. Assign it in the Unity Editor.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Entered Trigger");
            if (InteractionText != null)
                {
                    InteractionText.SetActive(!InteractionText.activeSelf);
                    Debug.Log("Show Press E");
                }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Staying in Trigger");
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Talk");
                }
                else
                {
                    Debug.LogError("Animator is null. Assign it in Unity Editor");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Exited Trigger");
            if (InteractionText != null)
                {
                    InteractionText.SetActive(!InteractionText.activeSelf);
                    Debug.Log("Hide Press E");
                }
        }
    }
}
