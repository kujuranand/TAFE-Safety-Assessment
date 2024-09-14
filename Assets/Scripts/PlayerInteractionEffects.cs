using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionEffects : MonoBehaviour
{
    [Header("Highlight Settings")]
    public Color highlightColor = Color.yellow;
    public float highlightMaxIntensity = 2.0f;
    public float highlightMinIntensity = 0.5f;
    public float pulseSpeed = 1.0f;

    [Header("Tick and Cross Mark Settings")]
    public GameObject tickCrossCanvasPrefab; // Reference to the TickCrossCanvas prefab that has both tick and cross images
    public Transform markSpawnPoint;         // Point above the object where the tick or cross mark will be spawned
    public Vector3 markSize = new Vector3(1f, 1f, 1f); // Mark size field
    public Camera mainCamera;                // Reference to the main camera

    private Renderer objectRenderer;
    private Material originalMaterial;
    private Color originalEmissionColor;
    private Coroutine highlightCoroutine;
    private GameObject tickCrossCanvasInstance; // Instance of the TickCrossCanvas prefab
    private Image tickImage;   // Reference to the tick image in the canvas
    private Image crossImage;  // Reference to the cross image in the canvas

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("Renderer component is not found on this GameObject!", gameObject);
        }
        else
        {
            originalMaterial = objectRenderer.material;
            originalEmissionColor = originalMaterial.GetColor("_EmissionColor");
        }
    }

    private void Update()
    {
        // Ensure the tick or cross mark (if spawned) faces the camera
        if (tickCrossCanvasInstance != null && mainCamera != null)
        {
            tickCrossCanvasInstance.transform.LookAt(mainCamera.transform);
        }
    }

    // Start the highlight effect
    public void StartHighlight()
    {
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
        }
        highlightCoroutine = StartCoroutine(PulseHighlight());
    }

    // Stop the highlight effect
    public void StopHighlight()
    {
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
            highlightCoroutine = null;
        }

        objectRenderer.material.SetColor("_EmissionColor", originalEmissionColor);
        objectRenderer.material.DisableKeyword("_EMISSION");
    }

    // Spawn or show the tick mark after a successful interaction (Yes Action)
    public void ShowTickMark()
    {
        if (tickCrossCanvasPrefab != null && markSpawnPoint != null)
        {
            SetupTickCrossCanvas();
            tickImage.enabled = true;  // Enable the tick image
            crossImage.enabled = false; // Disable the cross image
        }
        else
        {
            Debug.LogError("TickCrossCanvas prefab or mark spawn point is not assigned!", gameObject);
        }
    }

    // Spawn or show the cross mark after an incorrect interaction (No Action)
    public void ShowCrossMark()
    {
        if (tickCrossCanvasPrefab != null && markSpawnPoint != null)
        {
            SetupTickCrossCanvas();
            tickImage.enabled = false; // Disable the tick image
            crossImage.enabled = true; // Enable the cross image
        }
        else
        {
            Debug.LogError("TickCrossCanvas prefab or mark spawn point is not assigned!", gameObject);
        }
    }

    // Set up the TickCrossCanvas instance, ensuring only one instance exists
    private void SetupTickCrossCanvas()
    {
        if (tickCrossCanvasInstance == null)
        {
            tickCrossCanvasInstance = Instantiate(tickCrossCanvasPrefab, markSpawnPoint.position, Quaternion.identity);
            tickCrossCanvasInstance.transform.localScale = markSize;

            // Get references to the Tick and Cross images from the prefab
            tickImage = tickCrossCanvasInstance.transform.Find("TickImage").GetComponent<Image>();
            crossImage = tickCrossCanvasInstance.transform.Find("CrossImage").GetComponent<Image>();
        }
    }

    // Coroutine to pulse the highlight in and out continuously while the player is in the trigger
    private IEnumerator PulseHighlight()
    {
        bool increasing = true;
        float currentIntensity = highlightMinIntensity;

        while (true)
        {
            float elapsedTime = 0f;

            // Loop between increasing and decreasing intensity
            while (elapsedTime < 1.0f / pulseSpeed)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / (1.0f / pulseSpeed);

                currentIntensity = increasing ? Mathf.Lerp(highlightMinIntensity, highlightMaxIntensity, t) :
                                                Mathf.Lerp(highlightMaxIntensity, highlightMinIntensity, t);

                objectRenderer.material.SetColor("_EmissionColor", highlightColor * currentIntensity);
                objectRenderer.material.EnableKeyword("_EMISSION");

                yield return null;
            }

            increasing = !increasing;
        }
    }
}
