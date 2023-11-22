using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SupervisorInteraction : MonoBehaviour
{
    public GameObject InteractionText;
    public GameObject SupervisorText1;
    public GameObject SupervisorText2;
    public GameObject SupervisorText3;
    public GameObject HardHat;
    public GameObject HiVisShirt;
    public GameObject SafetyBoots;
    public GameObject Phone;
    public Animator animator;

    private bool playerInsideTrigger = false;

    private void Start()
    {
        // Ensure that the Animator component is assigned in the Unity Editor
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned to the script.");
        }

        // Ensure that the Phone game object is initially inactive
        if (Phone != null)
        {
            Phone.SetActive(false);
        }
        else
        {
            Debug.LogError("Phone game object is not assigned to the script.");
        }
    }

    private void Update()
    {
        // Check for key press in the Update method
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OnEKeyPressed();
        }

        // Check if the phone call animation is playing
        if (IsPhoneCallAnimationPlaying())
        {
            Phone.SetActive(true); // Activate the phone game object
        }
        else
        {
            Phone.SetActive(false); // Deactivate the phone game object
        }
    }

    private void OnEKeyPressed()
    {
        // Check if each PPE is active
        bool isHardHatActive = HardHat != null && HardHat.activeSelf;
        bool isHiVisShirtActive = HiVisShirt != null && HiVisShirt.activeSelf;
        bool isSafetyBootsActive = SafetyBoots != null && SafetyBoots.activeSelf;

        // Determine the number of active PPEs
        int activePPECount = (isHardHatActive ? 1 : 0) + 
                             (isHiVisShirtActive ? 1 : 0) + 
                             (isSafetyBootsActive ? 1 : 0);

        if (animator != null)
        {
            InteractionText.SetActive(false); // Hide Interaction Text

            animator.SetTrigger("Talk1"); // Set trigger
            Debug.Log("First Trigger");

            // Control the visibility of SupervisorText1, SupervisorText2, and SupervisorText3 based on the number of active PPEs
            SupervisorText1.SetActive(activePPECount == 0);
            SupervisorText2.SetActive(activePPECount == 1 || activePPECount == 2);
            SupervisorText3.SetActive(activePPECount == 3);

            if (activePPECount == 3)
            {
                animator.SetTrigger("Talk2"); // Set "Talk2" trigger if all three PPEs are active
                Debug.Log("Talk2 Trigger");
            }

            Debug.Log($"Active PPE Count: {activePPECount}");
        }
        else
        {
            Debug.LogError("Animator is null.");
        }
    }

    // Method to check if the phone call animation is playing
    private bool IsPhoneCallAnimationPlaying()
    {
        // phone call animation
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PhoneCall");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             playerInsideTrigger = true;

            //Debug.Log("Entered Trigger");
            if (InteractionText != null)
                {
                    InteractionText.SetActive(true);
                    SupervisorText1.SetActive(false);
                    SupervisorText2.SetActive(false);
                    SupervisorText3.SetActive(false);
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
                    SupervisorText3.SetActive(false);
                    Debug.Log("Hide Press E");
                }
        }
    }
}
