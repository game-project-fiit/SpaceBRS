using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RulesManager : MonoBehaviour
{
    public List<GameObject> rulesImages;
    private int currentRuleIndex;
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
        => HandleRulesNavigation();

    private void HandleRulesNavigation()
    {
        if (!IsShowingRules) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            ChangeRule(false);
        else if (Input.GetKeyDown(KeyCode.Return))
            ChangeRule(true);
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentRuleIndex != rulesImages.Count - 1) return;
            
            PlayPageTurnSound();
            CloseRulesAndStartLevel();
        }
    }

    private void ShowRules()
    {
        if (rulesImages == null || rulesImages.Count == 0) return;

        currentRuleIndex = 0;

        foreach (var ruleImage in rulesImages)
            ruleImage.SetActive(false);

        rulesImages[currentRuleIndex].SetActive(true);
    }

    private void ChangeRule(bool forward)
    {
        if (currentRuleIndex >= rulesImages.Count - 1 && forward) return;
        if (currentRuleIndex == 0 && !forward) ExitGame();
        
        rulesImages[currentRuleIndex].SetActive(false);
        currentRuleIndex += forward ? 1 : -1;
        rulesImages[currentRuleIndex].SetActive(true);
        PlayPageTurnSound();
    }

    private void CloseRulesAndStartLevel()
    {
        foreach (var ruleImage in rulesImages)
            ruleImage.SetActive(false);

        IsShowingRules = false;

        if (gameManager)
            gameManager.StartGame();
    }

    private void PlayPageTurnSound()
    {
        if (pageTurnSound && audioSource)
            audioSource.PlayOneShot(pageTurnSound);
    }
    
    private static void ExitGame()
        => SceneManager.LoadScene("LevelsMenu");
}
