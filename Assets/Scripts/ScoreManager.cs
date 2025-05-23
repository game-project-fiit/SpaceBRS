using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;
	private int score;
	private AudioSource audioSource;
	public AudioClip scoreSound, decreasingScoreSound;
	public TextMeshProUGUI scoreText;
	public bool decreaseScore;

	private void Start()
		=> audioSource = GetComponent<AudioSource>();

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
			//DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}

	private void UpdateScoreText()
	{
		if (scoreText)
			scoreText.text = "Score: " + score;
	}

	public void ChangeScore(int amount, bool positive)
	{
		score += positive ? amount : -amount;
		UpdateScoreText();
		PlaySound(
			positive ? scoreSound : decreasingScoreSound,
			positive ? 1.0f : 0.2f
		);
	}

	public void ResetScore()
	{
		score = 0;
		UpdateScoreText();
	}

	private void PlaySound(AudioClip clip, float volume)
	{
		if (clip && audioSource)
			audioSource.PlayOneShot(clip, volume);
	}

	public int GetScore() => score;
}