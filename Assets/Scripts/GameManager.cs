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
				EndGameWithAutomaticWin();
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

			if (Input.GetKeyDown(KeyCode.Return))
			{
				if (isPaused)
					ResumeGame();
				else
					EndGame();
			}
		}

		if (isGameOver && Input.GetKeyDown(KeyCode.Return))
			ExitGame();
	}

	private void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0;
		pausePanel.SetActive(true);
		gamePanel.SetActive(false);
	}

	private void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1;
		pausePanel.SetActive(false);
		gamePanel.SetActive(true);
	}

	private void EndGame()
	{
		isGameActive = false;
		isGameOver = true;
		cometSpawner.ClearAllComets();

		var finalScore = ScoreManager.instance.GetScore();
		var resultIndex = finalScore switch
		{
			< 40 => 0,
			< 60 => 1,
			< 80 => 2,
			_ => 3
		};

		foreach (var img in resultImages)
			img.gameObject.SetActive(false);

		resultImages[resultIndex].gameObject.SetActive(true);
		tapEnterText.gameObject.SetActive(true);
		tapEnterText.text = "Tap Enter";

		audioSource.PlayOneShot(finalScore < 40 ? gameOverSound : victorySound);

		if (cometSpawner)
			cometSpawner.StopSpawning();
	}

	private void EndGameWithAutomaticWin()
	{
		isGameActive = false;
		isGameOver = true;
		cometSpawner.ClearAllComets();

		foreach (var img in resultImages)
			img.gameObject.SetActive(false);

		resultImages[4].gameObject.SetActive(true);
		tapEnterText.gameObject.SetActive(true);
		tapEnterText.text = "Tap Enter";
		audioSource.PlayOneShot(victorySound);

		if (cometSpawner)
			cometSpawner.StopSpawning();
	}

	private static void ExitGame()
		=> SceneManager.LoadScene("LevelsMenu");
}