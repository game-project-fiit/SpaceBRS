using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class VolumeController : MonoBehaviour
{
    public Slider slider;      
    public TextMeshProUGUI sliderText;    
    public AudioSource audioSource; 
    public float incrementAmount = 1f;
    private bool canChangeVolume = false;

    void Start()
    {
        UpdateText();
        slider.onValueChanged.AddListener(delegate { UpdateText(); });
        audioSource.volume = slider.value / 100f; 
    }

    void Update()
    {
        if (canChangeVolume)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                slider.value = Mathf.Clamp(slider.value + incrementAmount, slider.minValue, slider.maxValue);
                UpdateAudioVolume();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                slider.value = Mathf.Clamp(slider.value - incrementAmount, slider.minValue, slider.maxValue);
                UpdateAudioVolume();
            }
        }
    }

    public void SetVolumeControlActive(bool active)
    {
        canChangeVolume = active; 
    }

    public void SetVolume(float value)
    {
        slider.value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
        UpdateAudioVolume();
        UpdateText(); 
    }

    void UpdateText()
    {
        float value = slider.value; 
        sliderText.text = "Music Volume: " + Mathf.RoundToInt(value).ToString() + "%"; 
        UpdateAudioVolume(); 
    }

    void UpdateAudioVolume()
    {
        audioSource.volume = slider.value / 100f; 
    }
}
