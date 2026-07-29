using UnityEngine;

// Legacy health bars are disabled; the game now uses the text-based HUD.
public class Healthbars : MonoBehaviour
{
    private void Awake()
    {
        enabled = false;
    }

    private void OnGUI()
    {
        // Intentionally empty: health bars are disabled in favor of text UI.
    }
}
