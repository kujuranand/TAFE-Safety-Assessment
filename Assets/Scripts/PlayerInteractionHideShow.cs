using UnityEngine;

public class PlayerInteractionHideShow : MonoBehaviour
{
    [Header("Object Appearance Settings")]
    public GameObject objectToAppear; // GameObject to appear after interaction 2
    public GameObject objectToHide;   // GameObject to hide after interaction 2

    public void HandleObjectAppearance()
    {
        if (objectToAppear != null)
        {
            objectToAppear.SetActive(true);
        }

        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }
    }
}
