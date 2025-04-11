using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    public Slider musicSlider; 
    public Slider soundsSlider; 
    public Button hotkeysButton; 
    public TextMeshProUGUI[] settingTexts; 
    public AudioClip moveSound; 
    public AudioClip selectSound;
    private AudioSource audioSource;
    private int selectedIndex = 0; 
    public VolumeController volumeController; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        selectedIndex = 0; 
        UpdateMenu(); 
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChange); 
    }

    void Update()
    {
        if (settingTexts.Length == 0) return;

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
                break;
        }
    }

    void OnMusicVolumeChange(float value)
    {
        volumeController.SetVolume(value); 
        Debug.Log("Music Volume set to: " + value);
    }
}
