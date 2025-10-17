using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject uiPrefab;

    [Header("UI Sprites")]
    public Sprite starFullSprite;
    public Sprite starEmptySprite;

    private UICanvas uiCanvas;

    private float timeLimit = 120f;
    private float currentTime;
    private int totalIngredients;
    private int ingredientsEaten = 0;
    private bool isGameActive = true;

    void Awake()
    {
        Time.timeScale = 1f;
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

        foreach (var button in uiCanvas.restartButtons)
        {
            button.onClick.AddListener(RestartGame);
        }
    }

    public void StartLevel(int totalIngredientCount)
    {
        totalIngredients = totalIngredientCount;
        isGameActive = true;
        currentTime = timeLimit;

        uiCanvas.timerText.gameObject.SetActive(true);
        
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
    

    private void WinGame()
    {
        isGameActive = false;
        Time.timeScale = 0f;
        uiCanvas.gameWonPanel.SetActive(true);

        float timeTaken = timeLimit - currentTime;
        int starsEarned = CalculateStars(timeTaken);

        if (starFullSprite == null || starEmptySprite == null)
        {
            Debug.LogError("Star sprites have not been assigned in the GameManager's Inspector!");
            return;
        }

        for (int i = 0; i < uiCanvas.stars.Length; i++)
        {
            if (i < starsEarned)
            {
                uiCanvas.stars[i].sprite = starFullSprite;
            }
            else
            {
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
    
    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        int milliseconds = Mathf.FloorToInt((currentTime * 100) % 100);

        uiCanvas.timerText.text = string.Format("Time left: {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private int CalculateStars(float time)
    {
        if (time <= 24f) return 5;
        if (time <= 48f) return 4;
        if (time <= 72f) return 3;
        if (time <= 96f) return 2;
        return 1;
    }
}