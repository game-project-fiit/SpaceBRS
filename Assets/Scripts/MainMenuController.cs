using UnityEngine;
using UnityEngine.UI; // Не забудьте добавить этот namespace для работы с UI
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public Button[] menuButtons; // Массив кнопок
    public TextMeshProUGUI[] menuTexts; // Массив текстов для отображения
    public AudioClip moveSound;
    public AudioClip selectSound;
    public GameObject optionsPanel; // Ссылка на панель настроек
    private AudioSource audioSource;
    private int selectedIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateMenu();
    }

    void Update()
    {
        if (menuButtons.Length == 0) return; // Если массив пустой, ничего не делаем

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

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    void UpdateMenu()
    {
        for (var i = 0; i < menuButtons.Length; i++)
        {
            if (i == selectedIndex)
            {
                // Добавляем символ '>' к выделенному пункту
                menuTexts[i].text = ">" + menuTexts[i].text.TrimStart('>');
                menuButtons[i].Select(); // Выбор кнопки для управления фокусом
            }
            else
            {
                // Убираем символ '>' от невыделенных пунктов
                menuTexts[i].text = menuTexts[i].text.TrimStart('>');
            }
        }
    }

    void ShowOptionsPanel()
    {
        gameObject.SetActive(false); // Скрываем главное меню
        optionsPanel.SetActive(true); // Активируем панель настроек
        UpdateOptionsTextVisibility(true); // Показываем текст панели настроек
    }

    void UpdateOptionsTextVisibility(bool isVisible)
    {
        // Предполагаем, что у вас есть ссылки на тексты в optionsPanel
        TextMeshProUGUI[] optionTexts = optionsPanel.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in optionTexts)
        {
            text.gameObject.SetActive(isVisible); // Устанавливаем видимость текста
        }
    }

    void SelectOption()
    {
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("Start Game");
                // Здесь можно добавить код для начала игры
                break;
            case 1:
                Debug.Log("Options");
                ShowOptionsPanel(); // Переход на панель настроек
                break;
            case 2:
                Debug.Log("Quit Game");
                Application.Quit();
                break;
        }
    }
}
