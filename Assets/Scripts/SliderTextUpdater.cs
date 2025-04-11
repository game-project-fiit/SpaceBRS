using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class SliderTextUpdater : MonoBehaviour
{
    public Slider slider;      // Ссылка на ваш ползунок
    public TextMeshProUGUI sliderText;    // Ссылка на текстовый элемент
    public AudioSource audioSource; // Ссылка на AudioSource для управления громкостью
    public float incrementAmount = 1f; // Шаг изменения значения ползунка

    void Start()
    {
        // Установите начальное значение текста
        UpdateText();
        // Подпишитесь на событие изменения значения ползунка
        slider.onValueChanged.AddListener(delegate { UpdateText(); });
        
        // Установите начальную громкость
        audioSource.volume = slider.value / 100f; // Преобразуем значение ползунка в диапазон от 0 до 1
    }

    void Update()
    {
        // Проверяем нажатие клавиш стрелок
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Увеличиваем значение ползунка
            slider.value = Mathf.Clamp(slider.value + incrementAmount, slider.minValue, slider.maxValue);
            UpdateAudioVolume();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Уменьшаем значение ползунка
            slider.value = Mathf.Clamp(slider.value - incrementAmount, slider.minValue, slider.maxValue);
            UpdateAudioVolume();
        }
    }

    void UpdateText()
    {
        // Обновите текст с текущим значением ползунка в процентах
        float value = slider.value; // Получите текущее значение ползунка
        sliderText.text = "Music Value: " + Mathf.RoundToInt(value).ToString() + "%"; // Обновите текст
        UpdateAudioVolume(); // Обновите громкость музыки
    }

    void UpdateAudioVolume()
    {
        // Обновите громкость AudioSource в зависимости от значения ползунка
        audioSource.volume = slider.value / 100f; // Преобразуем значение ползунка в диапазон от 0 до 1
    }
}