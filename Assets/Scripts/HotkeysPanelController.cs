using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HotkeysPanelController : MonoBehaviour
{
    public Button[] controlButtons;
    public GameObject HotkeysPanel;
    public GameObject SettingsMenuPanel; 
    public GameObject MainMenuPanel;
    public AudioClip moveSound;
    public AudioClip selectSound;
    private AudioSource audioSource;
    private int selectedIndex = 0;
    private string selectedControlScheme;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on this GameObject!");
            return;
        }

        selectedControlScheme = PlayerPrefs.GetString("ControlScheme", "WASD");
        selectedIndex = selectedControlScheme == "WASD" ? 0 : 1;
        UpdateButtonSelection();
    }

    private void Update()
    {
        if (!HotkeysPanel.activeSelf) return;
        if (controlButtons.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex = (selectedIndex - 1 + controlButtons.Length) % controlButtons.Length;
            PlaySound(moveSound);
            UpdateButtonSelection();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            selectedIndex = (selectedIndex + 1) % controlButtons.Length;
            PlaySound(moveSound);
            UpdateButtonSelection();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlaySound(selectSound);
            SelectControlScheme(); // Выбор горячих клавиш
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlaySound(selectSound);
            HandleEscape();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing!");
        }
    }

    private void UpdateButtonSelection()
    {
        for (var i = 0; i < controlButtons.Length; i++)
        {
            var buttonImage = controlButtons[i].GetComponent<Image>();
            var text = controlButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            if (text is not null)
            {
                text.color = i == selectedIndex ? Color.black : Color.white;
            }

            if (buttonImage is not null)
            {
                buttonImage.color = i == selectedIndex ? Color.white : new Color(1, 1, 1, 0);
            }

            if (i == selectedIndex)
            {
                controlButtons[i].Select();
            }
        }
    }

    private void SelectControlScheme()
    {
        selectedControlScheme = selectedIndex switch
        {
            0 => "WASD",
            1 => "Arrows",
            _ => selectedControlScheme
        };

        PlayerPrefs.SetString("ControlScheme", selectedControlScheme);
        PlayerPrefs.Save();

        Debug.Log($"Selected control scheme: {selectedControlScheme}");
    }

    private void CloseHotKeysPanel()
    {
        HotkeysPanel.SetActive(false);
        SettingsMenuPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }
    private void HandleEscape()
    {
        if (HotkeysPanel.activeSelf)
        {
            CloseHotKeysPanel();
            ReturnToSettingsMenu();
        }

        if (MainMenuPanel.activeSelf)
        {
            CloseHotKeysPanel();
            ReturnToSettingsMenu();
        }
    }

    private void ReturnToSettingsMenu()
    {
        // Отключаем панель горячих клавиш и включаем панель настроек
        HotkeysPanel.SetActive(false);
        SettingsMenuPanel.SetActive(true);
        MainMenuPanel.SetActive(false);

        // Фокусируемся на первом элементе в меню настроек
        var settingsMenuButtons = SettingsMenuPanel.GetComponentsInChildren<Button>(true);
        if (settingsMenuButtons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(settingsMenuButtons[0].gameObject);
        }
        else
        {
            Debug.LogWarning("Нет кнопок в SettingsMenuPanel!");
        }
    }
    
    public static string GetControlScheme()
    {
        return PlayerPrefs.GetString("ControlScheme", "WASD");
    }
}


    //далее в скрипте который отвечает за логику движения нужно будет написать что-то вроде
     // void Update()
     // {
     //     string controlScheme = HotkeysPanelController.GetControlScheme();
     //
     //     if (controlScheme == "WASD")
     //     {
     //         // Управление через WASD
     //         if (Input.GetKey(KeyCode.W)) MoveUp();
     //         if (Input.GetKey(KeyCode.A)) MoveLeft();
     //         if (Input.GetKey(KeyCode.S)) MoveDown();
     //         if (Input.GetKey(KeyCode.D)) MoveRight();
     //     }
     //     else if (controlScheme == "Arrows")
     //     {
     //         // Управление через стрелки
     //         if (Input.GetKey(KeyCode.UpArrow)) MoveUp();
     //         if (Input.GetKey(KeyCode.LeftArrow)) MoveLeft();
     //         if (Input.GetKey(KeyCode.DownArrow)) MoveDown();
     //         if (Input.GetKey(KeyCode.RightArrow)) MoveRight();
     //     }
     // }