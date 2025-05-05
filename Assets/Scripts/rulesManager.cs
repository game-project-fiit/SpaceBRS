using UnityEngine;
using System.Collections.Generic;

public class RulesManager : MonoBehaviour
{
    public List<GameObject> rulesImages;
    private int currentRuleIndex = 0;
    public GameManager gameManager;
    private bool IsShowingRules { get; set; } = true;

    public AudioClip pageTurnSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ShowRules();
    }

    private void Update()
    {
        HandleRulesNavigation();
    }

    private void HandleRulesNavigation()
    {
        if (IsShowingRules)
        {
            // Листание правил по нажатию клавиши Enter
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ShowNextRule();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentRuleIndex == rulesImages.Count - 1)
                {
                    PlayPageTurnSound();
                    CloseRulesAndStartLevel();
                }
            }
        }
    }

    private void ShowRules()
    {
        if (rulesImages == null || rulesImages.Count == 0)
        {
            return;
        }

        currentRuleIndex = 0;

        foreach (var ruleImage in rulesImages)
        {
            ruleImage.SetActive(false);
        }

        rulesImages[currentRuleIndex].SetActive(true);
    }

    private void ShowNextRule()
    {
        if (currentRuleIndex < rulesImages.Count - 1)
        {
            rulesImages[currentRuleIndex].SetActive(false);
            currentRuleIndex++;
            rulesImages[currentRuleIndex].SetActive(true);
            PlayPageTurnSound();
        }
    }

    private void CloseRulesAndStartLevel()
    {
        foreach (var ruleImage in rulesImages)
        {
            ruleImage.SetActive(false);
        }

        IsShowingRules = false;

        if (gameManager != null)
        {
            gameManager.StartGame();
        }
    }

    private void PlayPageTurnSound()
    {
        if (pageTurnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pageTurnSound);
        }
    }
}
