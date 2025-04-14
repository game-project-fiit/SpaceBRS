using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SettingsMenuController : MonoBehaviour
{
    public static SettingsMenuController Instance;
    public GameObject optionsPanel;
    public Slider musicSlider; 
    public Slider soundSlider; 
    public TextMeshProUGUI[] settingTexts; 
    public GameObject SettingsMenuPanel; 
    public GameObject MainMenuPanel; 
    public GameObject HotkeysPanel;
    public GameObject storyPanel;
    private int selectedIndex = 0;

    private void Awake() => Instance = this;

    private void Start()
    {
        UpdateMenu(); 
        HotkeysPanel.SetActive(false); 
    }

    private void Update()
    {
        if (!SettingsMenuPanel.activeSelf) return;
        if (settingTexts.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + settingTexts.Length) % settingTexts.Length;
            UpdateMenu();
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % settingTexts.Length;
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            AdjustVolume(-0.01f);


        else if (Input.GetKeyDown(KeyCode.RightArrow))
            AdjustVolume(0.01f);


        if (Input.GetKeyDown(KeyCode.Return))
            SelectOption();

        if (Input.GetKeyDown(KeyCode.Escape))
            HandleEscape();
    }
    
    private void HandleEscape()
    {
        if (SettingsMenuPanel.activeSelf)
        {
            CloseSettingsPanel();
            ReturnToMainMenu();
        }
    }

    private void UpdateMenu()
    {
        for (var i = 0; i < settingTexts.Length; i++)
            settingTexts[i].text = i == selectedIndex 
                ? $"> {GetOptionText(i)}"
                : GetOptionText(i);
    }

    private string GetOptionText(int index)
    {
        return index switch
        {
            0 => $"Music Volume: {Mathf.RoundToInt(musicSlider.value * 100)}%",
            1 => $"Sound Volume: {Mathf.RoundToInt(soundSlider.value * 100)}%",
            2 => "Hotkeys",
            3 => "See the plot again",
            _ => ""
        };
    }

    private void AdjustVolume(float change)
    {
        switch (selectedIndex)
        {
            case 0:
                musicSlider.value = Mathf.Clamp(musicSlider.value + change, 0f, 1f);
                break;
            case 1:
                soundSlider.value = Mathf.Clamp(soundSlider.value + change, 0f, 1f);
                break;
        }

        UpdateMenu();
    }

    private void ReturnToMainMenu()
    {
        SettingsMenuPanel.SetActive(false);
        enabled = false;
        MainMenuPanel.SetActive(true);

        var mainMenuController = MainMenuPanel.GetComponentInParent<MainMenuController>();
        if (mainMenuController != null)
        {
            mainMenuController.enabled = true;
            mainMenuController.ResetSelection();
        }

        var mainMenuButtons = MainMenuPanel.GetComponentsInChildren<Button>(true);
        foreach (var button in mainMenuButtons)
        {
            button.gameObject.SetActive(true);
            button.interactable = true;
        }

        if (mainMenuButtons.Length > 0)
            EventSystem.current.SetSelectedGameObject(mainMenuButtons[0].gameObject);

        else
            Debug.LogWarning("No buttons found in MainMenuPanel!");

    }
    
    private void SelectOption()
    {
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("Music volume selected.");
                break;
            case 1:
                Debug.Log("Sound volume selected.");
                break;
            case 2:
                OpenHotkeysPanel();
                break;
            case 3:
                OpenStoryPanelForReplay();
                break;
        }
    }

    private void OpenHotkeysPanel()
    {
        if (storyPanel != null)
        {
            storyPanel.SetActive(false);
            var storyController = storyPanel.GetComponent<StoryController>();

            if (storyController != null)
            {
                storyController.StopAllCoroutines();
                storyController.enabled = false;
                storyController.currentSlideIndex = 0;
            }
        }

        if (HotkeysPanel == null || SettingsMenuPanel == null)
        {
            Debug.LogError("HotkeysPanel or SettingsMenuPanel is not assigned!");
            return;
        }

        HotkeysPanel.SetActive(true);
        SettingsMenuPanel.SetActive(false);
        MainMenuPanel.SetActive(false);

        var hotkeysButtons = HotkeysPanel.GetComponentsInChildren<Button>(true);
        if (hotkeysButtons.Length > 0)
            EventSystem.current.SetSelectedGameObject(hotkeysButtons[0].gameObject);
        
        else
            Debug.LogWarning("No buttons found in HotkeysPanel!");
    }

    public void ReturnToOptionsAfterPlot()
    {
        SettingsMenuPanel.SetActive(true);
        Debug.Log("Returning to options");
    }

    private void CloseSettingsPanel()
    {
        SettingsMenuPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        UpdateMenu();
    }

    private void OpenStoryPanelForReplay()
    {
        if (storyPanel == null)
        {
            Debug.Log("StoryPanel is not assigned!");
            return;
        }

        var storyController = storyPanel.GetComponent<StoryController>();
        if (storyController != null)
        {
            storyController.currentSlideIndex = 0;
            storyController.replayFromOptions = true;
            storyController.enabled = true;
        }

        storyPanel.SetActive(true);
        SettingsMenuPanel.SetActive(false);
    }

    private void OnEnable()
    {
        selectedIndex = 0;
        UpdateMenu();
        UpdateMenu();
    }
}
