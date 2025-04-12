using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class SliderTextColor : MonoBehaviour
{
    public Slider slider; // Ползунок
    public TextMeshProUGUI sliderText; // Текст ползунка

    void Start()
    {
        UpdateTextColor(); // Установить начальный цвет текста
        slider.onValueChanged.AddListener(delegate { UpdateTextColor(); }); // Добавить слушатель изменения значения
    }

    void UpdateTextColor()
    {
        // Получаем текущее значение ползунка в процентах
        float fillPercentage = (slider.value - slider.minValue) / (slider.maxValue - slider.minValue);

        // Проходим по каждому символу текста
        for (int i = 0; i < sliderText.text.Length; i++)
        {
            // Проверяем, прошло ли заполнение через текущий символ
            if ((float)i / sliderText.text.Length <= fillPercentage)
            {
                // Устанавливаем цвет символа в белый
                sliderText.text = sliderText.text.Insert(i, "<color=white>");
                sliderText.text = sliderText.text.Insert(i + 13, "</color>");
                i += 12; // Пропускаем теги
            }
        }
    }
}