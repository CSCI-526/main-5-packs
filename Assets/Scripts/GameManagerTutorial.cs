using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManagerTutorial : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI instructionText;
    public GameObject endPanel;

    private int step = 0;

    private bool chiliCollected = false;
    private bool iceMelted = false;
    private bool butterCollected = false;
    private bool stickyPassed = false;
    private bool breadCollected = false;
    private bool waterCleared = false;

    private int totalIngredients = 0;
    private int collectedIngredients = 0;

    private string[] tutorialSteps = {
        "Use W, A, S, D or Arrow keys to move!",
        "Now collect the CHILI to melt ice walls",
        "Great! Melt the ICE wall to move forward",
        "Awesome! Collect the BUTTER to glide through sticky floors",
        "Now glide through the sticky floor to continue!",
        "Great! Grab the BREAD to absorb water",
        "Use the BREAD to absorb the water patches",
        "Perfect! Tutorial Complete — You're ready to play the real game!"
    };

    void Start()
    {
        Time.timeScale = 1f;
        endPanel.SetActive(false);

        totalIngredients = GameObject.FindGameObjectsWithTag("Ingredient").Length;
        collectedIngredients = 0;

        ShowStep();

        StartCoroutine(AutoAdvanceAfterDelay(2f));
    }

    private IEnumerator AutoAdvanceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (step == 0)
        {
            NextStep();
        }
    }

    private void ShowStep()
    {
        instructionText.text = tutorialSteps[Mathf.Clamp(step, 0, tutorialSteps.Length - 1)];
    }


    public void OnChiliCollected()
    {
        StopAllCoroutines();
        collectedIngredients++;

        if (!chiliCollected)
        {
            chiliCollected = true;
            step = Mathf.Max(step, 2);
            ShowStep();
        }

        CheckForTutorialCompletion();
    }

    public void OnIceWallMelted()
    {
        if (!iceMelted)
        {
            iceMelted = true;
            step = Mathf.Max(step, 3);
            ShowStep();
        }
    }

    public void OnButterCollected()
    {
        collectedIngredients++;

        if (!butterCollected)
        {
            butterCollected = true;
            step = Mathf.Max(step, 4);
            ShowStep();
        }

        CheckForTutorialCompletion();
    }

    public void OnStickyZonePassed()
    {
        if (!stickyPassed)
        {
            stickyPassed = true;
            step = Mathf.Max(step, 5);
            ShowStep();
        }
    }

    public void OnBreadCollected()
    {
        collectedIngredients++;

        if (!breadCollected)
        {
            breadCollected = true;
            step = Mathf.Max(step, 6);
            ShowStep();
        }

        CheckForTutorialCompletion();
    }

    public void OnWaterPatchCleared()
    {
        if (!waterCleared)
        {
            waterCleared = true;
            step = Mathf.Max(step, 7);
            ShowStep();
        }
    }

    private void CheckForTutorialCompletion()
    {
        if (collectedIngredients >= totalIngredients)
        {
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        if (endPanel.activeSelf) return; 

        endPanel.SetActive(true);
        instructionText.text = tutorialSteps[tutorialSteps.Length - 1];
        Time.timeScale = 0f;
        Debug.Log("🎉 Tutorial completed — all ingredients collected!");
    }

    public void RetryTutorial()
    {
        SceneManager.LoadScene("DemoTutorialScene");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void NextStep()
    {
        step++;
        if (step < tutorialSteps.Length)
            ShowStep();
    }
}