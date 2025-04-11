using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    public Slider musicSlider; 
    public Slider soundsSlider; 
    public Button hotkeysButton; 
    public Button backButton; // Добавляем кнопку "Back"
    public TextMeshProUGUI[] settingTexts; 
    public AudioClip moveSound; 
    public AudioClip selectSound;
    private AudioSource audioSource;
    private int selectedIndex = 0; 
    public VolumeController volumeController; 
    public GameObject settingsPanel; // Панель настроек
    public GameObject mainMenuPanel; // Панель главного меню

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        selectedIndex = 0; 
        UpdateMenu(); 
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChange); 
        soundsSlider.onValueChanged.AddListener(OnSoundsVolumeChange);
    
        // Добавляем слушатель для кнопки "Back"
        backButton.onClick.AddListener(ReturnToMainMenu);
        Debug.Log("Back button listener added");
    }


    void Update()
    {
        if (settingTexts.Length == 0) return;

        // Навигация по меню
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + settingTexts.Length) % settingTexts.Length;
            PlaySound(moveSound);
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % settingTexts.Length;
            PlaySound(moveSound);
            UpdateMenu();
        }
        
        // Управление значением громкости музыки
        if (selectedIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                musicSlider.value = Mathf.Clamp(musicSlider.value + 1, musicSlider.minValue, musicSlider.maxValue);
                OnMusicVolumeChange(musicSlider.value);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                musicSlider.value = Mathf.Clamp(musicSlider.value - 1, musicSlider.minValue, musicSlider.maxValue);
                OnMusicVolumeChange(musicSlider.value);
            }
        }
        
        // Управление значением громкости звуков
        if (selectedIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                soundsSlider.value = Mathf.Clamp(soundsSlider.value + 1, soundsSlider.minValue, soundsSlider.maxValue);
                OnSoundsVolumeChange(soundsSlider.value);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                soundsSlider.value = Mathf.Clamp(soundsSlider.value - 1, soundsSlider.minValue, soundsSlider.maxValue);
                OnSoundsVolumeChange(soundsSlider.value);
            }
        }

        // Обработка нажатия клавиши Enter для выбора опции
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlaySound(selectSound);
            SelectOption();
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    void UpdateMenu()
    {
        for (var i = 0; i < settingTexts.Length; i++)
        {
            if (i == selectedIndex)
            {
                settingTexts[i].text = ">" + settingTexts[i].text.TrimStart('>');
            }
            else
            {
                settingTexts[i].text = settingTexts[i].text.TrimStart('>');
            }
        }
    }

    void SelectOption()
    {
        switch (selectedIndex)
        {
            case 0:
                volumeController.SetVolumeControlActive(true); 
                Debug.Log("Adjusting Music Volume");
                break;
            case 1:
                volumeController.SetVolumeControlActive(false); 
                Debug.Log("Adjusting Sounds Volume");
                break;
            case 2:
                Debug.Log("Opening Hotkeys Menu");
                // Здесь можно добавить логику для открытия меню горячих клавиш
                break;
            case 3: // Обработка кнопки "Back"
                ReturnToMainMenu(); // Вызов метода для возврата в главное меню
                break;
        }
    }

    void ReturnToMainMenu()
    {
        Debug.Log("Attempting to return to Main Menu");

        // Деактивируем панель настроек
        settingsPanel.SetActive(false); 
        Debug.Log("Settings panel deactivated");

        // Активируем панель главного меню
        mainMenuPanel.SetActive(true);
        Debug.Log("Main menu panel activated");

        // Убедитесь, что кнопки главного меню активны
        foreach (Transform child in mainMenuPanel.transform)
        {
            if (child.GetComponent<Button>() != null)
            {
                child.gameObject.SetActive(true); // Убедитесь, что каждая кнопка активна
                Debug.Log("Button " + child.name + " activated");
            }
        }
    }


    void OnMusicVolumeChange(float value)
    {
        volumeController.SetMusicVolume(value); 
        Debug.Log("Music Volume set to: " + value);
    }

    void OnSoundsVolumeChange(float value)
    {
        volumeController.SetSoundsVolume(value); 
        Debug.Log("Sounds Volume set to: " + value);
    }
}
