using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class HotkeysPanelController : MonoBehaviour
{
	public Button[] controlButtons;
	[FormerlySerializedAs("HotkeysPanel")] public GameObject hotkeysPanel;
	[FormerlySerializedAs("SettingsMenuPanel")] public GameObject settingsMenuPanel;
	[FormerlySerializedAs("MainMenuPanel")] public GameObject mainMenuPanel;
	public AudioClip moveSound;
	public AudioClip selectSound;
	private AudioSource audioSource;
	private int selectedIndex = 0;
	private string selectedControlScheme;

	public static string GetControlScheme() =>
		PlayerPrefs.GetString("ControlScheme", "WASD");

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		if (!audioSource)
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
		if (!hotkeysPanel.activeSelf) return;
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
			SelectControlScheme();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PlaySound(selectSound);
			HandleEscape();
		}
	}

	private void PlaySound(AudioClip clip)
	{
		if (audioSource && clip)
			audioSource.PlayOneShot(clip);
		else
			Debug.LogWarning("AudioSource or AudioClip is missing!");
	}

	private void UpdateButtonSelection()
	{
		for (var i = 0; i < controlButtons.Length; i++)
		{
			var buttonImage = controlButtons[i].GetComponent<Image>();
			var text = controlButtons[i].GetComponentInChildren<TextMeshProUGUI>();

			if (text is not null)
				text.color = i == selectedIndex
					? Color.black
					: Color.white;

			if (buttonImage is not null)
				buttonImage.color = i == selectedIndex
					? Color.white
					: new(1, 1, 1, 0);

			if (i == selectedIndex)
				controlButtons[i].Select();
		}
	}

	private void SelectControlScheme()
	{
		selectedControlScheme = selectedIndex switch
		{
			0 => "Arrows",
			1 => "WASD",
			_ => selectedControlScheme
		};

		PlayerPrefs.SetString("ControlScheme", selectedControlScheme);
		PlayerPrefs.Save();
		Debug.Log($"Selected control scheme: {selectedControlScheme}");
	}

	private void CloseHotKeysPanel()
	{
		hotkeysPanel.SetActive(false);
		settingsMenuPanel.SetActive(true);
		mainMenuPanel.SetActive(false);
	}

	private void HandleEscape()
	{
		if (hotkeysPanel.activeSelf)
		{
			CloseHotKeysPanel();
			ReturnToSettingsMenu();
		}

		if (!mainMenuPanel.activeSelf) return;

		CloseHotKeysPanel();
		ReturnToSettingsMenu();
	}

	private void ReturnToSettingsMenu()
	{
		hotkeysPanel.SetActive(false);
		settingsMenuPanel.SetActive(true);
		mainMenuPanel.SetActive(false);

		var settingsMenuButtons = settingsMenuPanel.GetComponentsInChildren<Button>(true);

		if (settingsMenuButtons.Length > 0)
			EventSystem.current.SetSelectedGameObject(settingsMenuButtons[0].gameObject);
		else
			Debug.LogWarning("Нет кнопок в SettingsMenuPanel!");
	}
}