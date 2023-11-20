using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem_HiVisShirt : MonoBehaviour
{

    public GameObject Shirt;
    public GameObject HiVisShirt;
    public GameObject InteractionText;

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
                if (Shirt != null)
                {
                    Shirt.SetActive(!Shirt.activeSelf);
                    Debug.Log("Hide Shirt");
                }
                
                if (HiVisShirt != null)
                {
                    HiVisShirt.SetActive(!HiVisShirt.activeSelf);
                    InteractionText.SetActive(false); // Hide Interaction Text
                    Debug.Log("Show HiVisShirt");
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
                    InteractionText.SetActive(false); // Hide Interaction Text
                    Debug.Log("Hide Press E");
                }
        }
    }

}
