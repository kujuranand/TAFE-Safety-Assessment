using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SupervisorInteraction : MonoBehaviour
{
    public GameObject InteractionText;
    public GameObject SupervisorText1;
    public GameObject SupervisorText2;
    public GameObject HardHat;
    public GameObject HiVisShirt;
    public GameObject SafetyBoots;
    public Animator animator;

    private bool playerInsideTrigger = false;

    private void Start()
    {
        // Ensure that the Animator component is assigned in the Unity Editor
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned to the script. Assign it in the Unity Editor.");
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

                animator.SetTrigger("Talk1"); // Set trigger
                Debug.Log("First Trigger");

                if (areObjectsVisible)
                {
                    animator.SetTrigger("Talk2"); // Set trigger only if all PPEs are active
                    SupervisorText2.SetActive(true); // All three PPEs are visible
                    SupervisorText1.SetActive(false); // Hide SupervisorText1
                    Debug.Log("All PPE active.");
                    Debug.Log("Second Trigger");
                }
                else
                {
                    SupervisorText1.SetActive(true); // One or more PPEs are not visible
                    SupervisorText2.SetActive(false); // Hide SupervisorText2
                    Debug.Log("One or more PPE is missing. Put on the PPE.");
                }
            }
            else
            {
                Debug.LogError("Animator is null. Assign it in Unity Editor");
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
                    SupervisorText1.SetActive(false);
                    Debug.Log("Show Press E");
                }
        }
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         // Check if all three PPEs are active
    //         bool areObjectsVisible = HardHat != null && HardHat.activeSelf && 
    //                                  HiVisShirt != null && HiVisShirt.activeSelf && 
    //                                  SafetyBoots != null && SafetyBoots.activeSelf;

    //         //Debug.Log("Staying in Trigger");
    //         if (Keyboard.current.eKey.wasPressedThisFrame)
    //         {
    //             if (animator != null)
    //             {
    //                 InteractionText.SetActive(false); // Hide Interaction Text

    //                 animator.SetTrigger("Talk1"); // Set trigger
    //                 Debug.Log("First Trigger");

    //                 if (areObjectsVisible)
    //                 {
    //                     animator.SetTrigger("Talk2"); // Set trigger only if all PPEs are active
    //                     SupervisorText2.SetActive(true); // All three PPEs are visible
    //                     SupervisorText1.SetActive(false); // Hide SupervisorText1
    //                     Debug.Log("All PPE active.");
    //                     Debug.Log("Second Trigger");
    //                 }
    //                 else
    //                 {
    //                     SupervisorText1.SetActive(true); // One or more PPEs are not visible
    //                     SupervisorText2.SetActive(false); // Hide SupervisorText2
    //                     Debug.Log("One or more PPE is missing. Put on the PPE.");
    //                 }
    //             }
    //             else
    //             {
    //                 Debug.LogError("Animator is null. Assign it in Unity Editor");
    //             }
    //         }
    //     }
    // }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;

            //Debug.Log("Exited Trigger");
            if (InteractionText != null)
                {
                    InteractionText.SetActive(false); // Hide Interaction Text
                    SupervisorText1.SetActive(false);
                    SupervisorText2.SetActive(false);
                    Debug.Log("Hide Press E");
                }
        }
    }
}
