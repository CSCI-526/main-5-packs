using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject uiPrefab; // The UI prefab will be assigned here in the inspector

    [Header("UI Sprites")]
    public Sprite starFullSprite;
    public Sprite starEmptySprite;

    // --- Private Variables ---
    private UICanvas uiCanvas; // A reference to the spawned UI's controller script

    private float timeLimit = 300f;
    private float currentTime;
    private int totalIngredients;
    private int ingredientsEaten = 0;
    private bool isGameActive = true;

    void Awake()
    {
        // Awake's job is now just to find the UI.
        if (uiPrefab != null)
        {
            GameObject uiInstance = Instantiate(uiPrefab);
            uiCanvas = uiInstance.GetComponent<UICanvas>();
        }
        else
        {
            Debug.LogError("UI Prefab is not assigned in the GameManager inspector!");
            return;
        }

        // Add listeners to the restart buttons
        foreach (var button in uiCanvas.restartButtons)
        {
            button.onClick.AddListener(RestartGame);
        }
    }

    // This is the new public method that MazeBuilder will call.
    public void StartLevel(int totalIngredientCount)
    {
        totalIngredients = totalIngredientCount;
        isGameActive = true;
        currentTime = timeLimit;

        uiCanvas.timerText.gameObject.SetActive(true);
        
        // Deactivate panels at the start
        uiCanvas.gameWonPanel.SetActive(false);
        uiCanvas.gameOverPanel.SetActive(false);

        if (totalIngredients == 0)
        {
            Debug.LogWarning("MazeBuilder reported 0 ingredients. The level will complete instantly.");
            WinGame();
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0, currentTime);
        UpdateTimerUI();

        if (currentTime <= 0)
        {
            LoseGame();
        }
    }

    private void InitializeGame()
    {
        currentTime = timeLimit;
        isGameActive = true;
        
        // Deactivate panels at the start
        uiCanvas.gameWonPanel.SetActive(false);
        uiCanvas.gameOverPanel.SetActive(false);

        totalIngredients = GameObject.FindGameObjectsWithTag("Ingredient").Length;
        if (totalIngredients == 0)
        {
            Debug.LogWarning("No ingredients found!");
            WinGame();
        }
    }

    public void OnIngredientEaten()
    {
        if (!isGameActive) return;
        ingredientsEaten++;
        if (ingredientsEaten >= totalIngredients)
        {
            WinGame();
        }
    }
    
    // --- The rest of the logic is the same, but uses the uiCanvas reference ---

    // Replace your old WinGame() method with this new one
    private void WinGame()
    {
        isGameActive = false;
        Time.timeScale = 0f;
        uiCanvas.gameWonPanel.SetActive(true);

        float timeTaken = timeLimit - currentTime;
        int starsEarned = CalculateStars(timeTaken);

        // Check if the sprite variables have been assigned in the Inspector
        if (starFullSprite == null || starEmptySprite == null)
        {
            Debug.LogError("Star sprites have not been assigned in the GameManager's Inspector!");
            return; // Exit the function to prevent errors
        }

        // Loop through each Image component in the stars array
        for (int i = 0; i < uiCanvas.stars.Length; i++)
        {
            // If the current star's index is less than the number of stars earned...
            if (i < starsEarned)
            {
                // ...set its sprite to the full star.
                uiCanvas.stars[i].sprite = starFullSprite;
            }
            else
            {
                // ...otherwise, set it to the empty star.
                uiCanvas.stars[i].sprite = starEmptySprite;
            }
        }
    }

    private void LoseGame()
    {
        isGameActive = false;
        Time.timeScale = 0f;
        uiCanvas.gameOverPanel.SetActive(true);
    }
    
    // In GameManager.cs
    private void UpdateTimerUI()
    {
        // Calculate minutes and seconds as before
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // NEW: Calculate milliseconds (hundredths of a second)
        int milliseconds = Mathf.FloorToInt((currentTime * 100) % 100);

        // Update the text format to include milliseconds
        uiCanvas.timerText.text = string.Format("Time left: {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private int CalculateStars(float time)
    {
        if (time <= 120) return 5;
        if (time <= 180) return 4;
        if (time <= 240) return 3;
        if (time <= 270) return 2;
        return 1;
    }
}