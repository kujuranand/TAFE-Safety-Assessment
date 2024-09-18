using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public enum InteractionType { PPEDisplay, PermitDisplay, HazardDisplay } // Enum for selecting interaction type

    [Header("Interaction Texts")]
    public string InteractionStartText = "Press [E]";
    public string QuestionText = "???";
    public string YesActionText = "Correct!";
    public string NoActionText = "Incorrect!";
    public string AddToListText = "Add to List"; // Generic for both PPE, Permit, and Hazard
    public string YesNoOptionsText = "Press [Y] for Yes, [N] for No"; // Customizable Yes/No options text

    [Header("UI Displays")]
    public PlayerInteractionPPEListDisplay ppeDisplay; // Display for the PPE list
    public PlayerInteractionPermitListDisplay permitDisplay; // Display for the Permit list
    public PlayerInteractionHazardListDisplay hazardDisplay; // Display for the Hazard list
    public LogDisplay logDisplay;
    public ScoreDisplay scoreDisplay;
    public InteractionType Type = InteractionType.PPEDisplay; // Field to select between PPE, Permit, or Hazard list

    [Header("Interaction Score")]
    public int CorrectScore = 10; // Score for correct answers
    public int IncorrectScore = 5; // Score to subtract for incorrect answers
    public string CorrectAnswer = "Yes"; // Custom field to specify the correct answer (Yes/No)

    private PlayerInteractionEffects interactionEffects;
    private PlayerInteractionHideShow hideShow;

    private int interactionStage = 0;
    private static int totalScore = 0;
    private bool playerInsideTrigger = false;

    private void Awake()
    {
        totalScore = 0; // Reset score when the game starts
        interactionEffects = GetComponent<PlayerInteractionEffects>();
        hideShow = GetComponent<PlayerInteractionHideShow>();
    }

    private void Update()
    {
        // Trigger interaction start (Interaction 0)
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame && interactionStage == 0)
        {
            CompleteInteraction0(); // Move to Interaction 1 automatically
        }

        // Handle Yes/No input for Interaction Stage 1
        if (interactionStage == 1)
        {
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                YesAction(); // Handle Yes action
            }
            else if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                NoAction(); // Handle No action
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;

            // Only reset to Interaction 0 if Interaction 1 is not complete
            if (interactionStage < 2)
            {
                interactionStage = 0;
                logDisplay.ShowLog(InteractionStartText);
                interactionEffects.StartHighlight();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            interactionEffects.StopHighlight();
            logDisplay.HideLog();
        }
    }

    // Complete Interaction 0 and start Interaction 1
    private void CompleteInteraction0()
    {
        logDisplay.HideLog();
        logDisplay.ShowLog(QuestionText + "\n" + YesNoOptionsText); // Automatically append new line
        interactionStage = 1; // Move directly to Interaction Stage 1
    }

    // Yes Action
    private void YesAction()
    {
        logDisplay.HideLog();

        if (CorrectAnswer == "Yes")
        {
            AddScore(CorrectScore);
            logDisplay.ShowLog(YesActionText);
            interactionEffects.ShowTickMark();
            AddToList(AddToListText);
        }
        else
        {
            AddScore(-IncorrectScore);
            logDisplay.ShowLog(NoActionText);
            interactionEffects.ShowCrossMark();
            AddToList(AddToListText + " [x]");
        }

        FinalizeInteraction();
    }

    // No Action
    private void NoAction()
    {
        logDisplay.HideLog();

        if (CorrectAnswer == "No")
        {
            AddScore(CorrectScore);
            logDisplay.ShowLog(YesActionText);
            interactionEffects.ShowTickMark();
            AddToList(AddToListText);
        }
        else
        {
            AddScore(-IncorrectScore);
            logDisplay.ShowLog(NoActionText);
            interactionEffects.ShowCrossMark();
            AddToList(AddToListText + " [x]");
        }

        FinalizeInteraction();
    }

    // Finalize the interaction
    private void FinalizeInteraction()
    {
        interactionStage = 2; // Mark Interaction as complete, no further interactions
        interactionEffects.StopHighlight(); // Stop the highlight effect
        UpdateScoreText();

        if (hideShow != null)
        {
            hideShow.HandleObjectAppearance();
        }
    }

    // Add Score to the total score
    private void AddScore(int points)
    {
        totalScore += points;
        UpdateScoreText();
    }

    // Update the Score Display
    private void UpdateScoreText()
    {
        if (scoreDisplay != null)
        {
            scoreDisplay.UpdateScore(totalScore);
        }
        else
        {
            Debug.LogError("ScoreDisplay UI element is not assigned!", gameObject);
        }
    }

    // Add item to the correct list based on the selected type (PPE, Permit, or Hazard)
    private void AddToList(string itemText)
    {
        if (Type == InteractionType.PPEDisplay && ppeDisplay != null)
        {
            ppeDisplay.AddPPEItem(itemText); // Add to PPE display
        }
        else if (Type == InteractionType.PermitDisplay && permitDisplay != null)
        {
            permitDisplay.AddPermitItem(itemText); // Add to Permit display
        }
        else if (Type == InteractionType.HazardDisplay && hazardDisplay != null)
        {
            hazardDisplay.AddHazardItem(itemText); // Add to Hazard display
        }
    }
}
