using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject HotkeysPanel;
    public Button[] menuButtons;
    public TextMeshProUGUI[] menuTexts;
    public AudioClip moveSound;
    public AudioClip selectSound;
    public GameObject optionsPanel;
    public GameObject menuPanel;
    public GameObject storyPanel;
    private AudioSource audioSource;
    private int selectedIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateMenu();
        optionsPanel.SetActive(false);
    }

    void Update()
    {
        if (menuButtons.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            PlaySound(moveSound);
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            PlaySound(moveSound);
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlaySound(selectSound);
            SelectOption();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    private void UpdateMenu()
    {
        for (var i = 0; i < menuButtons.Length; i++)
        {
            if (i == selectedIndex)
            {
                menuTexts[i].text = ">" + menuTexts[i].text.TrimStart('>');
                menuButtons[i].Select();
            }

            else
                menuTexts[i].text = menuTexts[i].text.TrimStart('>');
        }
    }

    private void SelectOption()
    {
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("Start Game");
                if (PlayerPrefs.GetInt("StoryViewed", 0) == 0)
                {
                    storyPanel.SetActive(true);
                    menuPanel.SetActive(false);
                }
                else
                    SceneManager.LoadScene("LevelsMenu");
                break;
            case 1:
                Debug.Log("Options");
                OpenOptions();
                break;
            case 2:
                Debug.Log("Quit Game");
                PlaySound(selectSound);
                StartCoroutine(QuitGame());
                break;
        }
    }

    private void OpenOptions()
    {
        foreach (var button in menuButtons)
        {
            button.gameObject.SetActive(false);
        }

        menuPanel.SetActive(false);
        enabled = false;

        if (optionsPanel != null)
        {
            menuPanel.SetActive(false);
            optionsPanel.SetActive(true);
            SettingsMenuController.Instance.gameObject.SetActive(true);
            SettingsMenuController.Instance.enabled = true;
        }

        if (HotkeysPanel != null)
        {
            menuPanel.SetActive(false);
            HotkeysPanel.SetActive(false);
        }

        else
            Debug.LogWarning("HotkeysPanel is not assigned in the inspector!");
    }

    public void ResetSelection()
    {
        selectedIndex = 0;
        UpdateMenu();
    }

    private System.Collections.IEnumerator QuitGame()
    {
        if (SettingsMenuController.Instance != null)
        {
            SettingsMenuController.Instance.optionsPanel.SetActive(false);
            SettingsMenuController.Instance.SettingsMenuPanel.SetActive(false);

            if (SettingsMenuController.Instance.HotkeysPanel != null)
                SettingsMenuController.Instance.HotkeysPanel.SetActive(false);
            if (SettingsMenuController.Instance.storyPanel != null)
                SettingsMenuController.Instance.storyPanel.SetActive(false);
        }

        yield return new WaitForSeconds(selectSound.length / 2);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}