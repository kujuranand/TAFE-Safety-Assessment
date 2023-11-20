using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem_Helmet : MonoBehaviour
{

    public GameObject Cap;
    public GameObject Helmet;
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
                if (Cap != null)
                {
                    Cap.SetActive(!Cap.activeSelf);
                    Debug.Log("Hide Cap");
                }
                
                if (Helmet != null)
                {
                    Helmet.SetActive(!Helmet.activeSelf);
                    InteractionText.SetActive(false); // Hide Interaction Text
                    Debug.Log("Show Helmet");
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
