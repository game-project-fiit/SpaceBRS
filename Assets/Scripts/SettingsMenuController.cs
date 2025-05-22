using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Audio;

public class SettingsMenuController : MonoBehaviour
{
	public static SettingsMenuController instance;
	public GameObject optionsPanel;
	public Slider musicSlider;
	public Slider soundSlider;
	public TextMeshProUGUI[] settingTexts;

	[FormerlySerializedAs("SettingsMenuPanel")]
	public GameObject settingsMenuPanel;

	[FormerlySerializedAs("MainMenuPanel")]
	public GameObject mainMenuPanel;

	[FormerlySerializedAs("HotkeysPanel")] public GameObject hotkeysPanel;
	public GameObject storyPanel;

	public AudioSource audioSource;
	public AudioClip audioClip;
	public AudioClip clickClip;
	public AudioMixer audioMixer;

	private int selectedIndex;

	private void Awake()
		=> instance = this;

	private void Start()
	{
		UpdateMenu();
		hotkeysPanel.SetActive(false);
	}

	private void Update()
	{
		if (!settingsMenuPanel.activeSelf) return;
		if (settingTexts.Length == 0) return;

		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			selectedIndex = (selectedIndex - 1 + settingTexts.Length) % settingTexts.Length;
			PlayNavigateSounds();
			UpdateMenu();
		}

		else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) // Зачем еще S для переключения?
		{
			selectedIndex = (selectedIndex + 1) % settingTexts.Length;
            PlayNavigateSounds();
            UpdateMenu();
		}

		if (Input.GetKey(KeyCode.LeftArrow))
			AdjustVolume(-0.3f * Time.deltaTime);
		else if (Input.GetKey(KeyCode.RightArrow))
			AdjustVolume(0.3f * Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.Return))
		{
			PlayClickSounds();
			SelectOption();
        }

		if (Input.GetKeyDown(KeyCode.Escape))
			HandleEscape();
	}

	private void HandleEscape()
	{
		if (!settingsMenuPanel.activeSelf) return;

		CloseSettingsPanel();
		ReturnToMainMenu();
	}

	private void UpdateMenu()
	{
		for (var i = 0; i < settingTexts.Length; i++)
			settingTexts[i].text = i == selectedIndex
				? $"> {GetOptionText(i)}"
				: GetOptionText(i);
	}

	private void PlayNavigateSounds()
	{
		if (audioSource && audioClip)
			audioSource.PlayOneShot(audioClip, soundSlider.value);
	}

	private void PlayClickSounds()
	{
        if (audioSource && clickClip)
            audioSource.PlayOneShot(clickClip, soundSlider.value);
    }

    private string GetOptionText(int index)
		=> index switch
		{
			0 => $"Music Volume: {Mathf.RoundToInt(musicSlider.value * 100)}%",
			1 => $"Sound Volume: {Mathf.RoundToInt(soundSlider.value * 100)}%",
			2 => "Hotkeys",
			3 => "See the plot again",
			_ => ""
		};

	private void AdjustVolume(float change)
	{
		switch (selectedIndex)
		{
			case 0:
				musicSlider.value = Mathf.Clamp01(musicSlider.value + change);
				AudioManager.Instance.SetMusicVolume(musicSlider.value);
				break;
			case 1:
				soundSlider.value = Mathf.Clamp01(soundSlider.value + change);
                AudioManager.Instance.setSVXVolume(soundSlider.value);
                break;
		}

		UpdateMenu();
	}

	private void ReturnToMainMenu()
	{
		settingsMenuPanel.SetActive(false);
		enabled = false;
		mainMenuPanel.SetActive(true);

		var mainMenuController = mainMenuPanel.GetComponentInParent<MainMenuController>();
		if (mainMenuController)
		{
			mainMenuController.enabled = true;
			mainMenuController.ResetSelection();
		}

		var mainMenuButtons = mainMenuPanel.GetComponentsInChildren<Button>(true);
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
		if (storyPanel)
		{
			storyPanel.SetActive(false);
			var storyController = storyPanel.GetComponent<StoryController>();

			if (storyController)
			{
				storyController.StopAllCoroutines();
				storyController.enabled = false;
				storyController.currentSlideIndex = 0;
			}
		}

		if (!hotkeysPanel || !settingsMenuPanel)
		{
			Debug.LogError("HotkeysPanel or SettingsMenuPanel is not assigned!");
			return;
		}

		hotkeysPanel.SetActive(true);
		settingsMenuPanel.SetActive(false);
		mainMenuPanel.SetActive(false);

		var hotkeysButtons = hotkeysPanel.GetComponentsInChildren<Button>(true);
		
		if (hotkeysButtons.Length > 0)
			EventSystem.current.SetSelectedGameObject(hotkeysButtons[0].gameObject);
		else
			Debug.LogWarning("No buttons found in HotkeysPanel!");
	}

	public void ReturnToOptionsAfterPlot()
	{
		settingsMenuPanel.SetActive(true);
		Debug.Log("Returning to options");
	}

	private void CloseSettingsPanel()
	{
		settingsMenuPanel.SetActive(false);
		mainMenuPanel.SetActive(true);
		UpdateMenu();
	}

	private void OpenStoryPanelForReplay()
	{
		if (!storyPanel)
		{
			Debug.Log("StoryPanel is not assigned!");
			return;
		}

		var storyController = storyPanel.GetComponent<StoryController>();
		
		if (storyController)
		{
			storyController.currentSlideIndex = 0;
			storyController.replayFromOptions = true;
			storyController.enabled = true;
		}

		storyPanel.SetActive(true);
		settingsMenuPanel.SetActive(false);
	}

	private void OnEnable()
	{
		musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
		soundSlider.value = PlayerPrefs.GetFloat("SVXVolume", 1f);

		selectedIndex = 0;
		UpdateMenu();
	}
}