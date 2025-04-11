using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundsSlider;
    public TextMeshProUGUI musicSliderText;
    public TextMeshProUGUI soundsSliderText;
    public AudioSource musicAudioSource;
    public AudioSource soundsAudioSource;
    public float incrementAmount = 1f;
    private bool canChangeVolume = false;

    void Start()
    {
        UpdateMusicText();
        UpdateSoundsText();

        musicSlider.onValueChanged.AddListener(delegate { UpdateMusicText(); });
        soundsSlider.onValueChanged.AddListener(delegate { UpdateSoundsText(); });

        musicAudioSource.volume = musicSlider.value / 100f;
        soundsAudioSource.volume = soundsSlider.value / 100f;
    }

    void Update()
    {
        if (canChangeVolume)
        {
            // Управление музыкой
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                musicSlider.value = Mathf.Clamp(musicSlider.value + incrementAmount, musicSlider.minValue,
                    musicSlider.maxValue);
                UpdateMusicAudioVolume();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                musicSlider.value = Mathf.Clamp(musicSlider.value - incrementAmount, musicSlider.minValue,
                    musicSlider.maxValue);
                UpdateMusicAudioVolume();
            }

            // Управление звуками
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                soundsSlider.value = Mathf.Clamp(soundsSlider.value + incrementAmount, soundsSlider.minValue,
                    soundsSlider.maxValue);
                UpdateSoundsAudioVolume();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                soundsSlider.value = Mathf.Clamp(soundsSlider.value - incrementAmount, soundsSlider.minValue,
                    soundsSlider.maxValue);
                UpdateSoundsAudioVolume();
            }
        }
    }

    public void SetVolumeControlActive(bool active)
    {
        canChangeVolume = active;
    }

    public void SetMusicVolume(float value)
    {
        musicSlider.value = Mathf.Clamp(value, musicSlider.minValue, musicSlider.maxValue);
        UpdateMusicAudioVolume();
        UpdateMusicText();
    }

    public void SetSoundsVolume(float value)
    {
        soundsSlider.value = Mathf.Clamp(value, soundsSlider.minValue, soundsSlider.maxValue);
        UpdateSoundsAudioVolume();
        UpdateSoundsText();
    }

    void UpdateMusicText()
    {
        float value = musicSlider.value;
        musicSliderText.text = "Music Volume: " + Mathf.RoundToInt(value).ToString() + "%";
        UpdateMusicAudioVolume();
    }

    void UpdateSoundsText()
    {
        float value = soundsSlider.value;
        soundsSliderText.text = "Sounds Volume: " + Mathf.RoundToInt(value).ToString() + "%";
        UpdateSoundsAudioVolume();
    }

    void UpdateMusicAudioVolume()
    {
        musicAudioSource.volume = musicSlider.value / 100f;
        Debug.Log("Music Volume set to: " + musicAudioSource.volume); // Отладочное сообщение
    }

    void UpdateSoundsAudioVolume()
    {
        soundsAudioSource.volume = soundsSlider.value / 100f;
        Debug.Log("Sounds Volume set to: " + soundsAudioSource.volume); // Отладочное сообщение
    }
}