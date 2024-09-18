using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogDisplay : MonoBehaviour
{
    private TextMeshProUGUI logText;

    private void Start()
    {
        // Get the TextMeshPro component
        logText = GetComponent<TextMeshProUGUI>();
    }

    public void ShowLog(string message)
    {
        // Update the text with the log message
        logText.text = message;
    }

    public void HideLog()
    {
        // Clear the text to hide the log message
        logText.text = "";
    }
}