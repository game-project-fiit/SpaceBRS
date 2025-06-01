using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public float gameDuration = 5f;
	public CometSpawner cometSpawner;
	public Slider timeSlider;
	public Image[] resultImages;
	public TextMeshProUGUI tapEnterText;
	public AudioClip gameOverSound;
	public AudioClip victorySound;
	private AudioSource audioSource;
	public GameObject pausePanel;
	public GameObject gamePanel;
	private bool isGameActive;
	private bool isGameOver;
	private bool isPaused;
	private bool automaticWin;
	private bool nextLevelAvailable;

	private void Start()
	{
		timeSlider.maxValue = gameDuration;
		timeSlider.value = gameDuration;

		foreach (var img in resultImages)
			img.gameObject.SetActive(false);

		tapEnterText.gameObject.SetActive(false);
		pausePanel.SetActive(false);
		gamePanel.SetActive(true);

		audioSource = gameObject.AddComponent<AudioSource>();
	}

	public void StartGame()
	{
		isGameActive = true;
		isGameOver = false;
		StartCoroutine(GameTimer());
		if (cometSpawner)
			cometSpawner.StartSpawning();
	}

	private IEnumerator GameTimer()
	{
		var elapsedTime = 0f;
		while (elapsedTime < gameDuration && isGameActive)
		{
			elapsedTime += Time.deltaTime;
			timeSlider.value = gameDuration - elapsedTime;

			if (ScoreManager.instance.GetScore() >= 100)
			{
				automaticWin = true;
				EndGame();
				yield break;
			}

			yield return null;
		}

		if (isGameActive)
			EndGame();
	}

	private void Update()
	{
		if (isGameActive)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (isPaused)
					ExitGame();
				else
					PauseGame();
			}

			if (isPaused && Input.GetKeyDown(KeyCode.Return))
                ResumeGame();
		}

        if (isGameOver)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				ExitGame();

			if (Input.GetKeyDown(KeyCode.Return))
			{
				if (nextLevelAvailable)
					LoadNextLevel();
				else
					ExitGame();
			}
		}
	}

	private void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0;
		pausePanel.SetActive(true);
		gamePanel.SetActive(false);

		if (cometSpawner != null)
			cometSpawner.StopSpawning();
	}

	private void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1;
		pausePanel.SetActive(false);
		gamePanel.SetActive(true);

		if (cometSpawner != null)
		{
			cometSpawner.StopSpawning();
			cometSpawner.StartSpawning();
		}
	}

	private void EndGame()
	{
		isGameActive = false;
		isGameOver = true;
		cometSpawner.ClearAllComets();

		var finalScore = ScoreManager.instance.GetScore();
		var currentScene = gameObject.scene.name;
		var prefsKey = $"BestScore_{currentScene}";
		var oldBest = PlayerPrefs.GetInt(prefsKey, 0);

		if (finalScore > oldBest)
		{
			PlayerPrefs.SetInt(prefsKey, finalScore);
			PlayerPrefs.Save();
		}

		ScoreManager.instance.ResetScore();

		var resultIndex = automaticWin
			? 4
			: finalScore switch
			{
				< 40 => 0,
				< 60 => 1,
				< 80 => 2,
				_ => 3
			};
		if (resultIndex != 0)
			nextLevelAvailable = true;

		foreach (var img in resultImages)
			img.gameObject.SetActive(false);

		resultImages[resultIndex].gameObject.SetActive(true);
		tapEnterText.gameObject.SetActive(true);
		tapEnterText.text = "Tap Enter";

		audioSource.PlayOneShot(finalScore < 40 ? gameOverSound : victorySound);

		if (cometSpawner)
			cometSpawner.StopSpawning();
	}

	private static void ExitGame()
	{
        SceneManager.LoadScene("LevelsMenu");
		Time.timeScale = 1f;
    }
	
	private void LoadNextLevel()
	{
		switch (gameObject.scene.name)
		{
			case "VVMLevel":
				SceneManager.LoadScene("Term1Level");
				break;
			case "Term1Level":
				SceneManager.LoadScene("Term2Level");
				break;
			case "Term2Level":
				ExitGame();
				break;
		}
	}
}