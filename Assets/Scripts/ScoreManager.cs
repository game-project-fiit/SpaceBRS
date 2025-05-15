using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score;
    private AudioSource audioSource;
    public AudioClip scoreSound, decreasingScoreSound;
    public TextMeshProUGUI scoreText;
    public bool decreaseScore;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
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

    private void PlaySound(AudioClip clip, float volume)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip, volume);
    }
    
    public int GetScore()
    {
        return score; 
    }

}