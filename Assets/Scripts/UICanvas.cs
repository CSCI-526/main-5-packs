using UnityEngine;
using TMPro; // Don't forget this for TextMeshPro elements

public class UICanvas : MonoBehaviour
{
    // This script will live on your UI prefab.
    // Its only job is to hold references to its own child elements.

    [Header("UI Element References")]
    public TextMeshProUGUI timerText;
    public GameObject gameWonPanel;
    // public TextMeshProUGUI resultText;
    public UnityEngine.UI.Image[] stars;
    public GameObject gameOverPanel;
    public UnityEngine.UI.Button[] restartButtons; // An array to hold all restart buttons
}